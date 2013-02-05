using System;
using System.Windows.Forms;

namespace Utility
{
    static class UpdateUtils
    {
        private const String gcUpdatesXmlUrl = "http://savemedia.googlecode.com/hg/Updates.xml";
        private static String mUpdatesXmlPath;
        private static bool mAlreadyCheckedForUpdates;

        private static System.Collections.Generic.Dictionary< String, String > mSettings;

        private static System.Net.WebClient mWebClient;

        private static SaveMedia.IMainForm mMainForm;

        public static void CheckForUpdatesIfNeeded( SaveMedia.IMainForm aForm )
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

            CheckForUpdates( aForm );
        }

        public static void CheckForUpdates( SaveMedia.IMainForm aForm )
        {
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
            String theCurrentVersion = SaveMedia.Program.FileVersion;

            if( IsNewerVersionAvailable( theCurrentVersion, theLatestVersion ) &&
                mMainForm.PromptForUpdate() == DialogResult.Yes )
            {
                System.Diagnostics.Process.Start( "http://savemedia.googlecode.com/" );
            }
        }

        private static bool IsNewerVersionAvailable( String aCurrentVersion, String aOtherVersion )
        {
            if( aCurrentVersion.Equals( aOtherVersion, StringComparison.Ordinal ) )
            {
                return false;
            }

            String[] theCurrentVersion = aCurrentVersion.Split( '.' );
            String[] theOtherVersion = aOtherVersion.Split( '.' );

            if( theCurrentVersion.Length != theOtherVersion.Length )
            {
                return true;
            }

            for( int theIndex = 0; theIndex < theCurrentVersion.Length; ++theIndex )
            {
                int theCurrent = System.Convert.ToInt32( theCurrentVersion[ theIndex ] );
                int theOther = System.Convert.ToInt32( theOtherVersion[ theIndex ] );

                if( theCurrent > theOther )
                {
                    return false;
                }
                else if( theCurrent < theOther  )
                {
                    return true;
                }
            }

            return false;
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
