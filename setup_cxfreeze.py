import sys
from cx_Freeze import setup, Executable

import metadata

# Dependencies are automatically detected, but it might need fine tuning.
build_exe_options = {
    "append_script_to_exe": True,
    "create_shared_zip": False,
    "icon": metadata.icon,
    "include_files": [
        ("ffmpeg.exe", ""),
        ("youtube-dl.exe", ""),
    ],
    "include_msvcr": True,
    "packages": ["os"],
    "excludes": ["tkinter"]
}

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
        "build_exe": build_exe_options
    },
    executables=[
        Executable(
            "savemedia.py",
            base=base,
        )
    ]
)
