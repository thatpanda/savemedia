using System;

namespace SaveMedia
{
    public class DownloadTag
    {
        public DownloadTag()
        {
            mVideoTitle = String.Empty;
            mVideoUrl = null;

            mThumbnailUrl = null;

            mDefaultFilename = String.Empty;
            mFileExtension = String.Empty;
            mDownloadDestination = String.Empty;

            mWaitingTime = 0;

            mError = String.Empty;
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
            get { return mDefaultFilename; }
            set { mDefaultFilename = value; }
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

        private String  mVideoTitle;
        private Uri     mVideoUrl;

        private Uri     mThumbnailUrl;

        private String  mDefaultFilename;
        private String  mFileExtension;
        private String  mDownloadDestination;

        private int     mWaitingTime;
        private String  mError;
    }
}
