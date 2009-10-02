using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveMedia.Sites
{
    class Vimeo
    {
        public static void TryParse( Uri aUrl,
                                     out String aVideoTitle,
                                     out Uri aVideoUrl,
                                     out Uri aThumbnailUrl,
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

            String theSourceCode;
            if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
            {
                aError = "Failed to connect to " + aUrl.Host;
                return;
            }

            String theVideoId;
            if( !Utilities.StringBetween( theSourceCode, "http://vimeo.com/moogaloop.swf?clip_id=", "\"", out theVideoId ) )
            {
                aError = "Failed to read video's ID";
                return;
            }

            String theThumbnailUrlStr;
            if( !Utilities.StringBetween( theSourceCode, "<link rel=\"videothumbnail\" href=\"", "\"", out theThumbnailUrlStr ) )
            {
                aError = "Failed to read video's thumbnail";
                return;
            }

            Uri theXmlUrl = new Uri( "http://vimeo.com/moogaloop/load/clip:" + theVideoId + "/local?param_clip_id=" + theVideoId );

            String theXmlSource;
            if( !Utilities.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aError = "Video is not found";
                return;
            }

            if( !Utilities.StringBetween( theXmlSource, "<caption>", "</caption>", out aVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }

            String theRequestSignature;
            if( !Utilities.StringBetween( theXmlSource, "<request_signature>", "</request_signature>", out theRequestSignature ) )
            {
                aError = "Failed to read video's signature";
                return;
            }

            String theRequestSignatureExpires;
            if( !Utilities.StringBetween( theXmlSource, "<request_signature_expires>", "</request_signature_expires>", out theRequestSignatureExpires ) )
            {
                aError = "Failed to read video's signature";
                return;
            }

            String theVideoUrlStr = "http://vimeo.com/moogaloop/play/clip:" + theVideoId + "/" + theRequestSignature + "/" + theRequestSignatureExpires + "/?q=sd&type=local";

            aThumbnailUrl = new Uri( theThumbnailUrlStr );
            aVideoUrl = new Uri( theVideoUrlStr );

            aFilename = aVideoTitle;
            aFileExtension = "Flash Video (*.flv)|*.flv";
        }
    }
}
