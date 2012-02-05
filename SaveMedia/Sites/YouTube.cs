using System;
using System.Collections.Generic;

using Utility;

namespace SaveMedia.Sites
{
    static class YouTube
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
            if( !StringUtils.StringBetween( theSourceCode, "<meta name=\"title\" content=\"", "\">", out theVideoTitle ) )
            {
                aError = "Failed to extract video's title";
                return;
            }

            String theVideoId;
            if( !StringUtils.StringBetween( theSourceCode, "'VIDEO_ID': \"", "\"", out theVideoId ) &&
                !StringUtils.StringBetween( theSourceCode, "\"VIDEO_ID\": \"", "\"", out theVideoId ) &&
                !StringUtils.StringBetween( theSourceCode, "\\u0026amp;video_id=", "\\u0026amp;", out theVideoId ) &&
                !StringUtils.StringBetween( theSourceCode, "&video_id=", "&", out theVideoId ) &&
                !StringUtils.StringBetween( theSourceCode, "&amp;video_id=", "&amp;", out theVideoId ) )
            {
                aError = "Failed to extract video's id";
                return;
            }

            //Uri theXmlUrl = new Uri( "http://www.youtube.com/get_video_info?&video_id=" + theVideoId );
            //if( !NetUtils.DownloadString( theXmlUrl, out theSourceCode ) )
            //{
            //    aError = "Failed to receive video's info";
            //    return;
            //}

            String theToken;
            if( !StringUtils.StringBetween( theSourceCode, "\"t\": \"", "\"", out theToken ) && 
                !StringUtils.StringBetween( theSourceCode, "\\u0026amp;t=", "\\u0026amp;", out theToken ) &&
                !StringUtils.StringBetween( theSourceCode, "\\u0026amp;t=", "\"", out theToken ) &&
                !StringUtils.StringBetween( theSourceCode, "&t=", "&", out theToken ) &&
                !StringUtils.StringBetween( theSourceCode, "&amp;t=", "&amp;", out theToken ) )
            {
                aError = "Failed to extract token";
                return;
            }

            String theFmtMap;
            if( !StringUtils.StringBetween( theSourceCode, "\"fmt_list\": \"", "\"", out theFmtMap ) && 
                !StringUtils.StringBetween( theSourceCode, "\\u0026amp;fmt_list=", "\\u0026amp;", out theFmtMap ) &&
                !StringUtils.StringBetween( theSourceCode, "&fmt_list=", "&", out theFmtMap ) &&
                !StringUtils.StringBetween( theSourceCode, "&amp;fmt_list=", "&amp;", out theFmtMap ) )
            {
                aError = "Failed to extract video's fmt map";
                return;
            }

            String thePreferedQuality = Settings.YouTubeQuality();
            String theAvailableFmt = AvailableQuality( theFmtMap, thePreferedQuality );

            String theFmtStreamMap;
            if( !StringUtils.StringBetween( theSourceCode, "\\u0026amp;url_encoded_fmt_stream_map=", "\\u0026amp;", out theFmtStreamMap ) && 
                !StringUtils.StringBetween( theSourceCode, "&amp;url_encoded_fmt_stream_map=", "&amp;", out theFmtStreamMap ) &&
                !StringUtils.StringBetween( theSourceCode, "&fmt_stream_map=", "&", out theFmtStreamMap ) &&
                !StringUtils.StringBetween( theSourceCode, "&amp;fmt_stream_map=", "&amp;", out theFmtStreamMap ) )
            {
                aError = "Failed to extract video's fmt stream map";
                return;
            }
            theFmtStreamMap = System.Web.HttpUtility.UrlDecode( theFmtStreamMap );
            if( theFmtStreamMap.ToLower().Contains( "%2f" ) )
            {
                theFmtStreamMap = Uri.UnescapeDataString( theFmtStreamMap );
            }
            theFmtStreamMap = "," + theFmtStreamMap;

            String[] theSplitStr = new String[1];
            theSplitStr[ 0 ] = ",url=";
            String[] theUrls = theFmtStreamMap.Split( theSplitStr, StringSplitOptions.RemoveEmptyEntries );
            if( theUrls.Length == 0 )
            {
                aError = "Failed to split video's URLs";
                return;
            }

            // TODO: check for valid URL
            String theUrl = theUrls[0];

            // debug only
            //System.Windows.Forms.MessageBox.Show(
            //    theUrl + "\n\n" +
            //    System.Web.HttpUtility.UrlDecode( theUrl ) + "\n\n" +
            //    Uri.UnescapeDataString( theUrl ) );

            if( !System.String.IsNullOrEmpty( theAvailableFmt ) )
            {
                //System.Windows.Forms.MessageBox.Show( theAvailableFmt );
                foreach( System.String theLink in theUrls )
                {
                    if( theLink.Contains( "&itag=" + theAvailableFmt + "&" ) ||
                        theLink.EndsWith( "&itag=" + theAvailableFmt ) )
                    {
                        int theFallbackHostIndex = theLink.IndexOf( "&fallback_host=" );
                        if( theFallbackHostIndex != -1 )
                        {
                            theUrl = theLink.Substring( 0, theFallbackHostIndex );
                        }
                        else
                        {
                            theUrl = theLink;
                        }
                        //System.Windows.Forms.MessageBox.Show( theLink );
                        //System.Windows.Forms.Clipboard.SetText( theLink );
                        break;
                    }
                }
            }

            //System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( theFullScreenUrl.Query );

            //String theVideoTitle = theQueryStrings[ "title" ];
            //String theVideoId    = theQueryStrings[ "video_id" ];
            //String theToken      = theQueryStrings[ "t" ];
            //String theFmtMap     = theQueryStrings[ "fmt_map" ];

            DownloadTag theTag = new DownloadTag();
            theTag.VideoTitle = theVideoTitle;
            theTag.VideoUrl = new Uri( theUrl );
            theTag.Quality = QualityStr( theAvailableFmt );
            theTag.ThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            theTag.FileName = theTag.VideoTitle;
            theTag.FileExtension = "Flash Video (*.flv)|*.flv";

            aDownloadQueue.Add( theTag );
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
            // "fmt_list": "44\/854x480\/99\/0\/0,35\/854x480\/9\/0\/115,43\/640x360\/99\/0\/0,34\/640x360\/9\/0\/115,18\/640x360\/9\/0\/115,5\/320x240\/7\/0\/0"
            if( aFmtMap.Contains( "%2F" ) || aFmtMap.Contains( "%2C" ) )
            {
                aFmtMap = Uri.UnescapeDataString( aFmtMap );
            }
            else if( aFmtMap.Contains( "\\" ) )
            {
                aFmtMap = aFmtMap.Replace( "\\", String.Empty );
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
