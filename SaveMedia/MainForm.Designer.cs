﻿namespace SaveMedia
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.FlowLayoutPanel mMainLayout;
            System.Windows.Forms.TableLayoutPanel theMediaInfoLayout;
            System.Windows.Forms.FlowLayoutPanel theMediaInfoLayout2;
            System.Windows.Forms.TableLayoutPanel theTableLayout;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
            this.mUrlGroupBox = new System.Windows.Forms.GroupBox();
            this.mUrl = new System.Windows.Forms.TextBox();
            this.mConversionGroupBox = new System.Windows.Forms.GroupBox();
            this.mConversion = new System.Windows.Forms.ComboBox();
            this.mMediaInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.mThumbnail = new System.Windows.Forms.PictureBox();
            this.mTitleLabel = new System.Windows.Forms.Label();
            this.mSizeLabel = new System.Windows.Forms.Label();
            this.mLocationLabel = new System.Windows.Forms.Label();
            this.mProgressBar = new System.Windows.Forms.ProgressBar();
            this.mStatus = new System.Windows.Forms.Label();
            this.mDownloadButton = new System.Windows.Forms.Button();
            this.mOkButton = new System.Windows.Forms.Button();
            this.mCancelButton = new System.Windows.Forms.Button();
            this.mMenuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            mMainLayout = new System.Windows.Forms.FlowLayoutPanel();
            theMediaInfoLayout = new System.Windows.Forms.TableLayoutPanel();
            theMediaInfoLayout2 = new System.Windows.Forms.FlowLayoutPanel();
            theTableLayout = new System.Windows.Forms.TableLayoutPanel();
            mMainLayout.SuspendLayout();
            this.mUrlGroupBox.SuspendLayout();
            this.mConversionGroupBox.SuspendLayout();
            this.mMediaInfoGroupBox.SuspendLayout();
            theMediaInfoLayout.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mThumbnail ) ).BeginInit();
            theMediaInfoLayout2.SuspendLayout();
            theTableLayout.SuspendLayout();
            this.mMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMainLayout
            // 
            mMainLayout.AutoSize = true;
            mMainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            mMainLayout.Controls.Add( this.mUrlGroupBox );
            mMainLayout.Controls.Add( this.mConversionGroupBox );
            mMainLayout.Controls.Add( this.mMediaInfoGroupBox );
            mMainLayout.Controls.Add( this.mProgressBar );
            mMainLayout.Controls.Add( theTableLayout );
            mMainLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            mMainLayout.Location = new System.Drawing.Point( 0, 22 );
            mMainLayout.Margin = new System.Windows.Forms.Padding( 0 );
            mMainLayout.Name = "mMainLayout";
            mMainLayout.Padding = new System.Windows.Forms.Padding( 6, 0, 6, 0 );
            mMainLayout.Size = new System.Drawing.Size( 398, 365 );
            mMainLayout.TabIndex = 3;
            mMainLayout.WrapContents = false;
            // 
            // mUrlGroupBox
            // 
            this.mUrlGroupBox.AutoSize = true;
            this.mUrlGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mUrlGroupBox.Controls.Add( this.mUrl );
            this.mUrlGroupBox.Location = new System.Drawing.Point( 9, 3 );
            this.mUrlGroupBox.Name = "mUrlGroupBox";
            this.mUrlGroupBox.Size = new System.Drawing.Size( 380, 58 );
            this.mUrlGroupBox.TabIndex = 0;
            this.mUrlGroupBox.TabStop = false;
            this.mUrlGroupBox.Text = "URL:";
            // 
            // mUrl
            // 
            this.mUrl.Location = new System.Drawing.Point( 6, 19 );
            this.mUrl.Name = "mUrl";
            this.mUrl.Size = new System.Drawing.Size( 368, 20 );
            this.mUrl.TabIndex = 1;
            this.mUrl.TextChanged += new System.EventHandler( this.mUrl_TextChanged );
            // 
            // mConversionGroupBox
            // 
            this.mConversionGroupBox.AutoSize = true;
            this.mConversionGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mConversionGroupBox.Controls.Add( this.mConversion );
            this.mConversionGroupBox.Location = new System.Drawing.Point( 9, 67 );
            this.mConversionGroupBox.Name = "mConversionGroupBox";
            this.mConversionGroupBox.Size = new System.Drawing.Size( 380, 59 );
            this.mConversionGroupBox.TabIndex = 1;
            this.mConversionGroupBox.TabStop = false;
            this.mConversionGroupBox.Text = "Conversion:";
            // 
            // mConversion
            // 
            this.mConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mConversion.FormattingEnabled = true;
            this.mConversion.ItemHeight = 13;
            this.mConversion.Items.AddRange( new object[] {
            "Do not convert file",
            "MPEG-1 Audio Layer 3 (*.mp3)",
            "Windows Media Video (*.wmv)"} );
            this.mConversion.Location = new System.Drawing.Point( 6, 19 );
            this.mConversion.Name = "mConversion";
            this.mConversion.Size = new System.Drawing.Size( 368, 21 );
            this.mConversion.TabIndex = 2;
            // 
            // mMediaInfoGroupBox
            // 
            this.mMediaInfoGroupBox.AutoSize = true;
            this.mMediaInfoGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mMediaInfoGroupBox.Controls.Add( theMediaInfoLayout );
            this.mMediaInfoGroupBox.Location = new System.Drawing.Point( 9, 132 );
            this.mMediaInfoGroupBox.MinimumSize = new System.Drawing.Size( 380, 115 );
            this.mMediaInfoGroupBox.Name = "mMediaInfoGroupBox";
            this.mMediaInfoGroupBox.Size = new System.Drawing.Size( 380, 134 );
            this.mMediaInfoGroupBox.TabIndex = 4;
            this.mMediaInfoGroupBox.TabStop = false;
            this.mMediaInfoGroupBox.Text = "Media Info";
            this.mMediaInfoGroupBox.Visible = false;
            // 
            // theMediaInfoLayout
            // 
            theMediaInfoLayout.AutoSize = true;
            theMediaInfoLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theMediaInfoLayout.ColumnCount = 2;
            theMediaInfoLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle() );
            theMediaInfoLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            theMediaInfoLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 20F ) );
            theMediaInfoLayout.Controls.Add( this.mThumbnail, 0, 0 );
            theMediaInfoLayout.Controls.Add( theMediaInfoLayout2, 1, 0 );
            theMediaInfoLayout.Location = new System.Drawing.Point( 6, 19 );
            theMediaInfoLayout.MaximumSize = new System.Drawing.Size( 550, 0 );
            theMediaInfoLayout.Name = "theMediaInfoLayout";
            theMediaInfoLayout.RowCount = 1;
            theMediaInfoLayout.RowStyles.Add( new System.Windows.Forms.RowStyle() );
            theMediaInfoLayout.Size = new System.Drawing.Size( 189, 96 );
            theMediaInfoLayout.TabIndex = 0;
            // 
            // mThumbnail
            // 
            this.mThumbnail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mThumbnail.Location = new System.Drawing.Point( 3, 3 );
            this.mThumbnail.MaximumSize = new System.Drawing.Size( 400, 300 );
            this.mThumbnail.Name = "mThumbnail";
            this.mThumbnail.Size = new System.Drawing.Size( 120, 90 );
            this.mThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mThumbnail.TabIndex = 0;
            this.mThumbnail.TabStop = false;
            // 
            // theMediaInfoLayout2
            // 
            theMediaInfoLayout2.Anchor = System.Windows.Forms.AnchorStyles.None;
            theMediaInfoLayout2.AutoSize = true;
            theMediaInfoLayout2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theMediaInfoLayout2.Controls.Add( this.mTitleLabel );
            theMediaInfoLayout2.Controls.Add( this.mSizeLabel );
            theMediaInfoLayout2.Controls.Add( this.mLocationLabel );
            theMediaInfoLayout2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            theMediaInfoLayout2.Location = new System.Drawing.Point( 129, 19 );
            theMediaInfoLayout2.Name = "theMediaInfoLayout2";
            theMediaInfoLayout2.Size = new System.Drawing.Size( 57, 57 );
            theMediaInfoLayout2.TabIndex = 1;
            theMediaInfoLayout2.WrapContents = false;
            // 
            // mTitleLabel
            // 
            this.mTitleLabel.AutoSize = true;
            this.mTitleLabel.Location = new System.Drawing.Point( 3, 3 );
            this.mTitleLabel.Margin = new System.Windows.Forms.Padding( 3 );
            this.mTitleLabel.Name = "mTitleLabel";
            this.mTitleLabel.Size = new System.Drawing.Size( 33, 13 );
            this.mTitleLabel.TabIndex = 0;
            this.mTitleLabel.Text = "Title: ";
            // 
            // mSizeLabel
            // 
            this.mSizeLabel.AutoSize = true;
            this.mSizeLabel.Location = new System.Drawing.Point( 3, 22 );
            this.mSizeLabel.Margin = new System.Windows.Forms.Padding( 3 );
            this.mSizeLabel.Name = "mSizeLabel";
            this.mSizeLabel.Size = new System.Drawing.Size( 30, 13 );
            this.mSizeLabel.TabIndex = 1;
            this.mSizeLabel.Text = "Size:";
            // 
            // mLocationLabel
            // 
            this.mLocationLabel.AutoSize = true;
            this.mLocationLabel.Location = new System.Drawing.Point( 3, 44 );
            this.mLocationLabel.Margin = new System.Windows.Forms.Padding( 3, 6, 3, 0 );
            this.mLocationLabel.Name = "mLocationLabel";
            this.mLocationLabel.Size = new System.Drawing.Size( 51, 13 );
            this.mLocationLabel.TabIndex = 2;
            this.mLocationLabel.Text = "Location:";
            // 
            // mProgressBar
            // 
            this.mProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.mProgressBar.Location = new System.Drawing.Point( 9, 278 );
            this.mProgressBar.Margin = new System.Windows.Forms.Padding( 3, 9, 3, 3 );
            this.mProgressBar.Name = "mProgressBar";
            this.mProgressBar.Size = new System.Drawing.Size( 380, 30 );
            this.mProgressBar.TabIndex = 5;
            // 
            // theTableLayout
            // 
            theTableLayout.AutoSize = true;
            theTableLayout.ColumnCount = 4;
            theTableLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            theTableLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle() );
            theTableLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle() );
            theTableLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle() );
            theTableLayout.Controls.Add( this.mStatus, 0, 0 );
            theTableLayout.Controls.Add( this.mDownloadButton, 1, 0 );
            theTableLayout.Controls.Add( this.mOkButton, 2, 0 );
            theTableLayout.Controls.Add( this.mCancelButton, 3, 0 );
            theTableLayout.Dock = System.Windows.Forms.DockStyle.Top;
            theTableLayout.Location = new System.Drawing.Point( 6, 320 );
            theTableLayout.Margin = new System.Windows.Forms.Padding( 0, 9, 0, 9 );
            theTableLayout.Name = "theTableLayout";
            theTableLayout.RowCount = 1;
            theTableLayout.RowStyles.Add( new System.Windows.Forms.RowStyle() );
            theTableLayout.Size = new System.Drawing.Size( 386, 36 );
            theTableLayout.TabIndex = 6;
            // 
            // mStatus
            // 
            this.mStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mStatus.AutoSize = true;
            this.mStatus.Location = new System.Drawing.Point( 3, 11 );
            this.mStatus.Name = "mStatus";
            this.mStatus.Size = new System.Drawing.Size( 37, 13 );
            this.mStatus.TabIndex = 3;
            this.mStatus.Text = "Status";
            // 
            // mDownloadButton
            // 
            this.mDownloadButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.mDownloadButton.AutoSize = true;
            this.mDownloadButton.Enabled = false;
            this.mDownloadButton.Location = new System.Drawing.Point( 148, 3 );
            this.mDownloadButton.Margin = new System.Windows.Forms.Padding( 3, 3, 2, 3 );
            this.mDownloadButton.Name = "mDownloadButton";
            this.mDownloadButton.Size = new System.Drawing.Size( 75, 30 );
            this.mDownloadButton.TabIndex = 2;
            this.mDownloadButton.Text = "Download";
            this.mDownloadButton.UseVisualStyleBackColor = true;
            this.mDownloadButton.Click += new System.EventHandler( this.mDownloadButton_Click );
            // 
            // mOkButton
            // 
            this.mOkButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.mOkButton.AutoSize = true;
            this.mOkButton.Location = new System.Drawing.Point( 228, 3 );
            this.mOkButton.Margin = new System.Windows.Forms.Padding( 3, 3, 2, 3 );
            this.mOkButton.Name = "mOkButton";
            this.mOkButton.Size = new System.Drawing.Size( 75, 30 );
            this.mOkButton.TabIndex = 4;
            this.mOkButton.Text = "OK";
            this.mOkButton.UseVisualStyleBackColor = true;
            this.mOkButton.Visible = false;
            this.mOkButton.Click += new System.EventHandler( this.mOkButton_Click );
            // 
            // mCancelButton
            // 
            this.mCancelButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.mCancelButton.AutoSize = true;
            this.mCancelButton.Location = new System.Drawing.Point( 308, 3 );
            this.mCancelButton.Name = "mCancelButton";
            this.mCancelButton.Size = new System.Drawing.Size( 75, 30 );
            this.mCancelButton.TabIndex = 5;
            this.mCancelButton.Text = "Cancel";
            this.mCancelButton.UseVisualStyleBackColor = true;
            this.mCancelButton.Visible = false;
            this.mCancelButton.Click += new System.EventHandler( this.mCancelButton_Click );
            // 
            // mMenuStrip
            // 
            this.mMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.mAboutToolStripMenuItem} );
            this.mMenuStrip.Location = new System.Drawing.Point( 0, 0 );
            this.mMenuStrip.MaximumSize = new System.Drawing.Size( 0, 22 );
            this.mMenuStrip.Name = "mMenuStrip";
            this.mMenuStrip.Padding = new System.Windows.Forms.Padding( 0, 0, 0, 2 );
            this.mMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mMenuStrip.Size = new System.Drawing.Size( 542, 22 );
            this.mMenuStrip.TabIndex = 4;
            this.mMenuStrip.Text = "MenuStrip";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size( 56, 20 );
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler( this.optionsToolStripMenuItem_Click );
            // 
            // mAboutToolStripMenuItem
            // 
            this.mAboutToolStripMenuItem.Name = "mAboutToolStripMenuItem";
            this.mAboutToolStripMenuItem.Size = new System.Drawing.Size( 48, 20 );
            this.mAboutToolStripMenuItem.Text = "&About";
            this.mAboutToolStripMenuItem.Click += new System.EventHandler( this.mAboutToolStripMenuItem_Click );
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size( 542, 399 );
            this.Controls.Add( mMainLayout );
            this.Controls.Add( this.mMenuStrip );
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.MainMenuStrip = this.mMenuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SaveMedia";
            this.Activated += new System.EventHandler( this.MainForm_Activated );
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.MainForm_FormClosed );
            mMainLayout.ResumeLayout( false );
            mMainLayout.PerformLayout();
            this.mUrlGroupBox.ResumeLayout( false );
            this.mUrlGroupBox.PerformLayout();
            this.mConversionGroupBox.ResumeLayout( false );
            this.mMediaInfoGroupBox.ResumeLayout( false );
            this.mMediaInfoGroupBox.PerformLayout();
            theMediaInfoLayout.ResumeLayout( false );
            theMediaInfoLayout.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mThumbnail ) ).EndInit();
            theMediaInfoLayout2.ResumeLayout( false );
            theMediaInfoLayout2.PerformLayout();
            theTableLayout.ResumeLayout( false );
            theTableLayout.PerformLayout();
            this.mMenuStrip.ResumeLayout( false );
            this.mMenuStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion       
        
        private System.Windows.Forms.GroupBox mUrlGroupBox;
        private System.Windows.Forms.GroupBox mConversionGroupBox;
        private System.Windows.Forms.ComboBox mConversion;
        private System.Windows.Forms.Button mDownloadButton;
        private System.Windows.Forms.GroupBox mMediaInfoGroupBox;
        private System.Windows.Forms.MenuStrip mMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mAboutToolStripMenuItem;
        private System.Windows.Forms.ProgressBar mProgressBar;
        private System.Windows.Forms.TextBox mUrl;
        private System.Windows.Forms.Label mStatus;
        private System.Windows.Forms.PictureBox mThumbnail;
        private System.Windows.Forms.Label mTitleLabel;
        private System.Windows.Forms.Label mSizeLabel;
        private System.Windows.Forms.Button mOkButton;
        private System.Windows.Forms.Button mCancelButton;
        private System.Windows.Forms.Label mLocationLabel;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    }
}
