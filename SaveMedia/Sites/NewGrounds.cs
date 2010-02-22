using System;

using Utility;

namespace SaveMedia.Sites
{
    static class NewGrounds
    {
        public static void TryParse( ref Uri            aUrl,
                                     out DownloadTag    aTag )
        {
            aTag = new DownloadTag();

            String theSourceCode;
            if( !NetUtils.DownloadString( aUrl, out theSourceCode ) )
            {
                aTag.Error = "Failed to connect to " + aUrl.Host;
                return;
            }

            String theVideoTitle;
            if( !StringUtils.StringBetween( theSourceCode, "<title>", "</title>", out theVideoTitle ) )
            {
                aTag.Error = "Failed to read video's title";
                return;
            }

            String theVideoUrlStr;
            if( !StringUtils.StringBetween( theSourceCode, "var fw = new FlashWriter(\"", "\"", out theVideoUrlStr ) )
            {
                aTag.Error = "Failed to read video's URL";
                return;
            }

            String theThumbnailUrlStr;
            if( !StringUtils.StringBetween( theSourceCode, "<link rel=\"image_src\" href=\"", "\"", out theThumbnailUrlStr ) )
            {
                aTag.Error = "Failed to read video's thumbnail";
                return;
            }

            aTag.VideoTitle = Uri.UnescapeDataString( theVideoTitle );
            aTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
            aTag.VideoUrl = new Uri( theVideoUrlStr );
            aTag.Filename = aTag.VideoTitle;
            aTag.FileExtension = "Flash Movie (*.swf)|*.swf";
        }
    }
}
