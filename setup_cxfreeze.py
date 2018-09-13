# https://cx-freeze.readthedocs.io/en/latest/distutils.html

import sys
from cx_Freeze import setup, Executable

import metadata


# GUI applications require a different base on Windows (the default is for a
# console application).
base = None
if sys.platform == "win32":
    base = "Win32GUI"

setup(
    name=metadata.name,
    version=metadata.version,
    description=metadata.description,
    options={
        "build_exe": {
            "include_files": [
                ("ffmpeg.exe", ""),
            ],
            "include_msvcr": True,
            "packages": ["os"],
            "excludes": ["tkinter"],
            "zip_include_packages": "*",
            "zip_exclude_packages": ""
        }
    },
    executables=[
        Executable(
            "savemedia.py",
            base=base,
            icon=metadata.icon,
        )
    ]
)
