using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SaveMedia.Sites
{
    class CollegeHumor
    {
        public static void TryParse( Uri        aUrl,
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

            String theVideoId;
            if( !Utilities.StringBetween( theSourceCode, "<link rel=\"canonical\" href=\"", "\"", out theVideoId ) )
            {
                aError = "Failed to read video's ID";
                return;
            }

            Uri theXmlUrl = new Uri( "http://www.collegehumor.com/moogaloop" + theVideoId );

            String theXmlSource;
            if( !Utilities.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aError = "Failed to connect to " + theXmlUrl.Host;
                return;
            }

            String theVideoTitle;
            if( !Utilities.StringBetween( theXmlSource, "<caption>", "</caption>", out theVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }

            String theVideoUrlStr;
            if( !Utilities.StringBetween( theXmlSource, "<file>", "</file>", out theVideoUrlStr ) )
            {
                aError = "Failed to read video's Url";
                return;
            }

            String theThumbnailUrlStr;
            if( !Utilities.StringBetween( theXmlSource, "<thumbnail>", "</thumbnail>", out theThumbnailUrlStr ) )
            {
                aError = "Failed to read video's thumbnail";
                return;
            }

            aVideoTitle = theVideoTitle;
            aVideoUrl = new Uri( theVideoUrlStr );
            aThumbnailUrl = new Uri( theThumbnailUrlStr );
            aFilename = aVideoTitle;
            aFileExtension = "Flash Video (*.flv)|*.flv";
        }
    }
}
