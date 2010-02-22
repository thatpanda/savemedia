using System;

namespace Utility
{
    static class NetUtils
    {
        private static System.Net.WebClient                         mSourceCodeClient;
        private static System.Net.DownloadStringCompletedEventArgs  mSourceCodeEvent;

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
                aResult = mSourceCodeClient.DownloadString( aUrl );
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

            //if( mSourceCodeEvent != null )
            //{
            //    if( mSourceCodeEvent.Error != null )
            //    {
            //        aResult = mSourceCodeEvent.Error.Message;
            //    }
            //    else
            //    {
            //        aResult = mSourceCodeEvent.Result;
            //        aResult = System.Web.HttpUtility.HtmlDecode( aResult );
            //        isSuccess = true;
            //    }
            //}

            System.Windows.Forms.Application.UseWaitCursor = false;

            return isSuccess;
        }

        private static void DownloadSourceCodeCompleted( object sender, System.Net.DownloadStringCompletedEventArgs e )
        {
            mSourceCodeEvent = e;
        }
    }
}
