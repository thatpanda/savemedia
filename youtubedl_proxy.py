# -*- coding: utf-8 -*-
import HTMLParser
import json
import locale
import logging
import os.path
import subprocess

from datatag import DownloadTag


def _run(*args):
    exe_path = u"youtube-dl.exe"
    cmd = [
        exe_path,
        u"--newline",
    ]
    cmd.extend(args)
    si = subprocess.STARTUPINFO()
    si.dwFlags = subprocess.STARTF_USESHOWWINDOW
    si.wShowWindow = subprocess.SW_HIDE
    return subprocess.Popen(
        cmd,
        stdin=subprocess.PIPE,
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        universal_newlines=True,
        startupinfo=si,
    )


def _run_wait(*args):
    exe_path = u"youtube-dl.exe"
    cmd = [
        exe_path,
        u"--newline",
    ]
    cmd.extend(args)
    si = subprocess.STARTUPINFO()
    si.dwFlags = subprocess.STARTF_USESHOWWINDOW
    si.wShowWindow = subprocess.SW_HIDE
    p = subprocess.Popen(
        cmd,
        stdin=subprocess.PIPE,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        universal_newlines=True,
        startupinfo=si,
    )
    stdout, stderr = p.communicate()
    return p.returncode, stdout, stderr


class YoutubeDlProxyError(Exception):
    def __init__(self, message=None):
        if not message:
            self.message = u"Error: unknown error"
        elif ";" in message:
            self.message = message[:message.index(";")]

    def __str__(self):
        return repr(self.message)


class YoutubeDlProxy:
    def __init__(self, url):
        self.download_tag = DownloadTag(url)

        self._initialize()

    # private functions
    #======================================================================
    @staticmethod
    def _validate(message):
        if not message or message.lower().startswith("error"):
            raise YoutubeDlProxyError(message)

    def _download_with_progress(self, url, show_progress=True):
        h = HTMLParser.HTMLParser()
        p = _run(url)
        while p.poll() is None:
            if p.stdout:
                message = p.stdout.readline().rstrip(u"\n")
                if show_progress and message:
                    print(h.unescape(message))

    def _get_filename(self, url):
        template = u"%(title)s.%(ext)s"
        returncode, stdout, stderr = _run_wait(u"--output", template,
                                               u"--get-filename", url)
        self._validate(stdout)
        h = HTMLParser.HTMLParser()
        return h.unescape(stdout.rstrip("\n"))
    
    def _get_thumbnail_url(self, url):
        returncode, stdout, stderr = _run_wait(u"--get-thumbnail", url)
        self._validate(stdout)
        return stdout.rstrip("\n")

    def _get_video_url(self, url):
        returncode, stdout, stderr = _run_wait(u"--get-url", url)
        self._validate(stdout)
        return stdout.rstrip("\n")

    def _initialize(self):
        returncode, stdout, stderr = _run_wait(
            u"--skip-download",
            u"--output", u"%(title)s.%(ext)s",
            u"--get-filename",
            u"--get-thumbnail",
            u"--get-url",
            u"--no-playlist",
            self.download_tag.source_url)

        system_encoding = locale.getpreferredencoding()
        try:
            stdout = stdout.decode(system_encoding)
            stderr = stderr.decode(system_encoding)
        except UnicodeEncodeError:
            pass

        _g_logger.debug(u"code: {0}".format(returncode))
        _g_logger.debug(u"stdout: {0}".format(stdout))
        if stderr:
            _g_logger.debug(u"stderr: {0}".format(stderr))

        if returncode != 0:
            return

        content = stdout.splitlines()
        if len(content) != 3:
            return

        self.download_tag.filename = content[2]
        self.download_tag.thumbnail_url = content[1]
        self.download_tag.video_url = content[0]

        _g_logger.info(u"source: {0}".format(self.download_tag.source_url))
        _g_logger.info(
            u"default filename: {0}".format(self.download_tag.filename))
        _g_logger.info(
            u"thumbnail: {0}".format(self.download_tag.thumbnail_url))
        _g_logger.info(
            u"download url: {0}".format(self.download_tag.video_url))

    def _initialize_with_json(self):
        returncode, stdout, stderr = _run_wait(
            u"--write-info-json",
            u"--skip-download",
            u"--output", u"savemedia",
            #u"--output", u"savemedia-%(playlist_index)s",
            u"--no-playlist",
            self.download_tag.source_url)

        _g_logger.debug(u"code: {0}".format(returncode))
        _g_logger.debug(u"stdout: {0}".format(stdout))
        _g_logger.debug(u"stderr: {0}".format(stderr))

        if returncode != 0:
            return

        filename = u"savemedia.info.json"
        if not os.path.isfile(filename):
            return

        metadata = json.load(open(filename))
        self.download_tag.filename = metadata.get(u"fulltitle", None)
        self.download_tag.thumbnail_url = metadata.get(u"thumbnail", None)
        self.download_tag.video_url = metadata.get(u"url", None)


_g_logger = logging.getLogger(__name__)


if __name__ == "__main__":
    console_handler = logging.StreamHandler()
    console_handler.setLevel(logging.INFO)
    formatter = logging.Formatter(u"%(name)-12s [%(levelname)s] %(message)s")
    console_handler.setFormatter(formatter)

    logger = logging.getLogger()
    logger.setLevel(logging.DEBUG)
    logger.addHandler(console_handler)

    # url = "http://www.youtube.com/watch?v=..."
    # p = YoutubeDlProxy(url)
    # print(p.download_tag.filename)
    # print(p.download_tag.thumbnail_url)
    # print(p.download_tag.video_url)
