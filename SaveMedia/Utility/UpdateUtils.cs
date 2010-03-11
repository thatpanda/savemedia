using System;
using System.Windows.Forms;

namespace Utility
{
    static class UpdateUtils
    {
        private const String gcUpdatesXmlUrl = "http://savemedia.googlecode.com/files/Updates.xml";
        private static String mUpdatesXmlPath;
        private static bool mAlreadyCheckedForUpdates;

        private static System.Collections.Generic.Dictionary< String, String > mSettings;

        private static System.Net.WebClient mWebClient;

        private static SaveMedia.MainForm mMainForm;

        public static void StartupCheckIfNeeded( SaveMedia.MainForm aForm )
        {
            if( mAlreadyCheckedForUpdates )
            {
                return;
            }

            if( DateTime.Today == SaveMedia.Settings.LastTimeCheckedForUpdates() )
            {
                mAlreadyCheckedForUpdates = true;
                return;
            }

            if( SaveMedia.Settings.CheckForUpdates().Equals( "auto", StringComparison.Ordinal ) )
            {
                CheckForUpdates( aForm );
            } 
        }

        public static void CheckForUpdatesIfNeeded( SaveMedia.MainForm aForm )
        {
            if( mAlreadyCheckedForUpdates )
            {
                return;
            }

            if( DateTime.Today == SaveMedia.Settings.LastTimeCheckedForUpdates() )
            {
                mAlreadyCheckedForUpdates = true;
                return;
            }

            switch( SaveMedia.Settings.CheckForUpdates() )
            {
                case "auto":
                    CheckForUpdates( aForm );
                    break;
                case "when fails":
                    CheckForUpdates( aForm );
                    break;
                case "never":
                    return;
                default:
                    break;
            }
        }

        public static void CheckForUpdates( SaveMedia.MainForm aForm )
        {
            if( mAlreadyCheckedForUpdates )
            {
                return;
            }

            mMainForm = aForm;

            mAlreadyCheckedForUpdates = true;
            SaveMedia.Settings.LastTimeCheckedForUpdates( DateTime.Now );

            if( mWebClient == null )
            {
                mWebClient = new System.Net.WebClient();
                //mWebClient.CachePolicy = new System.Net.Cache.RequestCachePolicy( System.Net.Cache.RequestCacheLevel.Revalidate );
                mWebClient.Encoding = System.Text.Encoding.UTF8;
                mWebClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                mWebClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler( DownloadUpdateCompleted );
            }

            while( mWebClient.IsBusy )
            {
                Application.DoEvents();
            }

            try
            {
                Uri theUpdatesXmlUrl = new Uri( gcUpdatesXmlUrl );
                mUpdatesXmlPath = System.IO.Path.GetTempFileName();

                mWebClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
                mWebClient.DownloadFileAsync( theUpdatesXmlUrl, mUpdatesXmlPath );
            }
            catch( System.Net.WebException )
            {
            }
            catch( System.NotSupportedException )
            {
            }
        }

        private static void DownloadUpdateCompleted( object sender, System.ComponentModel.AsyncCompletedEventArgs e )
        {
            if( e.Error == null && System.IO.File.Exists( mUpdatesXmlPath ) )
            {
                Load();
            }

            FileUtils.DeleteFile( mUpdatesXmlPath );

            if( mSettings == null || !mSettings.ContainsKey( "Version" ) )
            {
                return;
            }

            String theLatestVersion = mSettings[ "Version" ];
            String theCurrentVersion = SaveMedia.Program.TitleVersion;

            if( theLatestVersion.Equals( theCurrentVersion, StringComparison.Ordinal ) )
            {
                return;
            }

            if( mMainForm.PromptForUpdate() != DialogResult.Yes )
            {
                return;
            }

            System.Diagnostics.Process.Start( "http://savemedia.googlecode.com/" );
        }

        private static void Load()
        {
            if( mSettings == null )
            {
                mSettings = new System.Collections.Generic.Dictionary<String, String>();
            }

            mSettings = FileUtils.ReadXml( mUpdatesXmlPath );
        }
    }
}
