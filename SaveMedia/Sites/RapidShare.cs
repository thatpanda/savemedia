using System;
using System.Net;

using Utility;

namespace SaveMedia.Sites
{
    static class RapidShare
    {
        public static void TryParse( ref Uri            aUrl,
                                     out DownloadTag    aTag )
        {
            aTag = new DownloadTag();


            //aTag.VideoTitle = theVideoTitle;
            //aTag.VideoUrl = new Uri( "http://www.youtube.com/get_video?" +
            //                           "video_id=" + theVideoId );
            //aTag.ThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            //aTag.FileName = aTag.VideoTitle;
            //aTag.FileExtension = "Flash Video (*.flv)|*.flv";

            if( aUrl.Host.StartsWith( "www" ) )
            {
                String theSource;
                if( !NetUtils.DownloadString( aUrl, out theSource ) )
                {
                    aTag.Error = "Failed to connect to " + aUrl.Host;
                    return;
                }

                String theNextUrlStr;
                if( !StringUtils.StringBetween( theSource, "<form id=\"ff\" action=\"", "\"", out theNextUrlStr ) )
                {
                    aTag.Error = "Failed to analyze source code";
                    return;
                }

                Uri theNextUrl;
                if( Uri.TryCreate( theNextUrlStr, UriKind.Absolute, out theNextUrl ) )
                {
                    TryParse( ref theNextUrl, out aTag );
                }
                else
                {
                    aTag.Error = "Unknown URL";
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
            //    aTag.Error = "Failed to extract mirror's URL";
            //    return;
            //}

            //Uri theDownloadUrl;
            //bool isValid = Uri.TryCreate( theFirstMirror, UriKind.Absolute, out theDownloadUrl );
            //if( !isValid )
            //{
            //    aTag.Error = "Unknown URL";
            //    return;
            //}

            //String theWaitingTimeStr;
            //int theWaitingTime;
            //if( !StringUtils.StringBetween( theSourceCode, "var c=", ";", out theWaitingTimeStr ) ||
            //    !int.TryParse( theWaitingTimeStr, out theWaitingTime ) )
            //{
            //    aTag.Error = "Failed to extract waiting time";
            //    return;
            //}

            //aTag.VideoUrl = theDownloadUrl;
            //aTag.WaitingTime = theWaitingTime;
            //aTag.FileName = System.IO.Path.GetFileName( theDownloadUrl.OriginalString );
            //aTag.FileExtension = System.IO.Path.GetExtension( theDownloadUrl.OriginalString );
        }
    }
}
