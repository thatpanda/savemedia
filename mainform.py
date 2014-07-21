#!/usr/bin/python
from __future__ import absolute_import
import logging

import wx
import wx.combo


def _bitmap_for_file_ext(extension):
    """Create a wx.Bitmap that represent the given file extension"""
    if not extension:
        return wx.NullBitmap

    file_type = wx.TheMimeTypesManager.GetFileTypeFromExtension(extension)
    if not file_type:
        return wx.NullBitmap

    icon_info = file_type.GetIconInfo()
    if not icon_info:
        return wx.NullBitmap

    icon, icon_file, icon_offset = icon_info
    if icon.Ok():
        return wx.BitmapFromIcon(icon)
    return wx.NullBitmap


def _image_combobox(parent, choices):
    bcb = wx.combo.BitmapComboBox(
        parent, style=wx.CB_READONLY | wx.TE_PROCESS_ENTER)
    for choice in choices:
        bcb.Append(item=choice.name,
                   bitmap=choice.bitmap,
                   clientData=choice,
                   )
    return bcb


class MainForm(wx.Frame):
    def __init__(self, parent, title, icon_path, conversions):
        super(MainForm, self).__init__(
            parent,
            size=(500, 280),
            style=(wx.MINIMIZE_BOX | wx.RESIZE_BORDER | wx.SYSTEM_MENU |
                   wx.CAPTION | wx.CLOSE_BOX),
            title=title
        )
        self._default_title = title

        if icon_path:
            self.SetIcon(wx.Icon(icon_path, wx.BITMAP_TYPE_ICO))

        for conversion in conversions:
            file_extension = conversion.display_ext
            conversion.bitmap = _bitmap_for_file_ext(file_extension)
        self._conversions = conversions

        self._input_panel = None
        self.url_textbox = None
        self.conversion_combobox = None

        self.progress_panel = None
        self.video_thumbnail = None
        self.main_label = None
        self.progress_label = None
        self.progressbar = None
        self.progress_sizer = None

        self.option_button = None
        self.bottom_right_panel = None
        self.download_button = None
        self.cancel_button = None
        self.ok_button = None
        self.bottom_right_sizer = None

        self.main_sizer = None
        self.main_panel = None

        self._initialize_ui()
        self.Centre()

    def _initialize_ui(self):
        self.main_panel = wx.Panel(self)
        self.main_panel.SetBackgroundColour(
            wx.SystemSettings.GetColour(wx.SYS_COLOUR_WINDOW))

        # input layout
        #======================================================================
        self._input_panel = wx.Panel(self.main_panel)

        self.url_textbox = wx.TextCtrl(self._input_panel,
                                       style=wx.TE_PROCESS_ENTER)
        self.url_textbox.SetHint(u"Video URL")

        self.conversion_combobox = _image_combobox(self._input_panel,
                                                   self._conversions)
        if self.conversion_combobox.GetCount > 0:
            self.conversion_combobox.SetSelection(0)

        input_sizer = wx.GridBagSizer()
        input_sizer.Add(self.url_textbox, pos=(0, 0), flag=wx.EXPAND)
        input_sizer.Add(self.conversion_combobox, pos=(1, 0),
                        flag=wx.EXPAND | wx.TOP, border=12)
        input_sizer.AddGrowableCol(0)
        self._input_panel.SetSizerAndFit(input_sizer)

        # progress layout
        #======================================================================
        self.progress_panel = wx.Panel(self.main_panel)
        self.progress_panel.SetDoubleBuffered(True)

        self.video_thumbnail = wx.StaticBitmap(self.progress_panel,
                                               bitmap=wx.NullBitmap)

        main_font = wx.SystemSettings_GetFont(wx.SYS_DEFAULT_GUI_FONT)
        main_font.SetPointSize(main_font.GetPointSize() + 2)

        self.main_label = wx.StaticText(self.progress_panel, label="")
        self.main_label.SetFont(main_font)

        self.progress_label = wx.StaticText(self.progress_panel, label="")

        self.progressbar = wx.Gauge(self.progress_panel, size=(50, 18))
        self.progressbar.SetValue(50)

        self.progress_sizer = wx.GridBagSizer(vgap=0, hgap=0)
        self.progress_sizer.Add(self.video_thumbnail, pos=(0, 0), span=(2, 1),
                                flag=wx.RIGHT, border=12)
        self.progress_sizer.Add(self.main_label, pos=(0, 1),
                                flag=wx.EXPAND | wx.LEFT | wx.RIGHT, border=0)
        self.progress_sizer.Add(self.progress_label, pos=(1, 1),
                                flag=wx.EXPAND | wx.TOP, border=6)
        self.progress_sizer.Add(self.progressbar, pos=(2, 0), span=(1, 2),
                                flag=wx.EXPAND | wx.TOP, border=12)
        self.progress_sizer.AddGrowableCol(1)
        self.progress_panel.SetSizerAndFit(self.progress_sizer)

        self.progress_panel.Hide()

        # bottom layout
        #======================================================================
        bottom_panel = wx.Panel(self.main_panel)
        bottom_panel.SetBackgroundColour(
            wx.SystemSettings.GetColour(wx.SYS_COLOUR_FRAMEBK))

        self.option_button = wx.Button(bottom_panel, label=u"&Options...",
                                       size=(80, 28))
        self.option_button.Hide()

        self.bottom_right_panel = wx.Panel(bottom_panel)
        self.bottom_right_panel.SetBackgroundColour(
            wx.SystemSettings.GetColour(wx.SYS_COLOUR_WINDOW))

        self.download_button = wx.Button(self.bottom_right_panel,
                                         label=u"Download", size=(80, 28))

        self.cancel_button = wx.Button(self.bottom_right_panel, wx.ID_CANCEL,
                                       size=(80, 28))
        self.cancel_button.Hide()

        self.ok_button = wx.Button(self.bottom_right_panel, wx.ID_OK,
                                   size=(80, 28))
        self.ok_button.Hide()

        self.bottom_right_sizer = wx.BoxSizer(wx.HORIZONTAL)
        self.bottom_right_sizer.Add(self.download_button)
        self.bottom_right_sizer.Add(self.cancel_button)
        self.bottom_right_sizer.Add(self.ok_button)
        self.bottom_right_panel.SetSizerAndFit(self.bottom_right_sizer)

        bottom_sizer = wx.GridBagSizer(vgap=0, hgap=0)
        bottom_sizer.Add(wx.StaticText(bottom_panel), pos=(0, 0),
                         flag=wx.LEFT, border=24)
        bottom_sizer.Add(self.option_button, pos=(0, 1),
                         flag=wx.TOP | wx.BOTTOM, border=10)
        bottom_sizer.Add(self.bottom_right_panel, pos=(0, 2),
                         flag=wx.ALIGN_RIGHT | wx.TOP | wx.BOTTOM, border=10)
        bottom_sizer.Add(wx.StaticText(bottom_panel), pos=(0, 3),
                         flag=wx.RIGHT, border=24)
        bottom_sizer.AddGrowableCol(2)
        bottom_panel.SetSizerAndFit(bottom_sizer)

        # main layout
        #======================================================================
        self.main_sizer = wx.BoxSizer(wx.VERTICAL)
        self.main_sizer.AddSpacer(12)
        self.main_sizer.Add(self._input_panel, 1,
                            flag=wx.EXPAND | wx.LEFT | wx.RIGHT, border=25)
        self.main_sizer.Add(self.progress_panel, 1,
                            flag=wx.EXPAND | wx.LEFT | wx.RIGHT, border=25)
        self.main_sizer.AddSpacer(12)
        self.main_sizer.Add(bottom_panel, flag=wx.EXPAND)
        self.main_sizer.SetMinSize((500, 150))
        self.main_panel.SetSizerAndFit(self.main_sizer)
        self.main_sizer.SetSizeHints(self)

    def error_dialog(self, message, title=u"Error"):
        _g_logger.error(message)
        dialog = wx.MessageDialog(self, message, title, wx.OK | wx.ICON_ERROR)
        dialog.ShowModal()
        dialog.Destroy()

    # setters
    #======================================================================
    def set_input_text(self, text):
        self.url_textbox.SetValue(text)

    def set_main_label(self, text):
        self.main_label.SetLabelText(text)
        self._update_layout()

    def set_progress(self, percentage, status):
        self.SetTitle(u"{0}% - {1}".format(percentage, self._default_title))
        self.progress_label.SetLabelText(status)
        self.progressbar.SetValue(percentage)

    def set_thumbnail(self, bitmap):
        self.video_thumbnail.SetBitmap(bitmap)
        self.video_thumbnail.Show()
        self._update_layout()

    # layout functions
    #======================================================================
    def show_input_layout(self):
        self._input_panel.Show()
        self.progress_panel.Hide()

        self.download_button.Show()
        self.cancel_button.Hide()
        self.ok_button.Hide()

        self.main_sizer.Layout()
        self.main_sizer.SetSizeHints(self)

    def show_loading_layout(self):
        self._input_panel.Hide()

        self.video_thumbnail.Hide()
        self.main_label.SetLabelText(u"Loading...")
        self.progress_label.SetLabelText("")
        self.progressbar.Pulse()
        self.progress_panel.Show()

        self.download_button.Hide()
        self.cancel_button.Show()

        self._update_layout()

    def show_confirmation_layout(self, status):
        self.SetTitle(self._default_title)
        self.progress_label.SetLabelText(status)

        self.cancel_button.Hide()
        self.ok_button.Show()

        self.main_sizer.Layout()
        self.main_sizer.SetSizeHints(self)

    # private functions
    #======================================================================
    def _update_layout(self):
        self.progress_sizer.SetSizeHints(self.progress_panel)
        self.main_sizer.Layout()
        self.main_sizer.SetSizeHints(self)


_g_logger = logging.getLogger(__name__)


if __name__ == '__main__':
    app = wx.App()
    MainForm(None,
             "SaveMedia-dev",
             "savemedia.ico",
             conversions=[]).Show()
    app.MainLoop()
