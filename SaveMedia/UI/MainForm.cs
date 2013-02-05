using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Utility;

namespace SaveMedia
{
    public partial class MainForm : Form, IMainForm
    {
        public MainForm()
        {
            InitializeComponent();

            FormUtils.SetCueText( mUrl, "Video URL" );

            mConversionComboBox = new CustomComboBox();
            mConversionComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            mConversionComboBox.Font = mConversion.Font;
            mConversionComboBox.Name = "mConversion";
            mConversionComboBox.Size = new System.Drawing.Size( 368, 21 );
            mConversionComboBox.TabIndex = 4;
            
            mInputLayout.Controls.Remove( mConversion );
            mConversion = mConversionComboBox;
            mInputLayout.Controls.Add( mConversion, 0, 2 );

            // Manually calculate the height of the row as it does not recalculate properly (even it is set to AutoSize)
            // Note: calling suspend/resume/perform layout does not help
            // TODO: make CustomComboBox designerable should fix this
            mInputLayout.RowStyles[ 2 ] = new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Absolute,
                mConversionComboBox.Margin.Top + mConversionComboBox.Height + mConversionComboBox.Margin.Bottom );

            mDefaultTitle = SaveMedia.Program.Title + " " + SaveMedia.Program.TitleVersion;
            this.Text = mDefaultTitle;
            this.StatusMessage = String.Empty;
        }

        // ==================================
        // IMainForm Properties
        // ==================================

        public int ConversionProgress
        {
            set
            {
                if( this.InvokeRequired )
                {
                    this.Invoke( new Action<int>( delegate( int aValue ) { this.ConversionProgress = aValue; } ), value );
                    return;
                }

                this.Text = value.ToString() + "% - " + mDefaultTitle;
                mProgressBar.Value = value;

                this.StatusMessage = "Converting..." + value.ToString() + "%";
            }
        }

        public int DownloadProgress
        {
            set
            {
                this.Text = value + "% - " + mDefaultTitle;
                mProgressBar.Value = value;
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
                }
                
                mDownloadButton.Enabled = value;
            }
        }

        public ConverterTag SelectedConverter
        {
            get { return (ConverterTag)mConversionComboBox.SelectedItem; }
        }

        public String StatusMessage { set { mStatus.Text = value; } }

        public IWin32Window Win32Window
        {
            get{ return this; }
        }

        // ==================================
        // IMainForm Functions
        // ==================================

        public void Initialize( Controller aController, params ConverterTag[] aConverters )
        {
            mController = aController;

            ChangeLayout( Phase_t.eInitialized );

            mConversionComboBox.Initialize( aConverters );
        }

        public void ChangeLayout( Phase_t aPhase )
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
                case Phase_t.eInitialized:
                    this.Text = mDefaultTitle;

                    mInputLayout.Visible = true;
                    mDownloadButton.Visible = true;
                    mOkButton.Visible = false;
                    mCancelButton.Visible = false;
                    this.InputEnabled = true;

                    mMediaInfoLayout.Visible = false;
                    mProgressBar.Visible = false;
                    break;

                case Phase_t.eParsingUrl:
                    mInputLayout.Visible = false;

                    mThumbnail.Visible = false;
                    mTitleLabel.Text = "Loading...";
                    mStatus.Text = String.Empty;
                    mMediaInfoLayout.Visible = true;

                    mProgressBar.Style = ProgressBarStyle.Marquee;
                    mProgressBar.Visible = true;
                    break;

                case Phase_t.eDownloadStarted:
                    this.DownloadProgress = 0;
                    this.StatusMessage = "Downloading...";

                    mProgressBar.Style = ProgressBarStyle.Blocks;
                    mDownloadButton.Visible = false;
                    mCancelButton.Visible = true;
                    break;

                case Phase_t.eDownloadCompleted:
                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    this.StatusMessage = "Download Completed";

                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                case Phase_t.eConvertStarted:
                    this.ConversionProgress = 0;

                    mProgressBar.Style = ProgressBarStyle.Blocks;
                    mDownloadButton.Visible = false;
                    mCancelButton.Visible = true;
                    break;

                case Phase_t.eConvertCompleted:
                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    this.StatusMessage = "Conversion completed";

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                default:
                    break;
            }

            this.ResumeLayout( false );
            this.PerformLayout();
        }

        public void DisplayMediaInfo( DownloadTag aTag )
        {
            if( String.IsNullOrEmpty( aTag.VideoTitle ) )
            {
                return;
            }

            mTitleLabel.Text = aTag.VideoTitle;
            mSizeLabel.Text = "??? MB";

            if( String.IsNullOrEmpty( aTag.Quality ) )
            {
                mQualityLabel.Text = String.Empty;
            }
            else
            {
                mQualityLabel.Text = "Quality: " + aTag.Quality;
            }

            mLocationLabel.Text = String.Empty;

            this.StatusMessage = String.Empty;

            mMediaInfoLayout.Visible = true;
        }

        public String PromptForFolderDestination( String aDescription )
        {
            if( this.InvokeRequired )
            {
                PromptForFolderDestinationCallBack theCallBack = new PromptForFolderDestinationCallBack( PromptForFolderDestination );
                return (String) this.Invoke( theCallBack, new object[] { aDescription } );
            }

            FolderBrowserDialog theDialog = new FolderBrowserDialog();
            theDialog.Description = aDescription;

            DialogResult theDialogResult = theDialog.ShowDialog( this.Win32Window );
            if( theDialogResult == DialogResult.OK )
            {
                return theDialog.SelectedPath;
            }

            return null;
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

        public void ShowError( String aErrorMessage )
        {
            NotifyUser();
            MessageBox.Show( this, aErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            //mToolTip.Show( aErrorMessage, mUrl, 0, mUrl.Height );
        }

        public void ShowThumbnail( String aThumbnailPath )
        {
            mThumbnail.ImageLocation = aThumbnailPath;
            //mThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            //mThumbnail.Refresh();
            mThumbnail.Visible = true;
        }

        // ==================================
        // Delegates
        // ==================================

        delegate void ChangeLayoutCallBack( Phase_t aPhase );
        delegate void NotifyUserCallBack();
        delegate DialogResult PromptForUpdateCallBack();
        delegate String PromptForFolderDestinationCallBack( String aDescription );

        // ==================================
        // Functions
        // ==================================

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

        // ==================================
        // Event Handlers
        // ==================================

        private void mOkButton_Click( object sender, EventArgs e )
        {
            ChangeLayout( Phase_t.eInitialized );
            mController.ClearTemporaryFiles();
        }

        private void mCancelButton_Click( object sender, EventArgs e )
        {
            mController.Abort();
        }

        private void mDownloadButton_Click( object sender, EventArgs e )
        {
            mController.ParseUrl( mUrl.Text );
        }

        private void mUrl_TextChanged( object sender, EventArgs e )
        {
            //Uri theUrl;
            //mDownloadButton.Enabled = Uri.TryCreate( mUrl.Text, UriKind.Absolute, out theUrl );

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

        private void mOptionsButton_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Form theForm = new OptionsForm();
            theForm.ShowDialog( this );
        }

        private void mOptionsButton_MouseEnter( object sender, EventArgs e )
        {
            mOptionsButton.Image = global::SaveMedia.Properties.Resources.settings_hover;
        }

        private void mOptionsButton_MouseLeave( object sender, EventArgs e )
        {
            mOptionsButton.Image = global::SaveMedia.Properties.Resources.settings;
        }

        private void mAboutButton_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Form theForm = new AboutBox();
            theForm.ShowDialog( this );
        }

        private void mAboutButton_MouseEnter( object sender, EventArgs e )
        {
            mAboutButton.Image = global::SaveMedia.Properties.Resources.info_hover;
        }

        private void mAboutButton_MouseLeave( object sender, EventArgs e )
        {
            mAboutButton.Image = global::SaveMedia.Properties.Resources.info;
        }

        private void MainForm_FormClosed( object sender, FormClosedEventArgs e )
        {
            mController.ClearTemporaryFiles();
        }

        private void MainForm_Activated( object sender, EventArgs e )
        {
            String theNewUrl = ClipboardUtils.ReadUrl();
            if( mUrl.Enabled && !mUrl.Text.Equals( theNewUrl ) && !String.IsNullOrEmpty( theNewUrl ) )
            {
                mUrl.Text = theNewUrl;
            }
        }

        // ==================================
        // Members
        // ==================================

        private Controller mController = null;
        private CustomComboBox mConversionComboBox;
        private String mDefaultTitle;
    }
}