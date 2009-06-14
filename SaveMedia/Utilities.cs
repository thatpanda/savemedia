using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SaveMedia
{
    static class Utilities
    {
        private static System.Net.WebClient                         mSourceCodeClient;
        private static System.Net.DownloadStringCompletedEventArgs  mSourceCodeEvent;

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
            Utilities.SHFILEINFO theFileInfo = new Utilities.SHFILEINFO();

            hImg = Utilities.SHGetFileInfo(
                aFilename,
                Utilities.FILE_ATTRIBUTE_NORMAL,
                ref theFileInfo,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf( theFileInfo ),
                Utilities.SHGFI_ICON | Utilities.SHGFI_LARGEICON | Utilities.SHGFI_USEFILEATTRIBUTES );

            System.Drawing.Icon theIcon = System.Drawing.Icon.FromHandle( theFileInfo.hIcon );
            return theIcon;
        }

        public static String YouTubeAvailableQuality( String aFmtMap, String aPreferedQuality )
        {
            if( String.IsNullOrEmpty( aFmtMap ) ||
                String.IsNullOrEmpty( aPreferedQuality ) )
            {
                return String.Empty;
            }

            // "fmt_map": "35/640000/9/0/115,18/512000/9/0/115,34/0/9/0/115,5/0/7/0/0"
            aFmtMap = "," + aFmtMap;

            int thePreferedQuality = Convert.ToInt32( aPreferedQuality );

            if( aFmtMap.IndexOf( "," + aPreferedQuality + "/" ) != -1 )
            {
                return "&fmt=" + aPreferedQuality;
            }
            else if( thePreferedQuality > 22 && aFmtMap.IndexOf( ",22/" ) != -1 )
            {
                return "&fmt=22";
            }
            else if( thePreferedQuality > 18 && aFmtMap.IndexOf( ",18/" ) != -1 )
            {
                return "&fmt=18";
            }
            else if( thePreferedQuality > 6 && aFmtMap.IndexOf( ",6/" ) != -1 )
            {
                return "&fmt=6";
            }

            return String.Empty;
        }

        public static bool DownloadString( Uri aUrl, out String aResult )
        {
            System.Windows.Forms.Application.UseWaitCursor = true;

            bool isSuccess = false;
            aResult = String.Empty;
            mSourceCodeEvent = null;

            if( mSourceCodeClient == null )
            {
                mSourceCodeClient = new System.Net.WebClient();
                //mSourceCodeClient.CachePolicy = new System.Net.Cache.RequestCachePolicy( System.Net.Cache.RequestCacheLevel.Revalidate );
                mSourceCodeClient.Encoding = System.Text.Encoding.UTF8;                
                mSourceCodeClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                mSourceCodeClient.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler( DownloadSourceCodeCompleted );
            }            

            while( mSourceCodeClient.IsBusy )
            {
                System.Windows.Forms.Application.DoEvents();
            }

            try
            {
                mSourceCodeClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
                mSourceCodeClient.DownloadStringAsync( aUrl );

                while( mSourceCodeEvent == null )
                {
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            catch( System.Net.WebException e )
            {
                mSourceCodeEvent = null;
                aResult = e.Message;
            }

            if( mSourceCodeEvent != null )
            {
                if( mSourceCodeEvent.Error != null )
                {
                    aResult = mSourceCodeEvent.Error.Message;
                }
                else
                {
                    aResult = mSourceCodeEvent.Result;
                    isSuccess = true;
                }
            }

            System.Windows.Forms.Application.UseWaitCursor = false;

            return isSuccess;
        }

        private static void DownloadSourceCodeCompleted( object sender, System.Net.DownloadStringCompletedEventArgs e )
        {
            mSourceCodeEvent = e;
        }

        public static bool StringBetween( String aSource, String aStart, String aEnd, out String aResult )
        {
            aResult = String.Empty;

            if( String.IsNullOrEmpty( aSource ) )
            {
                return false;
            }

            int theStartIndex = aSource.IndexOf( aStart );            
            if( theStartIndex == -1 )
            {
                return false;                
            }
            theStartIndex = theStartIndex + aStart.Length;

            int theEndIndex = aSource.IndexOf( aEnd, theStartIndex );
            if( theEndIndex == -1 )
            {
                return false;
            }

            int theStringLength = theEndIndex - theStartIndex;
            aResult = aSource.Substring( theStartIndex, theStringLength );

            return true;
        }

        public static String FilenameCheck( String aFilename )
        {
            aFilename = aFilename.Replace( "\\", "" );
            aFilename = aFilename.Replace( "/", "" );
            aFilename = aFilename.Replace( ":", "" );
            aFilename = aFilename.Replace( "*", "" );
            aFilename = aFilename.Replace( "?", "" );
            aFilename = aFilename.Replace( "\"", "'" );
            aFilename = aFilename.Replace( "<", "" );
            aFilename = aFilename.Replace( ">", "" );
            aFilename = aFilename.Replace( "|", "" );

            if( String.IsNullOrEmpty( aFilename ) )
            {
                return "default";
            }

            return aFilename;
        }

        public static String SaveFile( String aFilename, String aFilter )
        {
            aFilename = FilenameCheck( aFilename );

            SaveFileDialog theSaveFileDialog = new SaveFileDialog();
            theSaveFileDialog.AddExtension = true;
            theSaveFileDialog.CheckFileExists = false;
            theSaveFileDialog.CheckPathExists = true;
            theSaveFileDialog.CreatePrompt = false;
            theSaveFileDialog.FileName = aFilename;
            theSaveFileDialog.Filter = aFilter;
            theSaveFileDialog.FilterIndex = 1;
            theSaveFileDialog.OverwritePrompt = true;
            theSaveFileDialog.RestoreDirectory = true;
            theSaveFileDialog.ValidateNames = true;

            if( theSaveFileDialog.ShowDialog() == DialogResult.OK )
            {
                return theSaveFileDialog.FileName;
            }

            return String.Empty;
        }

        public static
        System.Collections.Generic.Dictionary<String, String>
        ReadXml( String aFilePath )
        {
            System.Collections.Generic.Dictionary< String, String > theNodes = new System.Collections.Generic.Dictionary<String, String>();;

            if( System.IO.File.Exists( aFilePath ) )
            {
                System.Xml.XmlTextReader theReader = null;

                try
                {
                    theReader = new System.Xml.XmlTextReader( aFilePath );
                    String theElement = String.Empty;

                    while( theReader.Read() )
                    {
                        switch( theReader.NodeType )
                        {
                            case System.Xml.XmlNodeType.Element:
                                theElement = theReader.Name;
                                break;
                            case System.Xml.XmlNodeType.Text:
                                theNodes.Add( theElement, theReader.Value );
                                break;
                            case System.Xml.XmlNodeType.EndElement:
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if( theReader != null )
                    {
                        theReader.Close();
                    }
                }
            }

            return theNodes;
        }

        public static void WriteXml( String aFilePath, ref System.Collections.Generic.Dictionary<String, String> aNodes )
        {
            System.Xml.XmlTextWriter theWriter = null;

            try
            {
                theWriter = new System.Xml.XmlTextWriter( aFilePath, System.Text.Encoding.UTF8 );

                foreach( System.Collections.Generic.KeyValuePair< String, String > thePair in aNodes )
                {
                    theWriter.WriteStartElement( thePair.Key );
                    theWriter.WriteString( thePair.Value );
                    theWriter.WriteEndElement();
                }
            }
            catch
            {
            }
            finally
            {
                if( theWriter != null )
                {
                    theWriter.Close();
                }
            }
        }

        public static String ReadClipboardUrl()
        {
            String theClipboardString = ReadClipboardText();
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

        public static String ReadClipboardText()
        {
            System.Windows.Forms.IDataObject theData = System.Windows.Forms.Clipboard.GetDataObject();

            if( theData.GetDataPresent( System.Windows.Forms.DataFormats.Text ) )
            {
                return (String) theData.GetData( System.Windows.Forms.DataFormats.Text );
            }
            return String.Empty;
        }
    }
}
