using System;
using System.Windows.Forms;

namespace SaveMedia
{
    public enum Phase_t
    {
        eInitialized,
        eParsingUrl,
        eDownloadStarted,
        eDownloadCompleted,
        eConvertStarted,
        eConvertCompleted
    };

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

        IWin32Window Win32Window{ get; }

        // ==================================
        // Functions
        // ==================================

        void Initialize( Controller aController, params ConverterTag[] aConverters );

        void ChangeLayout( Phase_t aPhase );

        void DisplayMediaInfo( DownloadTag aTag );

        String PromptForFolderDestination( String aDescription );

        DialogResult PromptForUpdate();

        void ShowError( String aErrorMessage );

        void ShowThumbnail( String aThumbnailPath );
    }
}