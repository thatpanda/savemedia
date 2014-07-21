import logging
import os
import re
import shutil
import subprocess
import sys
from tempfile import mkstemp
import threading

import wx

from datatag import ConversionTag


def _change_file_ext(path, ext):
    return os.path.splitext(path)[0] + ext


def _get_temp_file_path():
    file_handle, file_path = mkstemp()
    os.close(file_handle)
    return file_path


def _has_unsupported_character(path):
    encoded_path = path.encode(sys.getfilesystemencoding())
    return path != encoded_path


def _run_ffmpeg(*args):
    cmd = [u"ffmpeg.exe"]
    cmd.extend(args)
    si = subprocess.STARTUPINFO()
    si.dwFlags = subprocess.STARTF_USESHOWWINDOW
    si.wShowWindow = subprocess.SW_HIDE
    return subprocess.Popen(
        cmd,
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        shell=False,
        universal_newlines=True,
        startupinfo=si
    )


class ConvertCompleteEvent:
    def __init__(self, destination=None, message=None, cancelled=False,
                 error=False):
        self.cancelled = cancelled
        self.destination = destination
        self.error = error
        self.message = message


class ConvertProgressEvent:
    def __init__(self, percentage, message=None):
        self.message = message
        self.percentage = percentage


class Converter:
    def __init__(self):
        # event callbacks
        self.complete_callback = None
        self.progress_callback = None

        # private members
        self._cancelled = False
        self._convert_thread = None
        self._destination = None
        self._ffmpeg = None
        self._source = None
        self._temp_destination = None
        self._temp_source = None

    def cancel(self):
        self._cancelled = True
        if self._ffmpeg:
            self._ffmpeg.kill()

    def convert_file(self, source, conversion):
        self._cancelled = False
        self._source = source
        self._destination = _change_file_ext(source, conversion.ext)

        self._temp_source = _get_temp_file_path()

        # If dst already exists, it will be replaced
        shutil.copyfile(source, self._temp_source)

        self._temp_destination = _get_temp_file_path()
        self._temp_destination = _change_file_ext(self._temp_destination,
                                                  conversion.ext)

        _g_logger.info(self._temp_source)
        _g_logger.info(self._temp_destination)

        # Replace {0} and {1} after split() in case they contain spaces
        ffmpeg_args = conversion.ffmpeg_args.split(" ")
        for index, arg in enumerate(ffmpeg_args):
            if arg == "{0}":
                ffmpeg_args[index] = self._temp_source
            elif arg == "{1}":
                ffmpeg_args[index] = self._temp_destination

        self._ffmpeg = _run_ffmpeg(*ffmpeg_args)

        self._convert_thread = threading.Thread(
            target=self._parse_ffmpeg_output,
        )
        self._convert_thread.start()

    @staticmethod
    def get_conversions():
        """Return a list of available conversions"""
        conversions = [
            ConversionTag(
                u"Do not convert file",
                display_ext=u".mp4"
            ),
            ConversionTag(
                u"MPEG-1 Audio Layer 3 (*.mp3)",
                u".mp3",
                u"-y -i {0} -ar 44100 -ab 192k -ac 2 {1}"
            ),
            ConversionTag(
                u"Windows Media Video (*.wmv)",
                u".wmv",
                u"-y -i {0} -vcodec wmv2 -sameq -acodec mp2 -ar 44100 -ab 192k -f avi {1}"
            )
        ]
        return conversions

    def _parse_ffmpeg_output(self):
        duration = 0
        message = u"Converting..."
        output = []
        while self._ffmpeg.returncode is None:
            if self.progress_callback and self._ffmpeg.stdout:
                #print(repr(p.stdout.readline().rstrip()))
                line = self._ffmpeg.stdout.readline().rstrip()
                output.append(line)
                if duration == 0:
                    result = re.search(r'duration: (\d{2}):(\d{2}):([\d\.]+),',
                                       line, re.IGNORECASE)
                    if result:
                        hours = float(result.group(1))
                        mins = float(result.group(2))
                        secs = float(result.group(3))
                        duration = hours*3600 + mins*60 + secs
                else:
                    result = re.search(r'time=(\d{2}):(\d{2}):([\d\.]+)',
                                       line, re.IGNORECASE)
                    if result:
                        hours = float(result.group(1))
                        mins = float(result.group(2))
                        secs = float(result.group(3))
                        progress = hours*3600 + mins*60 + secs
                        percentage = min(100, int(progress/duration * 100))
                        _g_logger.info(percentage)

                        event = ConvertProgressEvent(percentage, message)
                        wx.CallAfter(self.progress_callback, event)
            self._ffmpeg.poll()

        if not self._cancelled and self._ffmpeg.returncode != 0:
            # TODO error
            _g_logger.error(output)

        if self.progress_callback and percentage != 100:
            event = ConvertProgressEvent(100, message)
            wx.CallAfter(self.progress_callback, event)

        self._on_convert_complete()

    # thread handlers
    #======================================================================
    def _on_convert_complete(self):
        try:
            os.remove(self._temp_source)
        except WindowsError:
            _g_logger.exception(u"Unable to remove temp_source")

        if os.path.isfile(self._destination):
            os.remove(self._destination)

        if not os.path.isfile(self._temp_destination):
            # TODO: conversion failed
            return

        # TODO: handle IOError, e.g. destination is in-use
        shutil.move(self._temp_destination, self._destination)


        if self._cancelled:
            message = u"conversion cancelled"
        else:
            message = u"conversion completed"

        _g_logger.info(message)

        if self.complete_callback:
            event = ConvertCompleteEvent(
                self._destination, message, self._cancelled)
            wx.CallAfter(self.complete_callback, event)


_g_logger = logging.getLogger(__name__)


if __name__ == "__main__":
    import logging.handlers
    console_handler = logging.StreamHandler()
    console_handler.setLevel(logging.DEBUG)
    formatter = logging.Formatter(u"%(name)-12s [%(levelname)s] %(message)s")
    console_handler.setFormatter(formatter)

    logger = logging.getLogger()
    logger.setLevel(logging.DEBUG)
    logger.addHandler(console_handler)

    # source = "test.flv"
    # convert_file(source, get_conversions()[1])
