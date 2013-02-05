﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Utility;

namespace SaveMedia.Sites
{
    class YouTube : ISite
    {
        public bool Support( ref Uri aUrl )
        {
            return aUrl.Host.EndsWith( ".youtube.com" );
        }

        public void TryParse( ref Uri aUrl,
                              ref List<DownloadTag> aDownloadQueue,
                              ref IMainForm aUI,
                              out String aError )
        {
            aError = String.Empty;

            Uri theRequestUrl = aUrl;
            bool isPlaylist;

            ParseUrl( ref aUrl, out theRequestUrl, out isPlaylist );

            if( isPlaylist )
            {
                ParseYouTubePlaylist( ref theRequestUrl, ref aDownloadQueue, ref aUI, out aError );
            }
            else
            {
                ParseYouTubeVideo( ref theRequestUrl, ref aDownloadQueue, out aError );
            }
        }

        private void ParseYouTubeVideo( ref Uri aUrl,
                                        ref List<DownloadTag> aDownloadQueue,
                                        out String aError )
        {
            aError = String.Empty;

            System.Collections.Specialized.NameValueCollection theUrlQuery = System.Web.HttpUtility.ParseQueryString( aUrl.Query );
            String theVideoId = theUrlQuery["v"];
            if( String.IsNullOrEmpty( theVideoId ) )
            {
                aError = theVideoId;
                return;
            }

            Uri theXmlUrl = new Uri( "http://www.youtube.com/get_video_info?&video_id=" + theVideoId );
            String theVideoInfoXml;
            if( !NetUtils.DownloadString( theXmlUrl, out theVideoInfoXml ) )
            {
                aError = "Failed to receive video's info";
                return;
            }

            System.Collections.Specialized.NameValueCollection theVideoInfo = System.Web.HttpUtility.ParseQueryString( theVideoInfoXml );
            if( String.IsNullOrEmpty( theVideoInfo[ "status" ] ) || theVideoInfo[ "status" ].Equals( "fail" ) )
            {
                aError = theVideoInfo[ "reason" ];
                return;
            }

            String theVideoTitle = theVideoInfo[ "title" ];
            if( String.IsNullOrEmpty( theVideoTitle ) )
            {
                aError = "Failed to extract video's title";
                return;
            }

            String theSourceCode;
            if( !NetUtils.DownloadString( aUrl, out theSourceCode ) )
            {
                aError = "Failed to connect to " + aUrl.Host;
                return;
            }

            String theToken = theVideoInfo[ "token" ];
            if( String.IsNullOrEmpty( theToken ) )
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
            theFmtStreamMap = theVideoInfo[ "url_encoded_fmt_stream_map" ];
            if( String.IsNullOrEmpty( theFmtStreamMap ) )
            {
                aError = "Failed to extract video's fmt stream map";
                return;
            }

            String[] theSplitStr = new String[1];
            theSplitStr[ 0 ] = ",";
            String[] theFmtItems = theFmtStreamMap.Split( theSplitStr, StringSplitOptions.RemoveEmptyEntries );
            if( theFmtItems.Length == 0 )
            {
                aError = "Failed to read video's fmt stream map";
                return;
            }

            String theUrl = String.Empty;
            foreach( String theItem in theFmtItems )
            {
                // Don't use .NET System.Uri.UnescapeDataString in URL Decoding
                // http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx
                //String theFmtItem = System.Web.HttpUtility.UrlDecode( theEncodedItem );

                System.Collections.Specialized.NameValueCollection theCollection = System.Web.HttpUtility.ParseQueryString( theItem );
                if( theCollection[ "itag" ] == theAvailableFmt || System.String.IsNullOrEmpty( theAvailableFmt ) )
                {
                    theUrl = theCollection[ "url" ] + "&signature=" + theCollection[ "sig" ];
                    break;
                }
            }

            if( System.String.IsNullOrEmpty( theUrl ) )
            {
                aError = "Failed to get video's URL";
                return;
            }

            DownloadTag theTag = new DownloadTag();
            theTag.VideoTitle = theVideoTitle;
            theTag.VideoUrl = new Uri( theUrl );
            theTag.Quality = QualityStr( theAvailableFmt );
            theTag.ThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            theTag.FileName = theTag.VideoTitle;
            theTag.FileExtension = "Flash Video (*.flv)|*.flv";

            aDownloadQueue.Add( theTag );
        }

        private void ParseUrl( ref Uri aUrl, out Uri aRequestUrl, out bool aIsPlaylist )
        {
            String theUrlString = aUrl.OriginalString;

            //theUrlString = theUrlString.Replace( "https://", "http://" );
            theUrlString = theUrlString.Replace( "http://www.youtube.com/v/", "http://www.youtube.com/watch?v=" );
            theUrlString = theUrlString.Replace( "http://www.youtube.com/view_play_list?p=", "http://www.youtube.com/playlist?list=" );

            aRequestUrl = new Uri( theUrlString );
            aIsPlaylist = aRequestUrl.OriginalString.StartsWith( "http://www.youtube.com/playlist?list=" );
        }

        private void ParseYouTubePlaylist( ref Uri aUrl,
                                           ref List<DownloadTag> aDownloadQueue,
                                           ref IMainForm aUI,
                                           out String aError )
        {
            aError = String.Empty;

            System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( aUrl.Query );
            String thePlaylistId = theQueryStrings[ "list" ];

            String theSourceCode;
            if( !NetUtils.DownloadString( aUrl, out theSourceCode ) )
            {
                aError = "Failed to connect to " + aUrl.Host;
                return;
            }

            String thePlaylistTitle;
            if( !StringUtils.StringBetween( theSourceCode, "<meta property=\"og:title\" content=\"", "\"", out thePlaylistTitle ) &&
                !StringUtils.StringBetween( theSourceCode, "<h1 title=\"", "\"", out thePlaylistTitle ) &&
                !StringUtils.StringBetween( theSourceCode, "<h1>", "</h1>", out thePlaylistTitle ) )
            {
                aError = "Failed to read playlist's title";
                return;
            }

            // .NET 4.0 or above can use System.Net.WebUtility.HtmlDecode
            thePlaylistTitle = System.Web.HttpUtility.HtmlDecode( thePlaylistTitle );

            // Regular Expression Language - Quick Reference
            // http://msdn.microsoft.com/en-us/library/az24scfc.aspx
            String theUrlPattern = "<li\\s+class=\"playlist-video-item.+?<a\\shref=\"([^\"]+)";
            Match theUrlMatch = Regex.Match( theSourceCode, theUrlPattern, RegexOptions.Singleline );
            if( !theUrlMatch.Success )
            {
                aError = "Failed to read playlist";
                return;
            }

            String thePromptDescription = thePlaylistTitle + "\n\nWhere would you like to save the videos?";
            String thePlaylistDestination = aUI.PromptForFolderDestination( thePromptDescription );
            if( String.IsNullOrEmpty( thePlaylistDestination ) )
            {
                return;
            }

            while( String.IsNullOrEmpty( aError ) &&
                   theUrlMatch.Success )
            {
                String thePartialUrl = theUrlMatch.Groups[ 1 ].ToString();
                Uri theVideoUrl = new Uri( "http://" + aUrl.Host + thePartialUrl );

                ParseYouTubeVideo( ref theVideoUrl, ref aDownloadQueue, out aError );

                DownloadTag theTag = aDownloadQueue[ aDownloadQueue.Count - 1 ];
                theTag.DownloadDestination = thePlaylistDestination + "\\" + FileUtils.FilenameCheck( theTag.FileName ) + ".flv";

                theUrlMatch = theUrlMatch.NextMatch();
            }
        }

        private static String AvailableQuality( String aFmtMap, String aPreferedQuality )
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
