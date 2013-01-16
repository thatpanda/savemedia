using System;
using System.Collections.Generic;
using System.Globalization;

using Utility;

namespace SaveMedia.Sites
{
    class Tudou : ISite
    {
        public bool Support( ref Uri aUrl )
        {
            return aUrl.OriginalString.StartsWith( "http://www.tudou.com" );
        }

        public void TryParse( ref Uri aUrl,
                              ref List<DownloadTag> aDownloadQueue,
                              ref IMainForm aUI,
                              out String aError )
        {
            aError = String.Empty;

            aError = "Sorry, this site is no longer supported";
            return;

            String theVideoId = String.Empty;

            if( aUrl.OriginalString.ToLower( CultureInfo.CurrentCulture ).Contains( "iid=" ) )
            {
                // case 1 -> http://www.tudou.com/playlist/playindex.do?lid=3183353&iid=16030830
                System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( aUrl.Query );
                theVideoId = theQueryStrings[ "iid" ];
            }
            else
            {
                // case 2 -> http://www.tudou.com/programs/view/vjrv4umAuSk/

                String theSourceCode;
                if( !NetUtils.DownloadString( aUrl, out theSourceCode ) )
                {
                    aError = "Failed to connect to " + aUrl.Host;
                    return;
                }

                if( !StringUtils.StringBetween( theSourceCode, "iid=", "\"", out theVideoId ) )
                {
                    aError = "Failed to analyze video's URL";
                    return;
                }
            }

            Uri theXmlUrl = new Uri( "http://" + aUrl.Host + "/player/v.php?id=" + theVideoId );
            String theXmlSource = String.Empty;

            if( !NetUtils.DownloadString( theXmlUrl, out theXmlSource ) ||
                theXmlSource.Equals( "0" ) )
            {
                aError = "Video is not found";
                return;
            }

            String theVideoTitle;
            if( !StringUtils.StringBetween( theXmlSource, "q='", "'", out theVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }

            // this XML file provide the title as well, however the characters are encoded
            // they show up as &#27931;&#22855; &#34909;&#25758;&#36305;&#20301;
            theXmlUrl = new Uri( "http://v2.tudou.com/v2/cdn?id=" + theVideoId );

            if( !NetUtils.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aError = "Video is not found";
                return;
            }

            // <v time="74000" vi="1" ch="10" nls="0" title="洛奇 衝撞跑位" code="vjrv4umAuSk" enable="1" logo="0" band="0"><a/><b><f w="10" h="0" sha1="5c7b142105e6db7dc7e342b65fda12413c0cb975" size="1736164">http://123.129.251.201/flv/014/770/632/14770632.flv?key=7b83d716aa9581bd59145e49e26fee2422d102</f></b></v>
            String thePartialUrlString;
            if( !StringUtils.StringBetween( theXmlSource, "<f", "</f>", out thePartialUrlString ) )
            {
                aError = "Failed to read video's URL";
                return;
            }

            String theVideoUrlString = thePartialUrlString.Substring( thePartialUrlString.IndexOf( ">" ) + 1 );

            // this part was required for AutoIt
            //String theVideoFileSizeString;
            //if( !StringUtils.StringBetween( theXmlSource, "size=\"", "\"", out theVideoFileSizeString ) )
            //{
            //    aError = "Failed to read video's size";
            //    InputEnabled( true );
            //    return;
            //}

            String theThumbnailUrlString;
            if( !StringUtils.StringBetween( thePartialUrlString, "/flv/", ".flv", out theThumbnailUrlString ) )
            {
                aError = "Failed to read video's URL";
                return;
            }

            int theLastSlashIndex = theThumbnailUrlString.LastIndexOf( "/" );
            if( theLastSlashIndex == -1 )
            {
                aError = "Failed to read video's URL";
                return;
            }

            theThumbnailUrlString = "http://i01.img.tudou.com/data/imgs/i/" + theThumbnailUrlString.Remove( theLastSlashIndex );
            theThumbnailUrlString += "/p.jpg";

            DownloadTag theTag = new DownloadTag();
            theTag.VideoTitle = theVideoTitle;
            theTag.VideoUrl = new Uri( theVideoUrlString );
            theTag.ThumbnailUrl = new Uri( theThumbnailUrlString );
            theTag.FileName = theTag.VideoTitle;
            theTag.FileExtension = "Flash Video (*.flv)|*.flv";

            aDownloadQueue.Add( theTag );
        }
    }
}
