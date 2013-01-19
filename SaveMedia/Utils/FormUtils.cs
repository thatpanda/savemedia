// Ref:
// http://pietschsoft.com/post/2009/01/CSharp-Flash-Window-in-Taskbar-via-Win32-FlashWindowEx.aspx
// http://msdn.microsoft.com/en-us/library/ms182161.aspx

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Utility
{
    [CLSCompliantAttribute( false )]

    public static class FormUtils
    {
        [StructLayout( LayoutKind.Sequential )]
        internal struct FlashInfo
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

        [SuppressUnmanagedCodeSecurityAttribute]
        internal static class UnsafeNativeMethods
        {
            [DllImport( "user32.dll" )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool FlashWindowEx( ref FlashInfo pwfi );

            [DllImport( "user32.dll", CharSet = CharSet.Auto )]
            internal static extern Int32 SendMessage( IntPtr hWnd, int msg, int wParam, [MarshalAs( UnmanagedType.LPWStr )] String lParam );
        }

        private const int EM_SETCUEBANNER = 0x1501;

        // Stop flashing. The system restores the window to its original state.
        public const uint StopFlashing = 0;

        // Flash the window caption.
        public const uint FlashCaption = 1;

        // Flash the taskbar button.
        public const uint FlashTaskbar = 2;

        // Flash both the window caption and taskbar button.
        // This is equivalent to setting the FlashCaption | FlashTaskbar flags.
        public const uint FlashAll = 3;

        // Flash continuously, until the StopFlashing flag is set.
        public const uint FlashUntilStopIsCalled = 4;

        // Flash continuously until the window comes to the foreground.
        public const uint FlashUntilWindowIsActive = 12;

        private static FlashInfo CreateFlashInfo( IntPtr handle, uint flags, uint count, uint timeout )
        {
            FlashInfo fi = new FlashInfo();
            fi.cbSize = Convert.ToUInt32( Marshal.SizeOf( fi ) );
            fi.hwnd = handle;
            fi.dwFlags = flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }

        // Flash the given Window until it receives focus.
        public static bool FlashWindow( System.Windows.Forms.Form aForm )
        {
            if( IsWin2kOrLater )
            {
                new UIPermission( UIPermissionWindow.AllWindows ).Demand();
                FlashInfo fi = CreateFlashInfo( aForm.Handle, FlashAll | FlashUntilWindowIsActive, uint.MaxValue, 0 );
                return UnsafeNativeMethods.FlashWindowEx( ref fi );
            }
            return false;
        }        

        // Flash the given Window for the specified number of times
        public static bool Flash( System.Windows.Forms.Form aForm, uint aCount )
        {
            if( IsWin2kOrLater )
            {
                new UIPermission( UIPermissionWindow.AllWindows ).Demand();
                FlashInfo fi = CreateFlashInfo( aForm.Handle, FlashAll, aCount, 0 );
                return UnsafeNativeMethods.FlashWindowEx( ref fi );
            }
            return false;
        }

        // Start Flashing the given Window
        public static bool Start( System.Windows.Forms.Form aForm )
        {
            if( IsWin2kOrLater )
            {
                new UIPermission( UIPermissionWindow.AllWindows ).Demand();
                FlashInfo fi = CreateFlashInfo( aForm.Handle, FlashAll, uint.MaxValue, 0 );
                return UnsafeNativeMethods.FlashWindowEx( ref fi );
            }
            return false;
        }

        // Stop Flashing the given Window
        public static bool Stop( System.Windows.Forms.Form aForm )
        {
            if( IsWin2kOrLater )
            {
                new UIPermission( UIPermissionWindow.AllWindows ).Demand();
                FlashInfo fi = CreateFlashInfo( aForm.Handle, StopFlashing, uint.MaxValue, 0 );
                return UnsafeNativeMethods.FlashWindowEx( ref fi );
            }
            return false;
        }

        private static bool IsWin2kOrLater
        {
            get { return System.Environment.OSVersion.Version.Major >= 5; }
        }

        public static void SetCueText( System.Windows.Forms.TextBox aControl, String aText )
        {
            UnsafeNativeMethods.SendMessage( aControl.Handle, EM_SETCUEBANNER, 0, aText );
        }
    } 
}
