#!/usr/bin/python
# -*- coding: utf-8 -*-

import locale
import logging
import logging.handlers
import os
import os.path
import platform
import re
import StringIO
import sys
import thread
import traceback
import urllib2
import urlparse

from win32com.shell import shell, shellcon
import wx

from converter import Converter
from downloader import Downloader
import mainform
import metadata
import pyperclip
from youtubedl_proxy import YoutubeDlProxy


def _app_icon_path():
    if os.path.isfile(metadata.icon):
        return metadata.icon
    elif sys.argv[0].lower().endswith(".exe"):
        return sys.argv[0]
    return None


def _bitmap_from_file(file_path, max_size=(None, None)):
    """Create a wx.Bitmap from an image file"""
    if not os.path.isfile(file_path):
        return wx.NullBitmap
    return _bitmap_from_image(wx.Image(file_path), max_size)


def _bitmap_from_image(image, max_size=(None, None)):
    """Create a wx.Bitmap from a wx.Image"""
    max_width = max_size[0]
    max_height = max_size[1]

    width = image.GetWidth()
    height = image.GetHeight()
    oversized = False

    if max_width and width > max_width:
        max_width = float(max_width)
        oversized = True
        ratio = max_width / width
        width = max_width
        height *= ratio

    if max_height and height > max_height:
        max_height = float(max_height)
        oversized = True
        ratio = max_height / height
        width *= ratio
        height = max_height

    if oversized:
        image = image.Scale(width, height)

    return wx.BitmapFromImage(image)


def _bitmap_from_string(string, max_size=(None, None)):
    """Create a wx.Bitmap from a string buffer"""
    stream = StringIO.StringIO(string)
    image = wx.ImageFromStream(stream)
    stream.close()
    return _bitmap_from_image(image, max_size)


# When the form window is activated, reading clipboard data using wxPython API
# causes lag. Thus, using the pyperclip API instead.
#
# def _clipboard_url():
#     if not wx.TheClipboard.IsOpened():
#         text_data = wx.TextDataObject()
#
#         wx.TheClipboard.Open()
#         success = wx.TheClipboard.GetData(text_data)
#         wx.TheClipboard.Close()
#
#         if success:
#             url = text_data.GetText()
#             parse_result = urlparse.urlparse(url)
#             if parse_result.scheme and parse_result.netloc:
#                 return url
#     return None
#
def _clipboard_url():
    if not wx.TheClipboard.IsOpened():
        text = pyperclip.paste()
        if text:
            parse_result = urlparse.urlparse(text)
            if parse_result.scheme and parse_result.netloc:
                return text
    return None


def _desktop_dir():
    return shell.SHGetFolderPath(0, shellcon.CSIDL_DESKTOPDIRECTORY, None, 0)


def _document_dir():
    sp = wx.StandardPaths.Get()
    func = getattr(sp, u"GetDocumentsDir")
    return func()


def _default_download_dir():
    if platform.system() == u"Windows":
        return _desktop_dir()
    return _document_dir()


def _download_thumbnail(url, callback):
    info_dict = dict()
    if url:
        try:
            response = urllib2.urlopen(url)
        except urllib2.HTTPError as e:
            _g_logger.exception(u"Failed to access {0}".format(url))
            info_dict["error"] = e
        except urllib2.URLError as e:
            _g_logger.exception(u"Failed to access {0}".format(url))
            info_dict["error"] = e
        else:
            data = response.read()
            response.close()
            info_dict["data"] = _bitmap_from_string(data, max_size=(None, 49))
    wx.CallAfter(callback, info_dict)


def _error_hook(error_type, value, tb):
    message = u"".join(traceback.format_exception(error_type, value, tb))
    dialog = wx.MessageDialog(None, message, u"Error", wx.OK | wx.ICON_ERROR)
    dialog.ShowModal()
    dialog.Destroy()
    sys.exit(message)


def _initialize_log():
    console_handler = logging.StreamHandler()
    console_handler.setLevel(logging.INFO)
    formatter = logging.Formatter(u"%(name)-12s [%(levelname)s] %(message)s")
    console_handler.setFormatter(formatter)

    file_path = u"log.txt"
    log_dir = os.path.dirname(file_path)
    if log_dir and not os.path.isdir(log_dir):
        os.mkdir(os.path.dirname(file_path))
    print(u"Log file: {0}".format(os.path.abspath(file_path))
          .encode(_preferred_encoding(), 'ignore'))

    file_handler = logging.handlers.TimedRotatingFileHandler(
        file_path, when="midnight", encoding="utf8")
    file_handler.setLevel(logging.DEBUG)
    formatter = logging.Formatter(
        u"%(asctime)s - %(name)s [%(levelname)s] %(message)s")
    file_handler.setFormatter(formatter)

    logger = logging.getLogger()
    logger.setLevel(logging.DEBUG)
    logger.addHandler(console_handler)
    logger.addHandler(file_handler)


def _parse_url(url, callback):
    proxy = YoutubeDlProxy(url)
    wx.CallAfter(callback, proxy.download_tag)


def _preferred_encoding():
    """Get preferred encoding.

    Returns the best encoding scheme for the system, based on
    locale.getpreferredencoding() and some further tweaks.

    """
    try:
        pref = locale.getpreferredencoding()
        u'TEST'.encode(pref)
    except UnicodeError:
        pref = 'UTF-8'
    return pref


def _prompt_to_save_file(parent, default_name, file_filter, callback):
        filename = _validate_filename(default_name)
        dialog = wx.FileDialog(
            parent,
            message=u"Save As",
            defaultDir=_default_download_dir(),
            defaultFile=filename,
            wildcard=file_filter,
            style=wx.SAVE | wx.FD_OVERWRITE_PROMPT,
        )
        destination = None
        if dialog.ShowModal() == wx.ID_OK:
            destination = dialog.GetPath()
            _g_logger.info(u"Save as: {0}".format(destination))
        dialog.Destroy()
        wx.CallAfter(callback, destination)


def _validate_filename(filename):
    """Remove invalid file name characters"""
    filename = re.sub(r"[\\/:*?<>|]", "", filename)
    filename = filename.replace('"', "'")
    if not filename:
        filename = u"Untitled"
    return filename


class Controller:
    def __init__(self):
        self._conversion_tag = None

        self._converter = Converter()
        self._converter.complete_callback = self._on_convert_complete
        self._converter.progress_callback = self._on_convert_progress_change

        self._download_tag = None

        self._downloader = Downloader()
        self._downloader.complete_callback = self._on_download_complete
        self._downloader.progress_callback = self._on_download_progress_change

        self.view = mainform.MainForm(
            parent=None,
            title=metadata.title,
            icon_path=_app_icon_path(),
            conversions=Converter.get_conversions(),
        )

        self.view.url_textbox.Bind(wx.EVT_TEXT_ENTER, self._on_url_enter_press)
        self.view.option_button.Bind(wx.EVT_BUTTON, self._on_option_click)
        self.view.download_button.Bind(wx.EVT_BUTTON, self._on_download_click)
        self.view.cancel_button.Bind(wx.EVT_BUTTON, self._on_cancel_click)
        self.view.ok_button.Bind(wx.EVT_BUTTON, self._on_ok_click)
        self.view.Bind(wx.EVT_ACTIVATE, self._on_set_focus)
        self.view.Bind(wx.EVT_CLOSE, self._on_form_close)

        self.view.Show()

        _initialize_log()

    # self.view handlers
    #======================================================================
    def _on_cancel_click(self, event):
        # TODO: cancel _parse_url()

        self._downloader.cancel()
        self._converter.cancel()
        self.view.show_input_layout()

    def _on_download_click(self, event):
        self.view.show_loading_layout()

        conversion_combobox = self.view.conversion_combobox
        selected_index = conversion_combobox.GetSelection()
        self._conversion_tag = conversion_combobox.GetClientData(
            selected_index)

        url = self.view.url_textbox.GetValue()
        if not url:
            self.view.show_input_layout()
            return

        if os.path.isfile(url):
            if self._conversion_tag.is_valid():
                self._converter.convert_file(url, self._conversion_tag)
                return
            self.view.show_input_layout()
            return

        thread.start_new_thread(
            _parse_url, (url, self._on_parse_url_complete))

    def _on_form_close(self, event):
        # TODO: cancel _parse_url()
        self._downloader.cancel()
        self.view.Destroy()

    def _on_ok_click(self, event):
        self.view.show_input_layout()

    def _on_option_click(self, event):
        pass

    def _on_set_focus(self, e):
        if e.GetActive():
            url = _clipboard_url()
            if url:
                self.view.set_input_text(url)

    def _on_url_enter_press(self, e):
        self._on_download_click(e)

    # thread handlers
    #======================================================================
    def _on_convert_complete(self, event):
        self._notify_user(event.message)

    def _on_convert_progress_change(self, event):
        self.view.set_progress(event.percentage, event.message)

    def _on_download_complete(self, event):
        if event.error:
            self.view.error_dialog(event.message)
            self.view.show_input_layout()
            return

        if self._conversion_tag.is_valid():
            source = event.destination
            self._converter.convert_file(source, self._conversion_tag)
            return

        self._notify_user(event.message)

    def _on_download_thumbnail_complete(self, info_dict):
        if "data" in info_dict:
            bitmap = info_dict["data"]
            self.view.set_thumbnail(bitmap)

    def _on_download_progress_change(self, event):
        self.view.set_progress(event.percentage, event.message)

    def _on_parse_url_complete(self, download_tag):
        self._download_tag = download_tag

        if download_tag.error:
            self.view.error_dialog(download_tag.error)
            self.view.show_input_layout()
            return

        if download_tag.thumbnail_url:
            thread.start_new_thread(
                _download_thumbnail,
                (
                    download_tag.thumbnail_url,
                    self._on_download_thumbnail_complete
                )
            )

        title, file_ext = os.path.splitext(download_tag.filename)
        self.view.set_main_label(title)

        file_filter = u"(*{0})|*{0}".format(file_ext)

        thread.start_new_thread(
            _prompt_to_save_file,
            (
                self.view,
                title,
                file_filter,
                self._on_prompt_to_save_file_complete
            )
        )

    def _on_prompt_to_save_file_complete(self, destination):
        if not destination:
            self.view.show_input_layout()
            return

        self._downloader.download(self._download_tag.video_url,
                                  destination)

    # private functions
    #======================================================================
    def _notify_user(self, message):
        self.view.show_confirmation_layout(message)
        if not self.view.IsActive():
            self.view.RequestUserAttention()


_g_logger = logging.getLogger(__name__)


if __name__ == '__main__':
    sys.excepthook = _error_hook

    import sys
    sys.stderr = open("error.log", "w")

    app = wx.App()
    Controller()
    app.MainLoop()
