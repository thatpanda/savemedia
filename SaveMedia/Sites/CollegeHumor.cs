using System;
using System.Collections.Generic;

using Utility;

namespace SaveMedia.Sites
{
    static class CollegeHumor
    {
        public static void TryParse( ref Uri                 aUrl,
                                     ref List< DownloadTag > aDownloadQueue,
                                     out String              aError )
        {
            aError = String.Empty;

            String theSourceCode;
            if( !NetUtils.DownloadString( aUrl, out theSourceCode ) )
            {
                aError = "Failed to connect to " + aUrl.Host;
                return;
            }

            String theVideoId;
            if( !StringUtils.StringBetween( theSourceCode, "<link rel=\"canonical\" href=\"", "\"", out theVideoId ) )
            {
                aError = "Failed to read video's ID";
                return;
            }

            Uri theXmlUrl = new Uri( "http://www.collegehumor.com/moogaloop" + theVideoId );

            String theXmlSource;
            if( !NetUtils.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aError = "Failed to connect to " + theXmlUrl.Host;
                return;
            }

            String theVideoTitle;
            if( !StringUtils.StringBetween( theXmlSource, "<caption>", "</caption>", out theVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }

            String theVideoUrlStr;
            if( !StringUtils.StringBetween( theXmlSource, "<file>", "</file>", out theVideoUrlStr ) )
            {
                aError = "Failed to read video's Url";
                return;
            }

            String theThumbnailUrlStr;
            if( !StringUtils.StringBetween( theXmlSource, "<thumbnail>", "</thumbnail>", out theThumbnailUrlStr ) )
            {
                aError = "Failed to read video's thumbnail";
                return;
            }

            DownloadTag theTag = new DownloadTag();
            theTag.VideoTitle = theVideoTitle;
            theTag.VideoUrl = new Uri( theVideoUrlStr );
            theTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
            theTag.FileName = theTag.VideoTitle;
            theTag.FileExtension = "Flash Video (*.flv)|*.flv";

            aDownloadQueue.Add( theTag );
        }
    }
}
