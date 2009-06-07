using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SaveMedia
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();

            String theYouTubeQuality = Settings.YouTubeQuality();

            switch( theYouTubeQuality )
            {
                case "6":
                    mFmt6.Checked = true;
                    break;
                case "18":
                    mFmt18.Checked = true;
                    break;
                case "22":
                    mFmt22.Checked = true;
                    break;
                default:
                    mNormalQuality.Checked = true;
                    break;
            }
        }

        private void OptionsForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            Settings.Save();
        }

        private void HandleQualitySettingsChanged( object sender, EventArgs e )
        {
            String theValue = "0";

            if( mFmt6.Checked )
            {
                theValue = "6";
            }
            else if( mFmt18.Checked )
            {
                theValue = "18";
            }
            else if( mFmt22.Checked )
            {
                theValue = "22";
            }

            Settings.YouTubeQuality( ref theValue );
        }
    }
}
