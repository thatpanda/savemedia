// Ref:
// http://pietschsoft.com/post/2009/01/CSharp-Flash-Window-in-Taskbar-via-Win32-FlashWindowEx.aspx

using System;
using System.Runtime.InteropServices;

namespace Utility
{
    public static class FormUtils
    {
        [DllImport( "user32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static extern bool FlashWindowEx( ref FLASHWINFO pwfi );

        [StructLayout( LayoutKind.Sequential )]
        private struct FLASHWINFO
        {
            // The size of the structure in bytes.
            public uint cbSize;

            // A Handle to the Window to be Flashed.
            public IntPtr hwnd;

            // The Flash Status.
            public uint dwFlags;

            // The number of times to Flash the window.
            public uint uCount;

            // The rate at which the Window is to be flashed, in milliseconds.
            // If Zero, the function uses the default cursor blink rate.
            public uint dwTimeout;
        }

        // Stop flashing. The system restores the window to its original state.
        public const uint FLASHW_STOP = 0;

        // Flash the window caption.
        public const uint FLASHW_CAPTION = 1;

        // Flash the taskbar button.
        public const uint FLASHW_TRAY = 2;

        // Flash both the window caption and taskbar button.
        // This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
        public const uint FLASHW_ALL = 3;

        // Flash continuously, until the FLASHW_STOP flag is set.
        public const uint FLASHW_TIMER = 4;

        // Flash continuously until the window comes to the foreground.
        public const uint FLASHW_TIMERNOFG = 12;

        private static FLASHWINFO Create_FLASHWINFO( IntPtr handle, uint flags, uint count, uint timeout )
        {
            FLASHWINFO fi = new FLASHWINFO();
            fi.cbSize = Convert.ToUInt32( Marshal.SizeOf( fi ) );
            fi.hwnd = handle;
            fi.dwFlags = flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }

        // Flash the given Window until it receives focus.
        public static bool FlashWindow( System.Windows.Forms.Form form )
        {
            if( IsWin2kOrLater )
            {
                FLASHWINFO fi = Create_FLASHWINFO( form.Handle, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0 );
                return FlashWindowEx( ref fi );
            }
            return false;
        }        

        // Flash the given Window for the specified number of times
        public static bool Flash( System.Windows.Forms.Form form, uint count )
        {
            if( IsWin2kOrLater )
            {
                FLASHWINFO fi = Create_FLASHWINFO( form.Handle, FLASHW_ALL, count, 0 );
                return FlashWindowEx( ref fi );
            }
            return false;
        }

        // Start Flashing the given Window
        public static bool Start( System.Windows.Forms.Form form )
        {
            if( IsWin2kOrLater )
            {
                FLASHWINFO fi = Create_FLASHWINFO( form.Handle, FLASHW_ALL, uint.MaxValue, 0 );
                return FlashWindowEx( ref fi );
            }
            return false;
        }

        // Stop Flashing the given Window
        public static bool Stop( System.Windows.Forms.Form form )
        {
            if( IsWin2kOrLater )
            {
                FLASHWINFO fi = Create_FLASHWINFO( form.Handle, FLASHW_STOP, uint.MaxValue, 0 );
                return FlashWindowEx( ref fi );
            }
            return false;
        }

        private static bool IsWin2kOrLater
        {
            get { return System.Environment.OSVersion.Version.Major >= 5; }
        }
    } 
}
