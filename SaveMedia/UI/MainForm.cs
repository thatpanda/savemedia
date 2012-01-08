using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Utility;

namespace SaveMedia
{
    public partial class MainForm : Form, IMainForm
    {
        private Controller mController = null;

        private String mDefaultTitle;

        delegate void ChangeLayoutCallBack( String aPhase );
        delegate void NotifyUserCallBack();
        delegate DialogResult PromptForUpdateCallBack();

        public MainForm()
        {
            InitializeComponent();

            mConversion = new CustomComboBox();
            mConversionGroupBox.Controls.Clear();
            mConversionGroupBox.Controls.Add( mConversion );
            mConversion.DrawMode = DrawMode.OwnerDrawFixed;
            mConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            mConversion.FormattingEnabled = true;
            mConversion.ItemHeight = 13;
            mConversion.Items.AddRange( new object[] {
                "Do not convert file",
                "MPEG-1 Audio Layer 3 (*.mp3)",
                "Windows Media Video (*.wmv)"} );
            mConversion.Location = new System.Drawing.Point( 6, 19 );
            mConversion.Name = "mConversion";
            mConversion.Size = new System.Drawing.Size( 368, 21 );
            mConversion.TabIndex = 2;

            ( (CustomComboBox) mConversion ).BuildImageList();

            mDefaultTitle = SaveMedia.Program.Title + " " + SaveMedia.Program.TitleVersion;
            this.Text = mDefaultTitle;

            mController = new Controller( this );

            if( !mController.ConverterExists )
            {
                mConversion.Items.Clear();
                mConversion.Items.Add( "Plug-in not found" );
                mConversion.Enabled = false;
            }
            
            mConversion.SelectedIndex = 0;
            this.StatusMessage = String.Empty;
        }

        public ComboBox ConversionComboBox
        {
            get
            {
                return (CustomComboBox) mConversion;
            }
        }

        public IWin32Window Win32Window
        {
            get
            {
                return this;
            }
        }

        public DialogResult PromptForUpdate()
        {
            if( this.InvokeRequired )
            {
                PromptForUpdateCallBack theCallBack = new PromptForUpdateCallBack( PromptForUpdate );
                return (DialogResult) this.Invoke( theCallBack );
            }

            return MessageBox.Show( this, "A newer version of SaveMedia is available.\n\nWould you like to download now?", "Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1 );
        }

        private void mOkButton_Click( object sender, EventArgs e )
        {
            ChangeLayout( "OK clicked" );
            mController.ClearTemporaryFiles();
        }

        private void mCancelButton_Click( object sender, EventArgs e )
        {
            mController.Abort();
        }

        private void mDownloadButton_Click( object sender, EventArgs e )
        {
            this.InputEnabled = false;

            mController.ParseUrl( mUrl.Text );
        }

        public void DisplayMediaInfo( DownloadTag aTag )
        {
            if( String.IsNullOrEmpty( aTag.VideoTitle ) )
            {
                return;
            }

            mTitleLabel.Text = "Title: " + aTag.VideoTitle.Replace( "\\", "" );
            mSizeLabel.Text = "Size: ??? MB";

            if( String.IsNullOrEmpty( aTag.Quality ) )
            {
                mQualityLabel.Text = String.Empty;
            }
            else
            {
                mQualityLabel.Text = "Quality: " + aTag.Quality;
            }

            mLocationLabel.Text = String.Empty;

            this.StatusMessage = "Ready to download";
            MediaInfoVisible( true );
        }

        public void DownloadStarted( String aDestination )
        {
            this.DownloadProgress = 0;
            this.Text = "0% - " + mDefaultTitle;
            this.StatusMessage = "Downloading...";
            //mLocationLabel.Text = "Location: " + aDestination;

            mDownloadButton.Visible = false;
            mCancelButton.Visible = true;
        }

        public void FileSize( String aValue )
        {
            mSizeLabel.Text = "Size: " + aValue;
        }

        public int DownloadProgress
        {
            set
            {
                this.Text = value + "% - " + mDefaultTitle;
                mProgressBar.Value = value;
            }
        }

        public void ConvertStarted()
        {
            this.ConversionProgress = 0;

            mDownloadButton.Visible = false;
            mCancelButton.Visible = true;
        }

        public String ThumbnailPath
        {
            set
            {
                mThumbnail.ImageLocation = value;
                mThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                mThumbnail.Refresh();
            }
        }

        public int ConversionProgress
        {
            set
            {
                this.Text = value.ToString() + "% - " + mDefaultTitle;
                mProgressBar.Value = value;

                this.StatusMessage = "Converting..." + value.ToString() + "%";
            }
        }

        public void ChangeLayout( String aPhase )
        {
            if( this.InvokeRequired )
            {
                ChangeLayoutCallBack theCallBack = new ChangeLayoutCallBack( ChangeLayout );
                this.Invoke( theCallBack, new object[] { aPhase } );
                return;
            }

            this.SuspendLayout();

            switch( aPhase )
            {
                case "Cancel clicked":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    this.StatusMessage = "Download cancelled";

                    MediaInfoVisible( false );
                    this.InputEnabled = true;
                    break;

                case "Conversion cancelled":
                case "Conversion completed":
                case "Conversion failed":
                case "Conversion failed, plug-in not found":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    this.StatusMessage = aPhase;

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                case "Downloading":

                    mDownloadButton.Visible = false;
                    mCancelButton.Visible = true;
                    break;

                case "Download cancelled":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    this.StatusMessage = aPhase;

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                case "Download completed":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    this.StatusMessage = aPhase;

                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                case "Download failed":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    this.StatusMessage = aPhase;

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                case "OK clicked":

                    this.InputEnabled = true;
                    MediaInfoVisible( false );
                    mProgressBar.Value = 0;
                    this.StatusMessage = String.Empty;

                    mDownloadButton.Visible = true;
                    mOkButton.Visible = false;
                    mCancelButton.Visible = false;
                    break;

                default:

                    throw new System.Exception( "Unknown phase" );
            }

            this.ResumeLayout( false );
            this.PerformLayout();
        }

        private void mUrl_TextChanged( object sender, EventArgs e )
        {
            Uri theUrl;
            mDownloadButton.Enabled = Uri.TryCreate( mUrl.Text, UriKind.Absolute, out theUrl );

            if( mDownloadButton.Enabled )
            {
                if( System.IO.File.Exists( mUrl.Text ) )
                {
                    mDownloadButton.Text = "Convert";
                }
                else
                {
                    mDownloadButton.Text = "Download";
                }
            }
        }

        public bool InputEnabled
        {
            set
            {
                mUrl.Enabled = value;
                mConversion.Enabled = value && mController.ConverterExists;

                if( value )
                {
                    String theNewUrl = ClipboardUtils.ReadUrl();
                    if( !mUrl.Text.Equals( theNewUrl ) && !String.IsNullOrEmpty( theNewUrl ) )
                    {
                        mUrl.Text = ClipboardUtils.ReadUrl();
                    }
                    mUrl_TextChanged( this, new EventArgs() );
                }
                else
                {
                    mDownloadButton.Enabled = false;
                }
            }
        }

        private void MediaInfoVisible( bool aIsVisible )
        {
            //int theMargin = 3;
            //int theNewHeight = mUrlGroupBox.Height + theMargin +
            //                   theMargin + mConversionGroupBox.Height;
            //mMediaInfoGroupBox.MinimumSize = new Size( mMediaInfoGroupBox.Width, theNewHeight );

            this.SuspendLayout();
            mUrlGroupBox.Visible = !aIsVisible;
            mConversionGroupBox.Visible = !aIsVisible;
            mMediaInfoGroupBox.Visible = aIsVisible;
            mThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            mThumbnail.Image = mThumbnail.InitialImage;
            mThumbnail.Refresh();
            this.ResumeLayout( false );
            this.PerformLayout();
        }

        public String StatusMessage{ set { mStatus.Text = value; } }

        private void MainForm_FormClosed( object sender, FormClosedEventArgs e )
        {
            mController.ClearTemporaryFiles();
        }

        private void NotifyUser()
        {
            if( this.InvokeRequired )
            {
                NotifyUserCallBack theCallBack = new NotifyUserCallBack( NotifyUser );
                this.Invoke( theCallBack );
                return;
            }

            if( !this.ContainsFocus )
            {
                FormUtils.FlashWindow( this );
            }
        }

        private void mAboutToolStripMenuItem_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Form theForm = new AboutBox();
            theForm.ShowDialog( this );
        }

        private void mOptionsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Form theForm = new OptionsForm();
            theForm.ShowDialog( this );
        }

        private void MainForm_Activated( object sender, EventArgs e )
        {
            String theNewUrl = ClipboardUtils.ReadUrl();
            if( mUrl.Enabled && !mUrl.Text.Equals( theNewUrl ) && !String.IsNullOrEmpty( theNewUrl ) )
            {
                mUrl.Text = theNewUrl;
            }
        }
    }
}