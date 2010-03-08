using System;

using Utility;

namespace SaveMedia.Sites
{
    static class CollegeHumor
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

            String theVideoId;
            if( !StringUtils.StringBetween( theSourceCode, "<link rel=\"canonical\" href=\"", "\"", out theVideoId ) )
            {
                aTag.Error = "Failed to read video's ID";
                return;
            }

            Uri theXmlUrl = new Uri( "http://www.collegehumor.com/moogaloop" + theVideoId );

            String theXmlSource;
            if( !NetUtils.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aTag.Error = "Failed to connect to " + theXmlUrl.Host;
                return;
            }

            String theVideoTitle;
            if( !StringUtils.StringBetween( theXmlSource, "<caption>", "</caption>", out theVideoTitle ) )
            {
                aTag.Error = "Failed to read video's title";
                return;
            }

            String theVideoUrlStr;
            if( !StringUtils.StringBetween( theXmlSource, "<file>", "</file>", out theVideoUrlStr ) )
            {
                aTag.Error = "Failed to read video's Url";
                return;
            }

            String theThumbnailUrlStr;
            if( !StringUtils.StringBetween( theXmlSource, "<thumbnail>", "</thumbnail>", out theThumbnailUrlStr ) )
            {
                aTag.Error = "Failed to read video's thumbnail";
                return;
            }

            aTag.VideoTitle = theVideoTitle;
            aTag.VideoUrl = new Uri( theVideoUrlStr );
            aTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
            aTag.FileName = aTag.VideoTitle;
            aTag.FileExtension = "Flash Video (*.flv)|*.flv";
        }
    }
}
