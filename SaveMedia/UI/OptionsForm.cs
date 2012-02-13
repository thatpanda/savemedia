using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SaveMedia
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();

            switch( Settings.CheckForUpdates() )
            {
                case "auto":
                    mAutoUpdate.Checked = true;
                    break;
                case "when fails":
                    mUpdateWhenFails.Checked = true;
                    break;
                case "never":
                    mNeverUpdate.Checked = true;
                    break;
                default:
                    mAutoUpdate.Checked = true;
                    HandleCheckForUpdatesChanged( mAutoUpdate, EventArgs.Empty );
                    break;
            }

            switch( Settings.YouTubeQuality() )
            {
                case "34":
                    mNormalQuality.Checked = true;
                    break;
                case "18":
                    mFmt18.Checked = true;
                    break;
                case "35":
                    mFmt35.Checked = true;
                    break;
                case "22":
                    mFmt22.Checked = true;
                    break;
                case "37":
                    mFmt37.Checked = true;
                    break;
                default:
                    mNormalQuality.Checked = true;
                    HandleYoutubeQualityChanged( mNormalQuality, EventArgs.Empty );
                    break;
            }
        }

        private void OptionsForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            Settings.Save();
        }

        private void HandleCheckForUpdatesChanged( object sender, EventArgs e )
        {
            RadioButton theButton = (RadioButton)sender;

            if( theButton.Checked )
            {
                Settings.CheckForUpdates( theButton.Tag.ToString() );
            }
        }

        private void HandleYoutubeQualityChanged( object sender, EventArgs e )
        {
            RadioButton theButton = (RadioButton) sender;

            if( theButton.Checked )
            {
                Settings.YouTubeQuality( theButton.Tag.ToString() );
            }
        }
    }
}
