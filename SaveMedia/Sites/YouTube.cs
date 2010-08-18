using System;

using Utility;

namespace SaveMedia.Sites
{
    static class YouTube
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
            if( !StringUtils.StringBetween( theSourceCode, "<meta name=\"title\" content=\"", "\">", out theVideoTitle ) )
            {
                aTag.Error = "Failed to extract video's title";
                return;
            }

            String theVideoId;
            if( !StringUtils.StringBetween( theSourceCode, "&video_id=", "&", out theVideoId ) )
            {
                aTag.Error = "Failed to extract video's id";
                return;
            }

            //Uri theXmlUrl = new Uri( "http://www.youtube.com/get_video_info?&video_id=" + theVideoId );
            //if( !NetUtils.DownloadString( theXmlUrl, out theSourceCode ) )
            //{
            //    aTag.Error = "Failed to receive video's info";
            //    return;
            //}

            String theToken;
            if( !StringUtils.StringBetween( theSourceCode, "&t=", "&", out theToken ) )
            {
                aTag.Error = "Failed to extract token";
                return;
            }

            String theFmtMap;
            if( !StringUtils.StringBetween( theSourceCode, "&fmt_list=", "&", out theFmtMap ) )
            {
                aTag.Error = "Failed to extract video's fmt map";
                return;
            }

            String thePreferedQuality = Settings.YouTubeQuality();
            String theAvailableFmt = AvailableQuality( theFmtMap, thePreferedQuality );

            String theFmtStreamMap;
            if( !StringUtils.StringBetween( theSourceCode, "&fmt_stream_map=", "&", out theFmtStreamMap ) )
            {
                aTag.Error = "Failed to extract video's fmt stream map";
                return;
            }
            theFmtStreamMap = System.Web.HttpUtility.UrlDecode( theFmtStreamMap );

            String[] theUrls = theFmtStreamMap.Split( ',' );
            if( theUrls.Length == 0 )
            {
                aTag.Error = "Failed to split video's URLs";
                return;
            }

            String theUrl;
            if( !StringUtils.StringBetween( theUrls[ 0 ], "|", "||", out theUrl ) )
            {
                aTag.Error = "Failed to extract video's URL";
                return;
            }

            if( !System.String.IsNullOrEmpty( theAvailableFmt ) )
            {
                foreach( System.String theLink in theUrls )
                {
                    if( theLink.StartsWith( theAvailableFmt + "|" ) )
                    {
                        if( !StringUtils.StringBetween( theLink, "|", "||", out theUrl ) )
                        {
                            aTag.Error = "Failed to extract video's URL";
                            return;
                        }
                        break;
                    }
                }
            }

            //System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( theFullScreenUrl.Query );

            //String theVideoTitle = theQueryStrings[ "title" ];
            //String theVideoId    = theQueryStrings[ "video_id" ];
            //String theToken      = theQueryStrings[ "t" ];
            //String theFmtMap     = theQueryStrings[ "fmt_map" ];

            aTag.VideoTitle = theVideoTitle;
            aTag.VideoUrl = new Uri( theUrl );
            aTag.Quality = QualityStr( theAvailableFmt );
            aTag.ThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            aTag.FileName = aTag.VideoTitle;
            aTag.FileExtension = "Flash Video (*.flv)|*.flv";
        }

        public static String AvailableQuality( String aFmtMap, String aPreferedQuality )
        {
            if( String.IsNullOrEmpty( aFmtMap ) ||
                String.IsNullOrEmpty( aPreferedQuality ) )
            {
                return String.Empty;
            }

            // %2F = /
            // %2C = ,
            if( aFmtMap.Contains( "%2F" ) || aFmtMap.Contains( "%2C" ) )
            {
                aFmtMap = Uri.UnescapeDataString( aFmtMap );
            }

            // "fmt_map": "35/640000/9/0/115,18/512000/9/0/115,34/0/9/0/115,5/0/7/0/0"
            aFmtMap = "," + aFmtMap;

            int thePreferedQuality = Convert.ToInt32( aPreferedQuality );

            if( aFmtMap.IndexOf( "," + aPreferedQuality + "/" ) != -1 )
            {
                return aPreferedQuality;
            }
            else if( aFmtMap.IndexOf( ",22/" ) != -1 &&
                     thePreferedQuality != 35 &&
                     thePreferedQuality != 18 &&
                     thePreferedQuality != 34 )
            {
                return "22";
            }
            else if( aFmtMap.IndexOf( ",35/" ) != -1 &&
                     thePreferedQuality != 18 &&
                     thePreferedQuality != 34 )
            {
                return "35";
            }
            else if( aFmtMap.IndexOf( ",6/" ) != -1 &&
                     thePreferedQuality != 18 &&
                     thePreferedQuality != 34 )
            {
                return "6";    // fmt 6 should be replaced by 35
            }
            else if( aFmtMap.IndexOf( ",18/" ) != -1 &&
                     thePreferedQuality != 34 )
            {
                return "18";
            }
            else if( aFmtMap.IndexOf( ",34/" ) != -1 )
            {
                return "34";
            }

            return String.Empty;
        }

        public static String QualityStr( String aFmtArg )
        {
            if( String.IsNullOrEmpty( aFmtArg ) )
            {
                return String.Empty;
            }

            switch( aFmtArg )
            {
                case "37":
                    return "HD (1080p)";
                case "22":
                    return "HD (720p)";
                case "35":
                case "6":
                    return "HQ";
                case "18":
                    return "iPod";
                default:
                    return "Standard";
            }
        }
    }
}
