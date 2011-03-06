﻿using System;

using Utility;

namespace SaveMedia.Sites
{
    static class Vimeo
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

            String theVideoId;
            if( !StringUtils.StringBetween( theSourceCode, "property=\"og:url\" content=\"http://vimeo.com/", "\"", out theVideoId ) &&
                !StringUtils.StringBetween( theSourceCode, "clipid=", ";", out theVideoId )  )
            {
                aTag.Error = "Failed to read video's ID";
                return;
            }

            //"thumbnail":"http:\/\/b.vimeocdn.com\/ts\/131\/533\/131533953_640.jpg"
            String theThumbnailUrlStr;
            if( !StringUtils.StringBetween( theSourceCode, "\"thumbnail\":\"", "\"", out theThumbnailUrlStr ) )
            {
                aTag.Error = "Failed to read video's thumbnail";
                return;
            }

            theThumbnailUrlStr = theThumbnailUrlStr.Replace( "\\", "" );

            Uri theXmlUrl = new Uri( "http://vimeo.com/moogaloop/load/clip:" + theVideoId + "/local?param_clip_id=" + theVideoId );

            String theXmlSource;
            if( !NetUtils.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                aTag.Error = "Video is not found";
                return;
            }

            String theVideoTitle;
            if( !StringUtils.StringBetween( theXmlSource, "<caption>", "</caption>", out theVideoTitle ) )
            {
                aTag.Error = "Failed to read video's title";
                return;
            }

            String theRequestSignature;
            if( !StringUtils.StringBetween( theXmlSource, "<request_signature>", "</request_signature>", out theRequestSignature ) )
            {
                aTag.Error = "Failed to read video's signature";
                return;
            }

            String theRequestSignatureExpires;
            if( !StringUtils.StringBetween( theXmlSource, "<request_signature_expires>", "</request_signature_expires>", out theRequestSignatureExpires ) )
            {
                aTag.Error = "Failed to read video's signature";
                return;
            }

            aTag.VideoTitle = theVideoTitle;
            aTag.VideoUrl = new Uri( "http://vimeo.com/moogaloop/play/clip:" + theVideoId + "/" + theRequestSignature + "/" + theRequestSignatureExpires + "/?q=sd&type=local" );
            aTag.ThumbnailUrl = new Uri( theThumbnailUrlStr );
            aTag.FileName = aTag.VideoTitle;
            aTag.FileExtension = "Flash Video (*.flv)|*.flv";
        }
    }
}
