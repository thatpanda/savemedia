from distutils.core import setup
import py2exe

import metadata


# http://www.py2exe.org/index.cgi/win32com.shell
# ModuleFinder can't handle runtime changes to __path__, but win32com uses them
try:
    # py2exe 0.6.4 introduced a replacement modulefinder.
    # This means we have to add package paths there, not to the built-in
    # one.  If this new modulefinder gets integrated into Python, then
    # we might be able to revert this some day.
    # if this doesn't work, try import modulefinder
    try:
        import py2exe.mf as modulefinder
    except ImportError:
        import modulefinder
    import win32com, sys
    for p in win32com.__path__[1:]:
        modulefinder.AddPackagePath("win32com", p)
    for extra in ["win32com.shell"]: #,"win32com.mapi"
        __import__(extra)
        m = sys.modules[extra]
        for p in m.__path__[1:]:
            modulefinder.AddPackagePath(extra, p)
except ImportError:
    # no build path setup, no worries.
    pass

data_files = [("", ["youtube-dl.exe"])]
setup(
    name=metadata.name,
    version=metadata.version,
    description=metadata.description,
    author=metadata.author,
    console=[
        {"script": "savemedia.py",
         "icon_resources": [(0, "savemedia.ico")]
        }
    ],
    windows=[
        {"script": "savemedia.py",
         "icon_resources": [(0, metadata.icon)]
        }
    ],
    data_files=data_files,
    options={
        "py2exe":{
            "bundle_files": 1,
            "dll_excludes": ["MSVCP90.dll", "HID.DLL", "w9xpopen.exe",
                             "mswsock.dll", "powrprof.dll"],
            },
        },
    zipfile=None
    )
