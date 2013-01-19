using System;
using System.Windows.Forms;

namespace SaveMedia
{
    public interface IMainForm
    {
        // ==================================
        // Properties
        // ==================================

        int ConversionProgress { set; }
        int DownloadProgress { set; }
        bool InputEnabled { set; }
        ConverterTag SelectedConverter { get; }
        String StatusMessage { set; }
        String ThumbnailPath { set; }

        IWin32Window Win32Window{ get; }

        // ==================================
        // Functions
        // ==================================

        void Initialize( Controller aController, params ConverterTag[] aConverters );

        void ChangeLayout( String aPhase );

        void ConvertStarted();

        void DisplayMediaInfo( DownloadTag aTag );

        void DownloadStarted( String aDestination );

        void FileSize( String aValue );

        bool PromptForFolderDestination( ref String aDescription, out String aDestination );

        DialogResult PromptForUpdate();
    }
}