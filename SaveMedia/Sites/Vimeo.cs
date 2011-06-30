using System;
using System.Collections.Generic;

using Utility;

namespace SaveMedia.Sites
{
    static class Vimeo
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

            String theVideoId;
            if( !StringUtils.StringBetween( theSourceCode, "property=\"og:url\" content=\"http://vimeo.com/", "\"", out theVideoId ) &&
                !StringUtils.StringBetween( theSourceCode, "clipid=", ";", out theVideoId )  )
            {
                aError = "Failed to read video's ID";
                return;
            }

            //"thumbnail":"http:\/\/b.vimeocdn.com\/ts\/131\/533\/131533953_640.jpg"
            String theThumbnailUrlStr;
            if( !StringUtils.StringBetween( theSourceCode, "\"thumbnail\":\"", "\"", out theThumbnailUrlStr ) )
            {
                aError = "Failed to read video's thumbnail";
                return;
            }

            theThumbnailUrlStr = theThumbnailUrlStr.Replace( "\\", "" );

            Uri theXmlUrl = new Uri( "http://vimeo.com/moogaloop/load/clip:" + theVideoId + "/local?param_clip_id=" + theVideoId );

            String theXmlSource;
            if( !NetUtils.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aError = "Video is not found";
                return;
            }

            String theVideoTitle;
            if( !StringUtils.StringBetween( theXmlSource, "<caption>", "</caption>", out theVideoTitle ) )
            {
                aError = "Failed to read video's title";
                return;
            }

            String theRequestSignature;
            if( !StringUtils.StringBetween( theXmlSource, "<request_signature>", "</request_signature>", out theRequestSignature ) )
            {
                aError = "Failed to read video's signature";
                return;
            }

            String theRequestSignatureExpires;
            if( !StringUtils.StringBetween( theXmlSource, "<request_signature_expires>", "</request_signature_expires>", out theRequestSignatureExpires ) )
            {
                aError = "Failed to read video's signature";
                return;
            }

            DownloadTag theTag = new DownloadTag();
            theTag.VideoTitle = theVideoTitle;
            theTag.VideoUrl = new Uri( "http://vimeo.com/moogaloop/play/clip:" + theVideoId + "/" + theRequestSignature + "/" + theRequestSignatureExpires + "/?q=sd&type=local" );
            theTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
            theTag.FileName = theTag.VideoTitle;
            theTag.FileExtension = "Flash Video (*.flv)|*.flv";

            aDownloadQueue.Add( theTag );
        }
    }
}
