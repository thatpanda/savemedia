using System;
using System.Windows.Forms;

namespace SaveMedia
{
    public interface IMainForm
    {
        // ==================================
        // Properties
        // ==================================

        ComboBox ConversionComboBox{ get; }

        int ConversionProgress{ set; }
        int DownloadProgress { set; }
        bool InputEnabled { set; }
        String StatusMessage{ set; }
        String ThumbnailPath{ set; }

        IWin32Window Win32Window{ get; }

        // ==================================
        // Functions
        // ==================================

        void ChangeLayout( String aPhase );

        void ConvertStarted();

        void DisplayMediaInfo( DownloadTag aTag );

        void DownloadStarted( String aDestination );

        void FileSize( String aValue );

        DialogResult PromptForUpdate();
    }
}