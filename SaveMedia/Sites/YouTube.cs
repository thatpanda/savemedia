using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveMedia.Sites
{
    static class YouTube
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

            String theVideoTitle;
            if( !Utilities.StringBetween( theSourceCode, "<meta name=\"title\" content=\"", "\">", out theVideoTitle ) )
            {
                aTag.Error = "Failed to extract video's title";
                return;
            }

            String theVideoId;
            if( !Utilities.StringBetween( theSourceCode, "\"video_id\": \"", "\"", out theVideoId ) )
            {
                aTag.Error = "Failed to extract video's id";
                return;
            }

            String theToken;
            if( !Utilities.StringBetween( theSourceCode, "\"t\": \"", "\"", out theToken ) )
            {
                aTag.Error = "Failed to extract token";
                return;
            }

            String theFmtMap;
            if( !Utilities.StringBetween( theSourceCode, "\"fmt_map\": \"", "\"", out theFmtMap ) )
            {
                aTag.Error = "Failed to extract video's fmt map";
                return;
            }

            //System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( theFullScreenUrl.Query );

            //String theVideoTitle = theQueryStrings[ "title" ];
            //String theVideoId    = theQueryStrings[ "video_id" ];
            //String theToken      = theQueryStrings[ "t" ];
            //String theFmtMap     = theQueryStrings[ "fmt_map" ];

            String thePreferedQuality = Settings.YouTubeQuality();
            String theQuality = Utilities.YouTubeAvailableQuality( theFmtMap, thePreferedQuality );

            aTag.VideoTitle = theVideoTitle;
            aTag.VideoUrl = new Uri( "http://www.youtube.com/get_video?" +
                                       "video_id=" + theVideoId +
                                       "&t=" + theToken +
                                       theQuality );
            aTag.ThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            aTag.Filename = aTag.VideoTitle;
            aTag.FileExtension = "Flash Video (*.flv)|*.flv";
        }
    }
}
