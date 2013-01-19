namespace SaveMedia
{
    partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.LinkLabel theFFmpegLink;
            System.Windows.Forms.TableLayoutPanel theMainLayout;
            System.Windows.Forms.TableLayoutPanel theBottomLayout;
            System.Windows.Forms.TableLayoutPanel theTopLayout;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.mDate = new System.Windows.Forms.Label();
            this.mCopyright = new System.Windows.Forms.LinkLabel();
            this.theLogo = new System.Windows.Forms.PictureBox();
            this.mTitle = new System.Windows.Forms.LinkLabel();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            theFFmpegLink = new System.Windows.Forms.LinkLabel();
            theMainLayout = new System.Windows.Forms.TableLayoutPanel();
            theBottomLayout = new System.Windows.Forms.TableLayoutPanel();
            theTopLayout = new System.Windows.Forms.TableLayoutPanel();
            theMainLayout.SuspendLayout();
            theBottomLayout.SuspendLayout();
            theTopLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.theLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // theFFmpegLink
            // 
            theFFmpegLink.AutoSize = true;
            theFFmpegLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            theFFmpegLink.LinkColor = System.Drawing.SystemColors.ControlText;
            theFFmpegLink.Location = new System.Drawing.Point(3, 15);
            theFFmpegLink.Name = "theFFmpegLink";
            theFFmpegLink.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            theFFmpegLink.Size = new System.Drawing.Size(153, 18);
            theFFmpegLink.TabIndex = 1;
            theFFmpegLink.TabStop = true;
            theFFmpegLink.Text = "FFmpeg 32-bit (2013-01-15)";
            this.mToolTip.SetToolTip(theFFmpegLink, "http://ffmpeg.zeranoe.com/builds/");
            theFFmpegLink.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            theFFmpegLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.theFFmpegLink_LinkClicked);
            // 
            // theMainLayout
            // 
            theMainLayout.AutoSize = true;
            theMainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theMainLayout.ColumnCount = 1;
            theMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theMainLayout.Controls.Add(theBottomLayout, 0, 2);
            theMainLayout.Controls.Add(theTopLayout, 0, 0);
            theMainLayout.Location = new System.Drawing.Point(0, 0);
            theMainLayout.Margin = new System.Windows.Forms.Padding(0);
            theMainLayout.Name = "theMainLayout";
            theMainLayout.Padding = new System.Windows.Forms.Padding(32, 12, 32, 32);
            theMainLayout.RowCount = 3;
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            theMainLayout.Size = new System.Drawing.Size(223, 146);
            theMainLayout.TabIndex = 0;
            // 
            // theBottomLayout
            // 
            theBottomLayout.Anchor = System.Windows.Forms.AnchorStyles.Top;
            theBottomLayout.AutoSize = true;
            theBottomLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theBottomLayout.ColumnCount = 1;
            theBottomLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theBottomLayout.Controls.Add(this.mDate, 0, 0);
            theBottomLayout.Controls.Add(theFFmpegLink, 0, 1);
            theBottomLayout.Location = new System.Drawing.Point(32, 81);
            theBottomLayout.Margin = new System.Windows.Forms.Padding(0);
            theBottomLayout.Name = "theBottomLayout";
            theBottomLayout.RowCount = 2;
            theBottomLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theBottomLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theBottomLayout.Size = new System.Drawing.Size(159, 33);
            theBottomLayout.TabIndex = 1;
            // 
            // mDate
            // 
            this.mDate.AutoSize = true;
            this.mDate.Location = new System.Drawing.Point(3, 0);
            this.mDate.Name = "mDate";
            this.mDate.Size = new System.Drawing.Size(42, 15);
            this.mDate.TabIndex = 0;
            this.mDate.Text = "mDate";
            // 
            // theTopLayout
            // 
            theTopLayout.AutoSize = true;
            theTopLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theTopLayout.ColumnCount = 3;
            theTopLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theTopLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            theTopLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theTopLayout.Controls.Add(this.mCopyright, 2, 2);
            theTopLayout.Controls.Add(this.theLogo, 0, 0);
            theTopLayout.Controls.Add(this.mTitle, 2, 1);
            theTopLayout.Location = new System.Drawing.Point(35, 15);
            theTopLayout.Name = "theTopLayout";
            theTopLayout.RowCount = 3;
            theTopLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            theTopLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theTopLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theTopLayout.Size = new System.Drawing.Size(137, 54);
            theTopLayout.TabIndex = 0;
            // 
            // mCopyright
            // 
            this.mCopyright.AutoSize = true;
            this.mCopyright.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.mCopyright.LinkColor = System.Drawing.SystemColors.ControlText;
            this.mCopyright.Location = new System.Drawing.Point(63, 27);
            this.mCopyright.Name = "mCopyright";
            this.mCopyright.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.mCopyright.Size = new System.Drawing.Size(71, 18);
            this.mCopyright.TabIndex = 1;
            this.mCopyright.TabStop = true;
            this.mCopyright.Text = "mCopyright";
            this.mToolTip.SetToolTip(this.mCopyright, "savemedia@notfaqs.com");
            this.mCopyright.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            this.mCopyright.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mCopyright_LinkClicked);
            // 
            // theLogo
            // 
            this.theLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.theLogo.Image = global::SaveMedia.Properties.Resources.SaveMedia;
            this.theLogo.Location = new System.Drawing.Point(3, 3);
            this.theLogo.Name = "theLogo";
            theTopLayout.SetRowSpan(this.theLogo, 3);
            this.theLogo.Size = new System.Drawing.Size(48, 48);
            this.theLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.theLogo.TabIndex = 1;
            this.theLogo.TabStop = false;
            this.mToolTip.SetToolTip(this.theLogo, "http://savemedia.googlecode.com/");
            this.theLogo.Click += new System.EventHandler(this.theLogo_Click);
            // 
            // mTitle
            // 
            this.mTitle.AutoSize = true;
            this.mTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.mTitle.LinkColor = System.Drawing.SystemColors.ControlText;
            this.mTitle.Location = new System.Drawing.Point(63, 6);
            this.mTitle.Name = "mTitle";
            this.mTitle.Size = new System.Drawing.Size(53, 21);
            this.mTitle.TabIndex = 0;
            this.mTitle.TabStop = true;
            this.mTitle.Text = "mTitle";
            this.mToolTip.SetToolTip(this.mTitle, "http://savemedia.googlecode.com/");
            this.mTitle.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            this.mTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mTitle_LinkClicked);
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(207, 146);
            this.Controls.Add(theMainLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SaveMedia";
            theMainLayout.ResumeLayout(false);
            theMainLayout.PerformLayout();
            theBottomLayout.ResumeLayout(false);
            theBottomLayout.PerformLayout();
            theTopLayout.ResumeLayout(false);
            theTopLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.theLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mDate;
        private System.Windows.Forms.PictureBox theLogo;
        private System.Windows.Forms.ToolTip mToolTip;
        private System.Windows.Forms.LinkLabel mCopyright;
        private System.Windows.Forms.LinkLabel mTitle;
    }
}