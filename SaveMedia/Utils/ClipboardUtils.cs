using System;

namespace Utility
{
    static class ClipboardUtils
    {
        public static String ReadUrl()
        {
            if( System.Windows.Forms.Clipboard.ContainsText() )
            {
                Uri theUri;
                if( Uri.TryCreate( System.Windows.Forms.Clipboard.GetText(), UriKind.Absolute, out theUri ) )
                {
                    return theUri.OriginalString;
                }
            }

            return String.Empty;
        }
    }
}
