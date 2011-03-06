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
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            mTitle.Text = SaveMedia.Program.Title + " " + SaveMedia.Program.TitleVersion;
            mDate.Text = SaveMedia.Program.Date;

            mCopyright.Text = SaveMedia.Program.Copyright;
        }

        private void theEmail_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "mailto:savemedia@notfaqs.com" );
        }

        private void theWeb_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://savemedia.googlecode.com/" );
        }

        private void theFFmpegLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://blog.k-tai-douga.com/" );
        }

        private void theCollegeHumorLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://www.collegehumor.com/" );
        }

        private void theNewGroundsLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://www.newgrounds.com/" );
        }

        private void theRapidShareLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://www.rapidshare.com/" );
        }

        private void theTudouLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://www.tudou.com/" );
        }

        private void theVimeoLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://vimeo.com/" );
        }

        private void theYouTubeLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( "http://www.youtube.com/" );
        }
    }
}
