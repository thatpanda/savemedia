using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SaveMedia
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            mTitle.Text = SaveMedia.Program.Title + " " + SaveMedia.Program.TitleVersion;
            mDate.Text = Utility.FileUtils.RetrieveLinkerTimestamp().ToLongDateString();

            mCopyright.Text = SaveMedia.Program.Copyright;
        }

        private void theLogo_Click( object sender, EventArgs e )
        {
            System.Diagnostics.Process.Start( "http://savemedia.googlecode.com/" );
        }

        private void mTitle_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://savemedia.googlecode.com/" );
        }

        private void mCopyright_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "mailto:savemedia@notfaqs.com" );
        }

        private void theFFmpegLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://ffmpeg.zeranoe.com/builds/" );
            //System.Diagnostics.Process.Start( "http://blog.k-tai-douga.com/" );
        }

        private void mDonate_Click( object sender, EventArgs e )
        {
            System.Diagnostics.Process.Start( "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=GAJG6H5TGNK4G&lc=US&item_name=SaveMedia&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_LG%2egif%3aNonHosted" );
        }
    }
}
