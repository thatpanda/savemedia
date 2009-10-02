using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveMedia.Sites
{
    static class NewGrounds
    {
        public static void TryParse( Uri aUrl,
                                     out String aVideoTitle,
                                     out Uri    aVideoUrl,
                                     out Uri    aThumbnailUrl,
                                     out String aFilename,
                                     out String aFileExtension,
                                     out String aError )
        {
            aVideoTitle = String.Empty;
            aVideoUrl = aUrl;
            aThumbnailUrl = aUrl;
            aFilename = String.Empty;
            aFileExtension = String.Empty;
            aError = String.Empty;

            String theSourceCode;
            if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
            {
                aError = "Failed to connect to " + aUrl.Host;
                return;
            }

            if( !Utilities.StringBetween( theSourceCode, "<title>", "</title>", out aVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }
            aVideoTitle = Uri.UnescapeDataString( aVideoTitle );

            String theVideoUrlStr;
            if( !Utilities.StringBetween( theSourceCode, "var fw = new FlashWriter(\"", "\"", out theVideoUrlStr ) )
            {
                aError = "Failed to read video's URL";
                return;
            }

            String theThumbnailUrlStr;
            if( !Utilities.StringBetween( theSourceCode, "<link rel=\"image_src\" href=\"", "\"", out theThumbnailUrlStr ) )
            {
                aError = "Failed to read video's thumbnail";
                return;
            }

            aThumbnailUrl = new Uri( theThumbnailUrlStr );
            aVideoUrl     = new Uri( theVideoUrlStr );

            aFilename = aVideoTitle;
            aFileExtension = "Flash Movie (*.swf)|*.swf";
        }
    }
}
