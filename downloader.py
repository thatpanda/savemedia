#!/usr/bin/python

import logging
from time import time
import threading
import urllib.error, urllib.parse, urllib.request

import wx


class DownloadCompleteEvent:
    def __init__(self, url, destination=None, message=None, cancelled=False,
                 error=False):
        self.cancelled = cancelled
        self.destination = destination
        self.error = error
        self.message = message
        self.url = url


class DownloadProgressEvent:
    def __init__(self, percentage, message=None):
        self.message = message
        self.percentage = percentage


class Downloader:
    def __init__(self):
        # event callbacks
        self.complete_callback = None
        self.progress_callback = None

        self._buffer_size = 8192  # same value as in urllib.urlretrieve
        self._bytes_per_sec = 0
        self._cancelled = False
        self._download_speed_timer = None
        self._download_thread = None
        self._downloaded_bytes = 0.0
        self._downloaded_MB = 0.0
        self._KB_per_sec = 0.0
        self._percentage = 0
        self._previous_downloaded_bytes = None
        self._progress_timer = None
        self._remaining_time = None
        self._total_bytes = 0
        self._total_MB = 0.0

    def cancel(self):
        self._cancelled = True
        
    def download(self, url, destination):
        self._cancelled = False
        self._download_thread = threading.Thread(
            target=self._download,
            args=(
                url,
                destination,
            ),
        )
        self._download_thread.start()

    # timer events
    #======================================================================
    def _download_speed_timer_tick(self):
        self._bytes_per_sec = (self._downloaded_bytes -
                               self._previous_downloaded_bytes)
        self._previous_downloaded_bytes = self._downloaded_bytes

        self._KB_per_sec = self._bytes_per_sec / 1024
        # _g_logger.debug("{0} KB/sec".format(self._KB_per_sec))

        if self._bytes_per_sec > 0:
            remaining_bytes = self._total_bytes - self._downloaded_bytes
            remaining_sec = round(remaining_bytes/self._bytes_per_sec)
            remaining_minute = round(remaining_sec/60)

            if remaining_minute > 1:
                self._remaining_time = "{0:.0f} minutes remaining".format(
                    remaining_minute)
            elif remaining_minute > 0:
                self._remaining_time = "{0:.0f} minute remaining".format(
                    remaining_minute)
            else:
                self._remaining_time = "{0:.0f} seconds remaining".format(
                    remaining_sec)

        if self._download_thread.isAlive():
            self._start_download_speed_timer()

    def _progress_timer_tick(self):
        self._report_progress()
        
        if self._download_thread.isAlive():
            self._start_progress_timer()

    # private functions
    #======================================================================
    def _download(self, url, destination):
        start_time = time()

        try:
            response = urllib.request.urlopen(url)
        except urllib.error.HTTPError as e:
            _g_logger.exception("Failed to access {0}".format(url))
            message = "Error: {0} - {1}".format(e.code, e.reason)
            event = DownloadCompleteEvent(url, error=True, message=message)
            wx.CallAfter(self.complete_callback, event)
            return
        except urllib.error.URLError as e:
            _g_logger.exception("Failed to access {0}".format(url))
            message = "Error: {0}".format(e.reason)
            event = DownloadCompleteEvent(url, error=True, message=message)
            wx.CallAfter(self.complete_callback, event)
            return
        else:
            metadata = response.info()

        self._total_bytes = float(metadata.get("Content-Length"))
        self._total_MB = self._total_bytes / 1024 / 1024

        if self.progress_callback:
            self._downloaded_bytes = 0.0
            self._downloaded_MB = 0.0
            self._KB_per_sec = 0.0
            self._previous_downloaded_bytes = 0.0
            self._remaining_time = "? minutes remaining"

            self._start_progress_timer()
            self._start_download_speed_timer()

        fp = open(destination, "wb")
        while not self._cancelled:
            data = response.read(self._buffer_size)
            if not data:
                break
            fp.write(data)
            if self.progress_callback:
                self._downloaded_bytes += len(data)
                self._downloaded_MB = self._downloaded_bytes / 1024 / 1024
                self._percentage = int(self._downloaded_bytes /
                                       self._total_bytes * 100)
        response.close()
        fp.close()

        if self._download_speed_timer:
            self._download_speed_timer.cancel()
            self._download_speed_timer = None
        if self._progress_timer:
            if not self._cancelled:
                self._report_progress()
            self._progress_timer.cancel()
            self._progress_timer = None

        end_time = time()
        elapsed_time = end_time - start_time
        _g_logger.info("elapsed time: {0}".format(elapsed_time))

        if self._cancelled:
            message = "download cancelled"
        else:
            message = "{0:.2f} MB - download completed".format(self._total_MB)

        _g_logger.info(message)

        if self.complete_callback:
            event = DownloadCompleteEvent(
                url, destination, message, self._cancelled)
            wx.CallAfter(self.complete_callback, event)

    def _report_progress(self):
        # e.g. 1 minute remaining - 2.21 of 7.93 MB (64 KB/sec)
        message = "{0} - {1:.2f} of {2:.2f} MB ({3:.0f} KB/sec)".format(
            self._remaining_time,
            self._downloaded_MB,
            self._total_MB,
            self._KB_per_sec
        )

        _g_logger.debug(message)

        event = DownloadProgressEvent(self._percentage, message)
        wx.CallAfter(self.progress_callback, event)

    def _start_download_speed_timer(self):
        self._download_speed_timer = threading.Timer(
                1.0, self._download_speed_timer_tick)
        self._download_speed_timer.start()

    def _start_progress_timer(self):
        self._progress_timer = threading.Timer(
                0.25, self._progress_timer_tick)
        self._progress_timer.start()


_g_logger = logging.getLogger(__name__)


if __name__ == '__main__':
    pass
