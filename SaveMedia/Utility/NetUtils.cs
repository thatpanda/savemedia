using System;

namespace Utility
{
    static class NetUtils
    {
        private static System.Net.WebClient mWebClient;

        public static bool DownloadString( Uri aUrl, out String aResult )
        {
            System.Windows.Forms.Application.UseWaitCursor = true;

            bool isSuccess = false;
            aResult = String.Empty;

            if( mWebClient == null )
            {
                mWebClient = new System.Net.WebClient();
                //mWebClient.CachePolicy = new System.Net.Cache.RequestCachePolicy( System.Net.Cache.RequestCacheLevel.Revalidate );
                mWebClient.Encoding = System.Text.Encoding.UTF8;
                mWebClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }

            while( mWebClient.IsBusy )
            {
                System.Windows.Forms.Application.DoEvents();
            }

            try
            {
                mWebClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
                aResult = mWebClient.DownloadString( aUrl );
                isSuccess = true;
            }
            catch( System.Net.WebException e )
            {
                aResult = e.Message;
            }
            catch( System.NotSupportedException e )
            {
                aResult = e.Message;
            }

            System.Windows.Forms.Application.UseWaitCursor = false;

            return isSuccess;
        }
    }
}
