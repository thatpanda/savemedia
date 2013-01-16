using System;
using System.Collections.Generic;
using System.Net;

using Utility;

namespace SaveMedia.Sites
{
    class RapidShare : ISite
    {
        public bool Support( ref Uri aUrl )
        {
            return aUrl.Host.EndsWith( ".rapidshare.com" );
        }

        public void TryParse( ref Uri aUrl,
                              ref List<DownloadTag> aDownloadQueue,
                              ref IMainForm aUI,
                              out String aError )
        {
            aError = String.Empty;

            //DownloadRapidShareFile( ref aUrl );
            aError = "Sorry, this site is not supported";
            return;

            DownloadTag theTag = new DownloadTag();


            //theTag.VideoTitle = theVideoTitle;
            //theTag.VideoUrl = new Uri( "http://www.youtube.com/get_video?" +
            //                           "video_id=" + theVideoId );
            //theTag.ThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            //theTag.FileName = theTag.VideoTitle;
            //theTag.FileExtension = "Flash Video (*.flv)|*.flv";

            if( aUrl.Host.StartsWith( "www" ) )
            {
                String theSource;
                if( !NetUtils.DownloadString( aUrl, out theSource ) )
                {
                    theTag.Error = "Failed to connect to " + aUrl.Host;
                    return;
                }

                String theNextUrlStr;
                if( !StringUtils.StringBetween( theSource, "<form id=\"ff\" action=\"", "\"", out theNextUrlStr ) )
                {
                    theTag.Error = "Failed to analyze source code";
                    return;
                }

                Uri theNextUrl;
                if( Uri.TryCreate( theNextUrlStr, UriKind.Absolute, out theNextUrl ) )
                {
                    TryParse( ref theNextUrl, ref aDownloadQueue, ref aUI, out aError );
                }
                else
                {
                    theTag.Error = "Unknown URL";
                }

                return;
            }

            //HttpWebRequest theRequest = (HttpWebRequest)WebRequest.Create( aUrl );
            //theRequest.UserAgent = SaveMedia.Program.UserAgent;
            //theRequest.Referer = aUrl.OriginalString;
            //theRequest.Method = "POST";
            //theRequest.ContentType = "application/x-www-form-urlencoded";
            //theRequest.Credentials = CredentialCache.DefaultCredentials;

            //String thePostData = "dl.start=Free";
            //theRequest.ContentLength = thePostData.Length;

            //System.IO.Stream theRequestStream = theRequest.GetRequestStream();
            //System.IO.StreamWriter theStreamWriter = new System.IO.StreamWriter( theRequestStream );
            //theStreamWriter.Write( thePostData );
            //theStreamWriter.Close();

            //HttpWebResponse theResponse = (HttpWebResponse)theRequest.GetResponse();
            //System.IO.Stream theResponseStream = theResponse.GetResponseStream();
            //System.IO.StreamReader theStreamReader = new System.IO.StreamReader( theResponseStream );

            //String theSourceCode = theStreamReader.ReadToEnd();
            //theStreamReader.Close();
            //theResponseStream.Close();
            //theResponse.Close();

            //String theFirstMirror;
            //if( !StringUtils.StringBetween( theSourceCode, "<form name=\"dlf\" action=\"", "\"", out theFirstMirror ) )
            //{
            //    theTag.Error = "Failed to extract mirror's URL";
            //    return;
            //}

            //Uri theDownloadUrl;
            //bool isValid = Uri.TryCreate( theFirstMirror, UriKind.Absolute, out theDownloadUrl );
            //if( !isValid )
            //{
            //    theTag.Error = "Unknown URL";
            //    return;
            //}

            //String theWaitingTimeStr;
            //int theWaitingTime;
            //if( !StringUtils.StringBetween( theSourceCode, "var c=", ";", out theWaitingTimeStr ) ||
            //    !int.TryParse( theWaitingTimeStr, out theWaitingTime ) )
            //{
            //    theTag.Error = "Failed to extract waiting time";
            //    return;
            //}

            //theTag.VideoUrl = theDownloadUrl;
            //theTag.WaitingTime = theWaitingTime;
            //theTag.FileName = System.IO.Path.GetFileName( theDownloadUrl.OriginalString );
            //theTag.FileExtension = System.IO.Path.GetExtension( theDownloadUrl.OriginalString );
        }

        private void DownloadRapidShareFile( ref Uri aUrl )
        {
            //mWaitingTime = theTag.WaitingTime;
            //mDelayTimer.Start();

            //theTag.DownloadDestination = FileUtils.SaveFile( theTag.FileName, theTag.FileExtension + "|*" + theTag.FileExtension, mUI.Win32Window );

            //if( String.IsNullOrEmpty( theTag.DownloadDestination ) )
            //{
            //    mUI.ChangeLayout( "Cancel clicked" );
            //    mDelayTimer.Stop();
            //    ClearTemporaryFiles();
            //    return;
            //}

            //mDownloadQueue.Add( theTag );

            //mDownloadButton.Visible = false;
            //mCancelButton.Visible = true;
        }
    }
}
