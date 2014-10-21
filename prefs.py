import codecs
import configparser
import os.path


def _default_config():
    _default = dict()
    _default["download_dir"] = os.path.expanduser("~")

    _config = configparser.ConfigParser(_default)
    _config.add_section("general")

    return _config


class Prefs(object):
    def __init__(self):
        self._config = _default_config()
        self._config_filename = "config.ini"

        if os.path.isfile(self._config_filename):
            self._config.read_file(codecs.open(self._config_filename, "r", "utf8"))

    def save(self):
        with codecs.open(self._config_filename,
                         encoding="utf8",
                         mode = "wb") as _config_file:
            self._config.write(_config_file)

    @property
    def download_dir(self):
        return self._config.get("general", "download_dir")

    @download_dir.setter
    def download_dir(self, value):
        self._config.set("general", "download_dir", value)


if __name__ == "__main__":
    _prefs = Prefs()
    print(_prefs.download_dir)
    _prefs.download_dir = "test"
    print(_prefs.download_dir)
    _prefs.save()