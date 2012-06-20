using System;

namespace SaveMedia
{
    public class ConverterTag
    {
        public ConverterTag( String aDisplayString, String aFileExtension, String aFFmpegArg )
        {
            mDisplayString = aDisplayString;
            mFFmpegArg = aFFmpegArg;
            mFileExtension = aFileExtension;
        }

        // ==================================
        // System.Object functions
        // ==================================

        public override String ToString()
        {
            return mDisplayString;
        }

        // ==================================
        // Properties
        // ==================================

        public String FFmpegArg
        {
            get { return mFFmpegArg; }
        }

        public String FileExtension
        {
            get { return mFileExtension; }
        }

        public bool IsValid
        {
            get
            {
                return !String.IsNullOrEmpty( FFmpegArg ) &&
                       !String.IsNullOrEmpty( FileExtension );
            }
        }

        // ==================================
        // Members
        // ==================================

        private String mDisplayString;
        private String mFFmpegArg;
        private String mFileExtension;
    }
}
