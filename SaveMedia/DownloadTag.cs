using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveMedia
{
    public class DownloadTag
    {
        public DownloadTag()
        {
            mUrl = null;
            mVideoTitle = String.Empty;
            mVideoUrl = null;
            mThumbnailUrl = null;
            mFilename = String.Empty;
            mFileExtension = String.Empty;
            mError = String.Empty;
        }

        public Uri Url
        {
            get{ return mUrl; }
            set{ mUrl = value; }
        }

        public String VideoTitle
        {
            get { return mVideoTitle; }
            set { mVideoTitle = value; }
        }

        public Uri VideoUrl
        {
            get { return mVideoUrl; }
            set { mVideoUrl = value; }
        }

        public Uri ThumbnailUrl
        {
            get { return mThumbnailUrl; }
            set { mThumbnailUrl = value; }
        }

        public String Filename
        {
            get { return mFilename; }
            set { mFilename = value; }
        }

        public String FileExtension
        {
            get { return mFileExtension; }
            set { mFileExtension = value; }
        }

        public String Error
        {
            get { return mError; }
            set { mError = value; }
        }

        private Uri     mUrl;
        private String  mVideoTitle;
        private Uri     mVideoUrl;
        private Uri     mThumbnailUrl;
        private String  mFilename;
        private String  mFileExtension;
        private String  mError;
    }
}
