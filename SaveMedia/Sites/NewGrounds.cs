using System;
using System.Collections.Generic;

using Utility;

namespace SaveMedia.Sites
{
    static class NewGrounds
    {
        public static void TryParse( ref Uri aUrl,
                                     ref List<DownloadTag> aDownloadQueue,
                                     out String aError )
        {
            aError = String.Empty;

            String theSourceCode;
            if( !NetUtils.DownloadString( aUrl, out theSourceCode ) )
            {
                aError = "Failed to connect to " + aUrl.Host;
                return;
            }

            String theVideoTitle;
            if( !StringUtils.StringBetween( theSourceCode, "<title>", "</title>", out theVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }

            String theVideoUrlStr;
            if( !StringUtils.StringBetween( theSourceCode, "var fw = new FlashWriter(\"", "\"", out theVideoUrlStr ) )
            {
                aError = "Failed to read video's URL";
                return;
            }

            String theThumbnailUrlStr;
            if( !StringUtils.StringBetween( theSourceCode, "<link rel=\"image_src\" href=\"", "\"", out theThumbnailUrlStr ) )
            {
                aError = "Failed to read video's thumbnail";
                return;
            }

            DownloadTag theTag = new DownloadTag();
            theTag.VideoTitle = Uri.UnescapeDataString( theVideoTitle );
            theTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
            theTag.VideoUrl = new Uri( theVideoUrlStr );
            theTag.FileName = theTag.VideoTitle;
            theTag.FileExtension = "Flash Movie (*.swf)|*.swf";

            aDownloadQueue.Add( theTag );
        }
    }
}
