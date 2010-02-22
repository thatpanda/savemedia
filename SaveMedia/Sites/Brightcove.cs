using System;

namespace SaveMedia.Sites
{
    class Brightcove
    {
        public static void TryParse( ref Uri    aUrl,
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

            //http://link.brightcove.com/services/player/bcpid6555681001?bctid=24835989001
            //http://link.brightcove.com/services/player/bcpid2517822001?bctid=2553471001
            /*
            Uri theAmfUrl = new Uri( "http://c.brightcove.com/services/messagebroker/amf" );
            HttpWebRequest theRequest = (HttpWebRequest)WebRequest.Create( theAmfUrl );
            theRequest.UserAgent = SaveMedia.Program.UserAgent;
            theRequest.Referer = "http://admin.brightcove.com/viewer/us1.20.02.00/federated_video.swf?isVid=true&purl=" + aUrl.OriginalString;
            theRequest.Method = "POST";
            theRequest.ContentType = "application/x-amf";
            theRequest.Credentials = CredentialCache.DefaultCredentials;

            String thePostData = "000300000001004d636f6d2e627269676874636f76652e657870657269656e63652e457870657269656e636552756e74696d654661636164652e67657450726f6772616d6d696e67576974684f766572726964657300022f31000000c00a000000020041f86bfc0e900000110903010a810353636f6d2e627269676874636f76652e657870657269656e63652e436f6e74656e744f7665727269646519636f6e74656e7452656649641b666561747572656452656649641b636f6e74656e7452656649647317636f6e74656e745479706515636f6e74656e7449647315666561747572656449640d74617267657413636f6e74656e744964010101040001057fffffffffffffff0617766964656f506c61796572054217215c78240000";
            //String thePostData = "000000000001003a636f6d2e627269676874636f76652e636174616c6f672e436174616c6f674661636164652e66696e64566964656f466f725075626c697368657200022f32000000270a0000000502000a323535333437313030310041d9efaa8e0000000101050041e2c25d46200000";
            byte[] thePostBytes = Utilities.HexToBytes( thePostData );
            theRequest.ContentLength = thePostBytes.Length;

            System.IO.Stream theRequestStream = theRequest.GetRequestStream();
            theRequestStream.Write( thePostBytes, 0, thePostBytes.Length );
            theRequestStream.Close();

            HttpWebResponse theResponse = (HttpWebResponse)theRequest.GetResponse();
            System.IO.Stream theResponseStream = theResponse.GetResponseStream();
            byte[] theResponseBytes = new byte[ theResponse.ContentLength ];
            theResponseStream.Read( theResponseBytes, 0, theResponseBytes.Length );
            theResponseStream.Close();
            theResponse.Close();

            String theHexString = Utilities.BytesToHex( theResponseBytes );
            String theResponseString = Utilities.HexToAscii( theHexString );
            */
        }
    }
}
