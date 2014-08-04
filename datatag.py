class ConversionTag:
    def __init__(self, name, ext=None, ffmpeg_args=None, display_ext=None):
        self.bitmap = None
        self.display_ext = display_ext
        self.ext = ext
        self.ffmpeg_args = ffmpeg_args
        self.name = name

        if self.display_ext is None:
            self.display_ext = self.ext

    def is_valid(self):
        return self.ffmpeg_args is not None


class DownloadTag:
    def __init__(self, url):
        self.error = False
        self.ext = None
        self.message = None
        self.source_url = url
        self.title = None
        self.thumbnail_url = None
        self.video_url = None