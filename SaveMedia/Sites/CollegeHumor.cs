using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SaveMedia.Sites
{
    class CollegeHumor
    {
        public static void TryParse( Uri                aUrl,
                                     out DownloadTag    aTag )
        {
            aTag = new DownloadTag();

            String theSourceCode;
            if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
            {
                aTag.Error = "Failed to connect to " + aUrl.Host;
                return;
            }

            String theVideoId;
            if( !Utilities.StringBetween( theSourceCode, "<link rel=\"canonical\" href=\"", "\"", out theVideoId ) )
            {
                aTag.Error = "Failed to read video's ID";
                return;
            }

            Uri theXmlUrl = new Uri( "http://www.collegehumor.com/moogaloop" + theVideoId );

            String theXmlSource;
            if( !Utilities.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aTag.Error = "Failed to connect to " + theXmlUrl.Host;
                return;
            }

            String theVideoTitle;
            if( !Utilities.StringBetween( theXmlSource, "<caption>", "</caption>", out theVideoTitle ) )
            {
                aTag.Error = "Failed to read video's title";
                return;
            }

            String theVideoUrlStr;
            if( !Utilities.StringBetween( theXmlSource, "<file>", "</file>", out theVideoUrlStr ) )
            {
                aTag.Error = "Failed to read video's Url";
                return;
            }

            String theThumbnailUrlStr;
            if( !Utilities.StringBetween( theXmlSource, "<thumbnail>", "</thumbnail>", out theThumbnailUrlStr ) )
            {
                aTag.Error = "Failed to read video's thumbnail";
                return;
            }

            aTag.VideoTitle = theVideoTitle;
            aTag.VideoUrl = new Uri( theVideoUrlStr );
            aTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
            aTag.Filename = aTag.VideoTitle;
            aTag.FileExtension = "Flash Video (*.flv)|*.flv";
        }
    }
}
