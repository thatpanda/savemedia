﻿using System;

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
            if( !StringUtils.StringBetween( theSourceCode, "\"video_id\": \"", "\"", out theVideoId ) )
            {
                aTag.Error = "Failed to extract video's id";
                return;
            }

            String theToken;
            if( !StringUtils.StringBetween( theSourceCode, "\"t\": \"", "\"", out theToken ) )
            {
                aTag.Error = "Failed to extract token";
                return;
            }

            String theFmtMap;
            if( !StringUtils.StringBetween( theSourceCode, "\"fmt_map\": \"", "\"", out theFmtMap ) )
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
            String theQuality = YouTubeAvailableQuality( theFmtMap, thePreferedQuality );

            aTag.VideoTitle = theVideoTitle;
            aTag.VideoUrl = new Uri( "http://www.youtube.com/get_video?" +
                                       "video_id=" + theVideoId +
                                       "&t=" + theToken +
                                       theQuality );
            aTag.ThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            aTag.FileName = aTag.VideoTitle;
            aTag.FileExtension = "Flash Video (*.flv)|*.flv";
        }

        public static String YouTubeAvailableQuality( String aFmtMap, String aPreferedQuality )
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
                return "&fmt=" + aPreferedQuality;
            }
            else if( thePreferedQuality > 22 && aFmtMap.IndexOf( ",22/" ) != -1 )
            {
                return "&fmt=22";
            }
            else if( thePreferedQuality > 18 && aFmtMap.IndexOf( ",18/" ) != -1 )
            {
                return "&fmt=18";
            }
            else if( thePreferedQuality > 6 && aFmtMap.IndexOf( ",6/" ) != -1 )
            {
                return "&fmt=6";
            }

            return String.Empty;
        }
    }
}
