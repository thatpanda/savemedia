namespace SaveMedia
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TableLayoutPanel theButtonLayout;
            System.Windows.Forms.TableLayoutPanel theInnerLayout;
            System.Windows.Forms.TableLayoutPanel theStatsLayout;
            System.Windows.Forms.TableLayoutPanel theMainLayout;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mDownloadButton = new System.Windows.Forms.Button();
            this.mOkButton = new System.Windows.Forms.Button();
            this.mCancelButton = new System.Windows.Forms.Button();
            this.mAboutButton = new System.Windows.Forms.PictureBox();
            this.mOptionsButton = new System.Windows.Forms.PictureBox();
            this.mMediaInfoLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mStatus = new System.Windows.Forms.Label();
            this.mLocationLabel = new System.Windows.Forms.Label();
            this.mTitleLabel = new System.Windows.Forms.Label();
            this.mQualityLabel = new System.Windows.Forms.Label();
            this.mSizeLabel = new System.Windows.Forms.Label();
            this.mThumbnail = new System.Windows.Forms.PictureBox();
            this.mInputLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mConversion = new System.Windows.Forms.ComboBox();
            this.mUrl = new System.Windows.Forms.TextBox();
            this.mProgressBar = new System.Windows.Forms.ProgressBar();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            theButtonLayout = new System.Windows.Forms.TableLayoutPanel();
            theInnerLayout = new System.Windows.Forms.TableLayoutPanel();
            theStatsLayout = new System.Windows.Forms.TableLayoutPanel();
            theMainLayout = new System.Windows.Forms.TableLayoutPanel();
            theButtonLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAboutButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mOptionsButton)).BeginInit();
            theInnerLayout.SuspendLayout();
            this.mMediaInfoLayout.SuspendLayout();
            theStatsLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mThumbnail)).BeginInit();
            this.mInputLayout.SuspendLayout();
            theMainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // theButtonLayout
            // 
            theButtonLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            theButtonLayout.AutoSize = true;
            theButtonLayout.BackColor = System.Drawing.SystemColors.Control;
            theButtonLayout.ColumnCount = 6;
            theButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            theButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theButtonLayout.Controls.Add(this.mDownloadButton, 3, 0);
            theButtonLayout.Controls.Add(this.mOkButton, 4, 0);
            theButtonLayout.Controls.Add(this.mCancelButton, 5, 0);
            theButtonLayout.Controls.Add(this.mAboutButton, 1, 0);
            theButtonLayout.Controls.Add(this.mOptionsButton, 0, 0);
            theButtonLayout.Location = new System.Drawing.Point(0, 233);
            theButtonLayout.Margin = new System.Windows.Forms.Padding(0);
            theButtonLayout.Name = "theButtonLayout";
            theButtonLayout.Padding = new System.Windows.Forms.Padding(22, 7, 22, 7);
            theButtonLayout.RowCount = 1;
            theButtonLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theButtonLayout.Size = new System.Drawing.Size(506, 48);
            theButtonLayout.TabIndex = 0;
            // 
            // mDownloadButton
            // 
            this.mDownloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mDownloadButton.AutoSize = true;
            this.mDownloadButton.Location = new System.Drawing.Point(231, 10);
            this.mDownloadButton.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this.mDownloadButton.Name = "mDownloadButton";
            this.mDownloadButton.Size = new System.Drawing.Size(80, 28);
            this.mDownloadButton.TabIndex = 0;
            this.mDownloadButton.Text = "Download";
            this.mDownloadButton.UseVisualStyleBackColor = true;
            this.mDownloadButton.Click += new System.EventHandler(this.mDownloadButton_Click);
            // 
            // mOkButton
            // 
            this.mOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mOkButton.AutoSize = true;
            this.mOkButton.Location = new System.Drawing.Point(316, 10);
            this.mOkButton.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this.mOkButton.Name = "mOkButton";
            this.mOkButton.Size = new System.Drawing.Size(80, 28);
            this.mOkButton.TabIndex = 1;
            this.mOkButton.Text = "OK";
            this.mOkButton.UseVisualStyleBackColor = true;
            this.mOkButton.Visible = false;
            this.mOkButton.Click += new System.EventHandler(this.mOkButton_Click);
            // 
            // mCancelButton
            // 
            this.mCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mCancelButton.AutoSize = true;
            this.mCancelButton.Location = new System.Drawing.Point(401, 10);
            this.mCancelButton.Name = "mCancelButton";
            this.mCancelButton.Size = new System.Drawing.Size(80, 28);
            this.mCancelButton.TabIndex = 2;
            this.mCancelButton.Text = "Cancel";
            this.mCancelButton.UseVisualStyleBackColor = true;
            this.mCancelButton.Visible = false;
            this.mCancelButton.Click += new System.EventHandler(this.mCancelButton_Click);
            // 
            // mAboutButton
            // 
            this.mAboutButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mAboutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mAboutButton.Image = global::SaveMedia.Properties.Resources.info;
            this.mAboutButton.Location = new System.Drawing.Point(52, 13);
            this.mAboutButton.Name = "mAboutButton";
            this.mAboutButton.Size = new System.Drawing.Size(21, 21);
            this.mAboutButton.TabIndex = 3;
            this.mAboutButton.TabStop = false;
            this.mToolTip.SetToolTip(this.mAboutButton, "About");
            this.mAboutButton.Click += new System.EventHandler(this.mAboutButton_Click);
            this.mAboutButton.MouseEnter += new System.EventHandler(this.mAboutButton_MouseEnter);
            this.mAboutButton.MouseLeave += new System.EventHandler(this.mAboutButton_MouseLeave);
            // 
            // mOptionsButton
            // 
            this.mOptionsButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mOptionsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mOptionsButton.Image = global::SaveMedia.Properties.Resources.settings;
            this.mOptionsButton.Location = new System.Drawing.Point(25, 13);
            this.mOptionsButton.Name = "mOptionsButton";
            this.mOptionsButton.Size = new System.Drawing.Size(21, 21);
            this.mOptionsButton.TabIndex = 4;
            this.mOptionsButton.TabStop = false;
            this.mToolTip.SetToolTip(this.mOptionsButton, "Options");
            this.mOptionsButton.Click += new System.EventHandler(this.mOptionsButton_Click);
            this.mOptionsButton.MouseEnter += new System.EventHandler(this.mOptionsButton_MouseEnter);
            this.mOptionsButton.MouseLeave += new System.EventHandler(this.mOptionsButton_MouseLeave);
            // 
            // theInnerLayout
            // 
            theInnerLayout.AutoSize = true;
            theInnerLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theInnerLayout.ColumnCount = 1;
            theInnerLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theInnerLayout.Controls.Add(this.mMediaInfoLayout, 0, 1);
            theInnerLayout.Controls.Add(this.mInputLayout, 0, 0);
            theInnerLayout.Controls.Add(this.mProgressBar, 0, 2);
            theInnerLayout.Location = new System.Drawing.Point(0, 0);
            theInnerLayout.Margin = new System.Windows.Forms.Padding(0);
            theInnerLayout.MinimumSize = new System.Drawing.Size(0, 106);
            theInnerLayout.Name = "theInnerLayout";
            theInnerLayout.Padding = new System.Windows.Forms.Padding(22, 10, 22, 10);
            theInnerLayout.RowCount = 3;
            theInnerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theInnerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theInnerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theInnerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            theInnerLayout.Size = new System.Drawing.Size(506, 233);
            theInnerLayout.TabIndex = 1;
            // 
            // mMediaInfoLayout
            // 
            this.mMediaInfoLayout.AutoSize = true;
            this.mMediaInfoLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mMediaInfoLayout.ColumnCount = 2;
            this.mMediaInfoLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mMediaInfoLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mMediaInfoLayout.Controls.Add(theStatsLayout, 1, 0);
            this.mMediaInfoLayout.Controls.Add(this.mThumbnail, 0, 0);
            this.mMediaInfoLayout.Location = new System.Drawing.Point(22, 74);
            this.mMediaInfoLayout.Margin = new System.Windows.Forms.Padding(0);
            this.mMediaInfoLayout.Name = "mMediaInfoLayout";
            this.mMediaInfoLayout.RowCount = 1;
            this.mMediaInfoLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mMediaInfoLayout.Size = new System.Drawing.Size(134, 118);
            this.mMediaInfoLayout.TabIndex = 1;
            // 
            // theStatsLayout
            // 
            theStatsLayout.AutoSize = true;
            theStatsLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theStatsLayout.ColumnCount = 1;
            theStatsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theStatsLayout.Controls.Add(this.mStatus, 0, 1);
            theStatsLayout.Controls.Add(this.mLocationLabel, 0, 4);
            theStatsLayout.Controls.Add(this.mTitleLabel, 0, 0);
            theStatsLayout.Controls.Add(this.mQualityLabel, 0, 3);
            theStatsLayout.Controls.Add(this.mSizeLabel, 0, 2);
            theStatsLayout.Location = new System.Drawing.Point(69, 3);
            theStatsLayout.Name = "theStatsLayout";
            theStatsLayout.RowCount = 5;
            theStatsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theStatsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theStatsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theStatsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theStatsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theStatsLayout.Size = new System.Drawing.Size(62, 112);
            theStatsLayout.TabIndex = 5;
            // 
            // mStatus
            // 
            this.mStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mStatus.AutoSize = true;
            this.mStatus.Location = new System.Drawing.Point(3, 29);
            this.mStatus.Margin = new System.Windows.Forms.Padding(3);
            this.mStatus.Name = "mStatus";
            this.mStatus.Size = new System.Drawing.Size(39, 15);
            this.mStatus.TabIndex = 2;
            this.mStatus.Text = "Status";
            // 
            // mLocationLabel
            // 
            this.mLocationLabel.AutoSize = true;
            this.mLocationLabel.Location = new System.Drawing.Point(3, 97);
            this.mLocationLabel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.mLocationLabel.Name = "mLocationLabel";
            this.mLocationLabel.Size = new System.Drawing.Size(56, 15);
            this.mLocationLabel.TabIndex = 2;
            this.mLocationLabel.Text = "Location:";
            this.mLocationLabel.Visible = false;
            // 
            // mTitleLabel
            // 
            this.mTitleLabel.AutoEllipsis = true;
            this.mTitleLabel.AutoSize = true;
            this.mTitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTitleLabel.Location = new System.Drawing.Point(0, 3);
            this.mTitleLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.mTitleLabel.MaximumSize = new System.Drawing.Size(400, 21);
            this.mTitleLabel.Name = "mTitleLabel";
            this.mTitleLabel.Size = new System.Drawing.Size(38, 20);
            this.mTitleLabel.TabIndex = 0;
            this.mTitleLabel.Text = "Title";
            this.mTitleLabel.UseMnemonic = false;
            // 
            // mQualityLabel
            // 
            this.mQualityLabel.AutoSize = true;
            this.mQualityLabel.Location = new System.Drawing.Point(3, 75);
            this.mQualityLabel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.mQualityLabel.Name = "mQualityLabel";
            this.mQualityLabel.Size = new System.Drawing.Size(48, 15);
            this.mQualityLabel.TabIndex = 3;
            this.mQualityLabel.Text = "Quality:";
            this.mQualityLabel.Visible = false;
            // 
            // mSizeLabel
            // 
            this.mSizeLabel.AutoSize = true;
            this.mSizeLabel.Location = new System.Drawing.Point(3, 50);
            this.mSizeLabel.Margin = new System.Windows.Forms.Padding(3);
            this.mSizeLabel.Name = "mSizeLabel";
            this.mSizeLabel.Size = new System.Drawing.Size(27, 15);
            this.mSizeLabel.TabIndex = 1;
            this.mSizeLabel.Text = "Size";
            this.mSizeLabel.Visible = false;
            // 
            // mThumbnail
            // 
            this.mThumbnail.Location = new System.Drawing.Point(3, 3);
            this.mThumbnail.MaximumSize = new System.Drawing.Size(467, 346);
            this.mThumbnail.Name = "mThumbnail";
            this.mThumbnail.Size = new System.Drawing.Size(60, 49);
            this.mThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mThumbnail.TabIndex = 0;
            this.mThumbnail.TabStop = false;
            // 
            // mInputLayout
            // 
            this.mInputLayout.AutoSize = true;
            this.mInputLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mInputLayout.ColumnCount = 1;
            this.mInputLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mInputLayout.Controls.Add(this.mConversion, 0, 2);
            this.mInputLayout.Controls.Add(this.mUrl, 0, 0);
            this.mInputLayout.Location = new System.Drawing.Point(22, 10);
            this.mInputLayout.Margin = new System.Windows.Forms.Padding(0);
            this.mInputLayout.Name = "mInputLayout";
            this.mInputLayout.RowCount = 3;
            this.mInputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mInputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.mInputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mInputLayout.Size = new System.Drawing.Size(461, 64);
            this.mInputLayout.TabIndex = 0;
            // 
            // mConversion
            // 
            this.mConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mConversion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mConversion.FormattingEnabled = true;
            this.mConversion.ItemHeight = 15;
            this.mConversion.Items.AddRange(new object[] {
            "Do not convert file",
            "MPEG-1 Audio Layer 3 (*.mp3)",
            "Windows Media Video (*.wmv)"});
            this.mConversion.Location = new System.Drawing.Point(3, 38);
            this.mConversion.Name = "mConversion";
            this.mConversion.Size = new System.Drawing.Size(455, 23);
            this.mConversion.TabIndex = 2;
            // 
            // mUrl
            // 
            this.mUrl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mUrl.Location = new System.Drawing.Point(3, 3);
            this.mUrl.Name = "mUrl";
            this.mUrl.Size = new System.Drawing.Size(455, 23);
            this.mUrl.TabIndex = 0;
            this.mUrl.TextChanged += new System.EventHandler(this.mUrl_TextChanged);
            // 
            // mProgressBar
            // 
            this.mProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mProgressBar.Location = new System.Drawing.Point(26, 202);
            this.mProgressBar.Margin = new System.Windows.Forms.Padding(4, 10, 4, 3);
            this.mProgressBar.MarqueeAnimationSpeed = 25;
            this.mProgressBar.Name = "mProgressBar";
            this.mProgressBar.Size = new System.Drawing.Size(454, 18);
            this.mProgressBar.TabIndex = 3;
            // 
            // theMainLayout
            // 
            theMainLayout.AutoSize = true;
            theMainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theMainLayout.ColumnCount = 1;
            theMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theMainLayout.Controls.Add(theButtonLayout, 0, 1);
            theMainLayout.Controls.Add(theInnerLayout, 0, 0);
            theMainLayout.Location = new System.Drawing.Point(0, 0);
            theMainLayout.Margin = new System.Windows.Forms.Padding(0);
            theMainLayout.Name = "theMainLayout";
            theMainLayout.RowCount = 2;
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theMainLayout.Size = new System.Drawing.Size(506, 281);
            theMainLayout.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AcceptButton = this.mDownloadButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(506, 281);
            this.Controls.Add(theMainLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SaveMedia";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            theButtonLayout.ResumeLayout(false);
            theButtonLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAboutButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mOptionsButton)).EndInit();
            theInnerLayout.ResumeLayout(false);
            theInnerLayout.PerformLayout();
            this.mMediaInfoLayout.ResumeLayout(false);
            this.mMediaInfoLayout.PerformLayout();
            theStatsLayout.ResumeLayout(false);
            theStatsLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mThumbnail)).EndInit();
            this.mInputLayout.ResumeLayout(false);
            this.mInputLayout.PerformLayout();
            theMainLayout.ResumeLayout(false);
            theMainLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion       

        private System.Windows.Forms.ComboBox mConversion;
        private System.Windows.Forms.Button mDownloadButton;
        private System.Windows.Forms.ProgressBar mProgressBar;
        private System.Windows.Forms.TextBox mUrl;
        private System.Windows.Forms.Label mStatus;
        private System.Windows.Forms.PictureBox mThumbnail;
        private System.Windows.Forms.Label mTitleLabel;
        private System.Windows.Forms.Label mSizeLabel;
        private System.Windows.Forms.Button mOkButton;
        private System.Windows.Forms.Button mCancelButton;
        private System.Windows.Forms.Label mLocationLabel;
        private System.Windows.Forms.Label mQualityLabel;
        private System.Windows.Forms.TableLayoutPanel mInputLayout;
        private System.Windows.Forms.TableLayoutPanel mMediaInfoLayout;
        private System.Windows.Forms.ToolTip mToolTip;
        private System.Windows.Forms.PictureBox mAboutButton;
        private System.Windows.Forms.PictureBox mOptionsButton;
    }
}

