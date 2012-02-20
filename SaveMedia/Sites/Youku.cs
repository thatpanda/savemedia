// Based on get-flash-videos
// https://github.com/monsieurvideo/get-flash-videos/blob/master/lib/FlashVideo/Site/Youku.pm

using System;
using System.Collections.Generic;

using Utility;

namespace SaveMedia.Sites
{
    static class Youku
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

            // MV 来自四川的歌声
            // http://v.youku.com/v_show/id_XOTA4ODg2NTI=.html
            // Last Exile
            // http://v.youku.com/v_show/id_XMTgzODA0MTY4.html

            // var videoId = '63254965';
            // var videoId2= 'XMjUzMDE5ODYw';
            String theVideoId;
            if( !StringUtils.StringBetween( theSourceCode, "var videoId = '", "'", out theVideoId ) &&
                !StringUtils.StringBetween( theSourceCode, "var videoId2= '", "'", out theVideoId ) )
            {
                aError = "Failed to read video's ID";
                return;
            }

            // http://v.youku.com/player/getPlayList/VideoIDS/%s/version/5/source/video/password/?ran=%d&n=%d
            // http://v.youku.com/player/getPlayList/VideoIDS/XMjUzMDE5ODYw/version/5/source/video/Type/Folder/Fid/5558308/Pt/7/Ob/1?n=3&ran=6084&password=
            Uri theJsonUrl = new Uri( "http://v.youku.com/player/getPlayList/VideoIDS/" + theVideoId );

            String theJson;
            if( !NetUtils.DownloadString( theJsonUrl, out theJson ) )
            {
                aError = "Video is not found";
                return;
            }

            /*
            {"data":[{"ct":"g",
            "cs":"2221",
            "logo":"http:\/\/g1.ykimg.com\/1100641F464D8993C0008C036C1E3731A0FCC0-1A6F-46FE-38D4-BA9BA9A3162A",
            "seed":1890,
            "tags":["\u51ef\u745f\u7433",
            "\u6e38\u620f"],
            "categories":"99",
            "videoid":"63254965",
            "vidEncoded":"XMjUzMDE5ODYw",
            "username":"\u827e\u7f57\u65af\u7279\u62c9\u7279",
            "userid":"57417271",
            "title":"\u51ef\u745f\u7433\u4e2d\u6587\u5267\u60c5\u7b2c8\u96c6",
            "key1":"b14a3a4d",
            "key2":"1eb8d90416697bb2",
            "tt":"0",
            "seconds":"1228.07",
            "streamfileids":{
            "hd2":"54*61*54*54*54*55*54*61*54*54*34*22*56*61*55*34*8*54*54*3*61*54*54*61*8*32*55*21*61*25*54*61*55*59*61*3*54*55*14*2*59*32*15*14*49*55*49*2*14*62*54*2*8*14*61*59*56*62*32*3*8*56*25*22*15*32*",
            "mp4":"54*61*54*54*54*62*54*61*54*54*34*22*62*56*15*61*32*56*54*3*61*54*54*61*8*32*55*21*61*25*54*61*55*59*61*3*54*55*14*2*59*32*15*14*49*55*49*2*14*62*54*2*8*14*61*59*56*62*32*3*8*56*25*22*15*32*",
            "flv":"54*61*54*54*54*59*54*61*54*54*34*22*62*56*56*61*32*54*54*3*61*54*54*61*8*32*55*21*61*25*54*61*55*59*61*3*54*55*14*2*59*32*15*14*49*55*49*2*14*62*54*2*8*14*61*59*56*62*32*3*8*56*25*22*15*32*"},
            "segs":{
            "hd2":[{"no":"0","size":"63570505","seconds":"423"},
                   {"no":"1","size":"54202118","seconds":"423"},
                   {"no":"2","size":"46589169","seconds":"383"}],
            "mp4":[{"no":"0","size":"28938844","seconds":"423"},
                   {"no":"1","size":"26018448","seconds":"423"},
                   {"no":"2","size":"22237273","seconds":"382"}],
            "flv":[{"no":"0","size":"13561507","seconds":"423"},
                   {"no":"1","size":"12854219","seconds":"421"},
                   {"no":"2","size":"12719580","seconds":"384"}]},
            "streamsizes":{"hd2":"164361792",
            "mp4":"77194565",
            "flv":"39135306"},
            "streamtypes":["hd2","flvhd","mp4"],
            "streamtypes_o":["hd2","flvhd","mp4"]}],
            "user":{"id":0},
            "controller":{"search_count":true,
            "mp4_restrict":1,
            "stream_mode":1,
            "share_disabled":false,
            "download_disabled":false,
            "video_capture":true,
            "area_code":480,
            "dma_code":753,
            "continuous":1}}*/

            String theSeed;
            if( !StringUtils.StringBetween( theJson, "\"seed\":", ",", out theSeed ) )
            {
                aError = "Failed to read video's seed";
                return;
            }

            //String theStreamTypes;
            String theStreamFileIds;
            if( !StringUtils.StringBetween( theJson, "\"streamfileids\":{", "}", out theStreamFileIds ) )
            {
                aError = "Failed to read the encrypted video ID";
                return;
            }

            List<string> theDecrypter = Shuffle( ref theSeed );
            Dictionary<String, String> theStreamType2Id = StreamType2Id( ref theStreamFileIds, ref theDecrypter );

            String thePreferredType = "mp4";
            String theStreamType = thePreferredType;
            String theFileId = String.Empty;
            if( theStreamType2Id.ContainsKey( thePreferredType ) )
            {
                theFileId = theStreamType2Id[ thePreferredType ];
            }
            else
            {
                foreach( String theKey in theStreamType2Id.Keys )
                {
                    theStreamType = theKey;
                    theFileId = theStreamType2Id[theStreamType];
                    break;
                }
            }

            String theSegments;
            if( !StringUtils.StringBetween( theJson, "\"" + theStreamType + "\":[", "]", out theSegments ) )
            {
                aError = "Failed to read the segments";
                return;
            }
            List<String> theFileSegmentIds = new List<string>();
            List<String> theFileSegmentSeconds = new List<string>();
            List<String> theFileSegmentKeys = new List<string>();
            if( !ParseSegmentData( ref theFileId, ref theSegments, ref theFileSegmentIds, ref theFileSegmentSeconds, ref theFileSegmentKeys ) )
            {
                aError = "Failed to parse segment data";
                return;
            }

            String theKey1;
            if( !StringUtils.StringBetween( theJson, "\"key1\":\"", "\"", out theKey1 ) )
            {
                aError = "Failed to read the 1st key";
                return;
            }

            String theKey2;
            if( !StringUtils.StringBetween( theJson, "\"key2\":\"", "\"", out theKey2 ) )
            {
                aError = "Failed to read the 2nd key";
                return;
            }

            String theFixedValue = "a55aa5a5";
            String theFinalKey = theKey2 + HexXor( ref theKey1, ref theFixedValue );

            String theVideoTitle;
            if( !StringUtils.StringBetween( theJson, "\"title\":\"", "\"", out theVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }
            theVideoTitle = ReadUnicodeTitle( ref theVideoTitle );

            String theThumbnailUrlStr;
            if( !StringUtils.StringBetween( theJson, "\"logo\":\"", "\"", out theThumbnailUrlStr ) )
            {
                aError = "Failed to read video's thumbnail";
                return;
            }
            theThumbnailUrlStr = theThumbnailUrlStr.Replace( "\\", "" );

            String theDestination = String.Empty;
            if( theFileSegmentIds.Count > 1 )
            {
                // TODO:
                System.Windows.Forms.FolderBrowserDialog theDialog = new System.Windows.Forms.FolderBrowserDialog();
                theDialog.Description = "Please select the destination for videos from:\n\n" + theVideoTitle;
                if( theDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK )
                {
                    aError = "Download cancelled";
                    return;
                }
                theDestination = theDialog.SelectedPath;
            }

            String theSessionId = SessionId();
            for( int theIndex = 0; theIndex < theFileSegmentIds.Count; theIndex++ )
            {
                if( theFileSegmentIds.Count == theFileSegmentKeys.Count )
                {
                    theFinalKey = theFileSegmentKeys[ theIndex ];
                }
                DownloadTag theTag = new DownloadTag();
                theTag.VideoTitle = theVideoTitle;
                if( theFileSegmentIds.Count > 1 )
                {
                    theTag.VideoTitle += " part " + ( theIndex + 1 );
                }
                theTag.VideoUrl = new Uri( "http://f.youku.com/player/getFlvPath/sid/" + theSessionId + "/st/" + theStreamType + "/fileid/" + theFileSegmentIds[ theIndex ] + "?K=" + theFinalKey + "&hd=1&myp=0&ts=" + theFileSegmentSeconds[ theIndex ] );
                theTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
                theTag.FileName = theTag.VideoTitle;
                //theTag.FileExtension = "Flash Video (*.flv)|*.flv";
                theTag.FileExtension = "MPEG-4 (*.mp4)|*.mp4";

                if( !String.IsNullOrEmpty( theDestination ) )
                {
                    theTag.DownloadDestination = theDestination + "\\" + FileUtils.FilenameCheck( theTag.FileName ) + ".mp4";
                }

                aDownloadQueue.Add( theTag );
            }
        }

        public static String ReadUnicodeTitle( ref String aTitle )
        {
            // aTitle = "\\u51ef\\u745f\\u7433\\u4e2d\\u6587\\u5267\\u60c5\\u7b2c8\\u96c6";
            String theUnicodeEscapeChar = "\\u";

            String theTitle = aTitle;
            int theIndex = theTitle.IndexOf( theUnicodeEscapeChar );
            while( theIndex != -1 )
            {
                String theFirstHalf = theTitle.Substring( 0, theIndex );
                String theUtf32Str = theTitle.Substring( theIndex + theUnicodeEscapeChar.Length, 4 );
                String theSecondHalf = theTitle.Substring( theIndex + theUnicodeEscapeChar.Length + 4 );

                int theUtf32 = Convert.ToInt32( theUtf32Str, 16 );
                String theUnicodeChar = Char.ConvertFromUtf32( theUtf32 );
                theTitle = theFirstHalf + theUnicodeChar + theSecondHalf;

                theIndex = theTitle.IndexOf( theUnicodeEscapeChar );
            }

            return theTitle;
        }

        public static String HexXor( ref String aHex, ref String aHex2 )
        {
            int theInt = Convert.ToInt32( aHex, 16 );
            int theInt2 = Convert.ToInt32( aHex2, 16 );
            int theResult = theInt ^ theInt2;
            return Convert.ToString( theResult, 16 );
        }

        public static String SessionId()
        {
            DateTime t = DateTime.Now;
            TimeSpan theTimeSpan = ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0 ) );
            double theUnixTime = theTimeSpan.TotalSeconds;
            String theUnixTimeStr = theUnixTime.ToString().Replace( ".", "" );
            Random r = new Random();

            return String.Format( "{0}1{1:00000}_00", theUnixTimeStr, r.Next( 10000 ) );
        }

        public static String DecryptId( ref String aEncryptedId, ref List<string> aDecryptor )
        {
            String theId = String.Empty;
            String[] theChars = aEncryptedId.Split( '*' );
            foreach( String theChar in theChars )
            {
                if( String.IsNullOrEmpty( theChar ) )
                {
                    continue;
                }
                int theIndex;
                int.TryParse( theChar, out theIndex );

                if( theIndex < aDecryptor.Count )
                {
                    theId += aDecryptor[ theIndex ];
                }
            }

            return theId;
        }

        public static bool ParseSegmentData( ref String aFileId,
                                             ref String aSegments,
                                             ref List<String> aFileSegmentIds,
                                             ref List<String> aFileSegmentSeconds,
                                             ref List<String> aFileSegmentKeys )
        {
            /*
            {"no":"0","size":"28938844","seconds":"423"},
            {"no":"1","size":"26018448","seconds":"423"},
            {"no":"2","size":"22237273","seconds":"382"}
            */
            /*
            {"no":"0","size":"31798590","seconds":"422","k":"4d8850a693ad090c28275823","k2":"142e3c35692c9e1c1"},
            {"no":"1","size":"22067001","seconds":"421","k":"ebef6d1cd2f0da5d28275823","k2":"15854034fbcf97988"},
            {"no":"2","size":"23828637","seconds":"422","k":"27ed05369f1fe3312410dca1","k2":"15735c4ee6fb926fe"},
            */
            String[] theSegments = aSegments.Split( '}' );
            foreach( String theSegment in theSegments )
            {
                String theSegmentNumber;
                if( StringUtils.StringBetween( theSegment, "\"no\":\"", "\"", out theSegmentNumber ) ||
                    StringUtils.StringBetween( theSegment, "\"no\":", ",", out theSegmentNumber ) )
                {
                    theSegmentNumber = String.Format( "{0:X2}", Convert.ToInt32( theSegmentNumber, 10 ) );
                    String theFileSegmentId = aFileId.Substring( 0, 8 ) + theSegmentNumber + aFileId.Substring( 10 );
                    aFileSegmentIds.Add( theFileSegmentId );
                }

                String theSecond;
                if( StringUtils.StringBetween( theSegment, "\"seconds\":\"", "\"", out theSecond ) )
                {
                    aFileSegmentSeconds.Add( theSecond );
                }

                String theKey;
                if( StringUtils.StringBetween( theSegment, "\"k\":\"", "\"", out theKey ) )
                {
                    aFileSegmentKeys.Add( theKey );
                }
            }

            return aFileSegmentIds.Count != 0;
        }

        public static Dictionary<String, String> StreamType2Id( ref String aStreamFileIds, ref List<string> aDecryptor )
        {
            /*
            "hd2":"54*61*54*54*54*55*54*61*54*54*34*22*56*61*55*34*8*54*54*3*61*54*54*61*8*32*55*21*61*25*54*61*55*59*61*3*54*55*14*2*59*32*15*14*49*55*49*2*14*62*54*2*8*14*61*59*56*62*32*3*8*56*25*22*15*32*",
            "mp4":"54*61*54*54*54*62*54*61*54*54*34*22*62*56*15*61*32*56*54*3*61*54*54*61*8*32*55*21*61*25*54*61*55*59*61*3*54*55*14*2*59*32*15*14*49*55*49*2*14*62*54*2*8*14*61*59*56*62*32*3*8*56*25*22*15*32*",
            "flv":"54*61*54*54*54*59*54*61*54*54*34*22*62*56*56*61*32*54*54*3*61*54*54*61*8*32*55*21*61*25*54*61*55*59*61*3*54*55*14*2*59*32*15*14*49*55*49*2*14*62*54*2*8*14*61*59*56*62*32*3*8*56*25*22*15*32*"
            */
            Dictionary<String, String> theType2Id = new Dictionary<String, String>();

            String[] theList = aStreamFileIds.Split( ',' );
            foreach( String theItem in theList )
            {
                String[] theKeyValue = theItem.Split( ':' );
                if( theKeyValue.Length == 2 )
                {
                    String theType = theKeyValue[ 0 ].Trim().Trim( '"' );
                    String theEncryptedId = theKeyValue[ 1 ].Trim().Trim( '"' );
                    String theDecryptedId = DecryptId( ref theEncryptedId, ref aDecryptor );
                    theType2Id.Add( theType, theDecryptedId );
                }
            }

            return theType2Id;
        }

        public static List<string> Shuffle( ref String aSeed )
        {
            // Modified Fisher-Yates shuffle
            // http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
            String theRoll = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/\\:._-1234567890";

            List<string> theShuffle = new List<string>();

            for( int theIndex = 0; theIndex < theRoll.Length; ++theIndex )
            {
                theShuffle.Add( theRoll.Substring( theIndex, 1 ) );
            }

            double theSeed;
            double.TryParse( aSeed, out theSeed );
            for( int theIndex = 0; theIndex < theRoll.Length; ++theIndex )
            {
                // PRNG is a standard linear congruential generator
                // with a = 211, c = 30031, and m = 2^16
                theSeed = ( 211 * theSeed + 30031 ) % Math.Pow( 2, 16 );

                double x = theSeed / Math.Pow( 2, 16 ) * ( theRoll.Length - theIndex );
                theShuffle.Add( theShuffle[ (int)x ] );
                theShuffle.RemoveAt( (int)x );
            }

            return theShuffle;
        }
    }
}
