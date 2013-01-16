using System;

namespace SaveMedia
{
    public class DownloadTag
    {
        public DownloadTag()
        {
            mVideoTitle = String.Empty;

            mDefaultFileName = String.Empty;
            mFileExtension = String.Empty;
            mDownloadDestination = String.Empty;
        }

        // ==================================
        // Properties
        // ==================================

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

        public String Quality
        {
            get { return mQuality; }
            set { mQuality = value; }
        }

        public Uri ThumbnailUrl
        {
            get { return mThumbnailUrl; }
            set { mThumbnailUrl = value; }
        }

        public String FileName
        {
            get { return mDefaultFileName; }
            set { mDefaultFileName = value; }
        }

        public String FileExtension
        {
            get { return mFileExtension; }
            set { mFileExtension = value; }
        }

        public String DownloadDestination
        {
            get { return mDownloadDestination; }
            set { mDownloadDestination = value; }
        }

        public int WaitingTime
        {
            get { return mWaitingTime; }
            set { mWaitingTime = value; }
        }

        public String Error
        {
            get { return mError; }
            set { mError = value; }
        }

        // ==================================
        // Members
        // ==================================

        private String  mVideoTitle;
        private Uri     mVideoUrl;
        private String  mQuality;

        private Uri     mThumbnailUrl;

        private String  mDefaultFileName;
        private String  mFileExtension;
        private String  mDownloadDestination;

        private int     mWaitingTime;
        private String  mError;
    }
}
