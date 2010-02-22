﻿using System;
using System.Runtime.InteropServices;

namespace Utility
{
    static class ImageUtils
    {
        [StructLayout( LayoutKind.Sequential )]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;            // out: icon
            public IntPtr iIcon;            // out: icon index
            public uint dwAttributes;       // out: SFGAO_ flags
            [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
            public string szDisplayName;    // out: display name (or path)
            [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
            public string szTypeName;       // out: type name
        };

        // dwFileAttributes
        public const uint FILE_ATTRIBUTE_NORMAL = 0x80;

        // uFlags
        public const uint SHGFI_ICON = 0x100;               // get icon
        public const uint SHGFI_LARGEICON = 0x0;            // get large icon
        public const uint SHGFI_SMALLICON = 0x1;            // get small icon
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;   // use passed dwFileAttribute

        [DllImport( "shell32.dll" )]
        public static extern IntPtr SHGetFileInfo( string pszPath,
                                                   uint dwFileAttributes,
                                                   ref SHFILEINFO psfi,
                                                   uint cbSizeFileInfo,
                                                   uint uFlags );

        public static System.Drawing.Icon AssociatedIcon( String aFilename )
        {
            IntPtr hImg;
            SHFILEINFO theFileInfo = new SHFILEINFO();

            hImg = SHGetFileInfo(
                aFilename,
                FILE_ATTRIBUTE_NORMAL,
                ref theFileInfo,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf( theFileInfo ),
                SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES );

            System.Drawing.Icon theIcon = System.Drawing.Icon.FromHandle( theFileInfo.hIcon );
            return theIcon;
        }
    }
}