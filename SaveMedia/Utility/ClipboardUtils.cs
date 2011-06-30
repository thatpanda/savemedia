using System;

namespace Utility
{
    static class ClipboardUtils
    {
        public static String ReadUrl()
        {
            String theClipboardString = ReadText();
            String theUrl = String.Empty;

            if( !String.IsNullOrEmpty( theClipboardString ) )
            {
                Uri theUri;
                bool isValidUrl = Uri.TryCreate( theClipboardString, UriKind.Absolute, out theUri );

                if( isValidUrl )
                {
                    theUrl = theUri.OriginalString;
                }
            }

            return theUrl;
        }

        public static String ReadText()
        {
            System.Windows.Forms.IDataObject theData = System.Windows.Forms.Clipboard.GetDataObject();

            if( theData.GetDataPresent( System.Windows.Forms.DataFormats.Text ) )
            {
                return (String)theData.GetData( System.Windows.Forms.DataFormats.Text );
            }
            return String.Empty;
        }
    }
}
