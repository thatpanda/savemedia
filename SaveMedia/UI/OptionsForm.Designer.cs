namespace SaveMedia
{
    partial class OptionsForm
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
            System.Windows.Forms.GroupBox theYouTubeGroup;
            System.Windows.Forms.TableLayoutPanel theYoutubeQualityLayout;
            System.Windows.Forms.PictureBox theYouTubeQualityIcon;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.GroupBox theAutoUpdateGroup;
            System.Windows.Forms.PictureBox theCheckForUpdatesIcon;
            System.Windows.Forms.TableLayoutPanel theMainLayout;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.mFmt37 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.mFmt22 = new System.Windows.Forms.RadioButton();
            this.mNormalQuality = new System.Windows.Forms.RadioButton();
            this.mFmt35 = new System.Windows.Forms.RadioButton();
            this.mFmt18 = new System.Windows.Forms.RadioButton();
            this.mNeverUpdate = new System.Windows.Forms.RadioButton();
            this.mAutoUpdate = new System.Windows.Forms.RadioButton();
            this.mUpdateWhenFails = new System.Windows.Forms.RadioButton();
            theYouTubeGroup = new System.Windows.Forms.GroupBox();
            theYoutubeQualityLayout = new System.Windows.Forms.TableLayoutPanel();
            theYouTubeQualityIcon = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            theAutoUpdateGroup = new System.Windows.Forms.GroupBox();
            theCheckForUpdatesIcon = new System.Windows.Forms.PictureBox();
            theMainLayout = new System.Windows.Forms.TableLayoutPanel();
            theYouTubeGroup.SuspendLayout();
            theYoutubeQualityLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(theYouTubeQualityIcon)).BeginInit();
            theAutoUpdateGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(theCheckForUpdatesIcon)).BeginInit();
            theMainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // theYouTubeGroup
            // 
            theYouTubeGroup.AutoSize = true;
            theYouTubeGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theYouTubeGroup.Controls.Add(theYoutubeQualityLayout);
            theYouTubeGroup.Location = new System.Drawing.Point(25, 148);
            theYouTubeGroup.Name = "theYouTubeGroup";
            theYouTubeGroup.Size = new System.Drawing.Size(272, 166);
            theYouTubeGroup.TabIndex = 1;
            theYouTubeGroup.TabStop = false;
            theYouTubeGroup.Text = "Preferred Quality for YouTube Videos:";
            // 
            // theYoutubeQualityLayout
            // 
            theYoutubeQualityLayout.AutoSize = true;
            theYoutubeQualityLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theYoutubeQualityLayout.ColumnCount = 5;
            theYoutubeQualityLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theYoutubeQualityLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            theYoutubeQualityLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theYoutubeQualityLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theYoutubeQualityLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            theYoutubeQualityLayout.Controls.Add(this.mFmt37, 2, 4);
            theYoutubeQualityLayout.Controls.Add(this.label5, 3, 4);
            theYoutubeQualityLayout.Controls.Add(this.mFmt22, 2, 3);
            theYoutubeQualityLayout.Controls.Add(this.mNormalQuality, 2, 0);
            theYoutubeQualityLayout.Controls.Add(this.mFmt35, 2, 2);
            theYoutubeQualityLayout.Controls.Add(this.mFmt18, 2, 1);
            theYoutubeQualityLayout.Controls.Add(label1, 3, 0);
            theYoutubeQualityLayout.Controls.Add(label4, 3, 3);
            theYoutubeQualityLayout.Controls.Add(label2, 3, 1);
            theYoutubeQualityLayout.Controls.Add(label3, 3, 2);
            theYoutubeQualityLayout.Controls.Add(theYouTubeQualityIcon, 0, 0);
            theYoutubeQualityLayout.Location = new System.Drawing.Point(10, 22);
            theYoutubeQualityLayout.Margin = new System.Windows.Forms.Padding(0);
            theYoutubeQualityLayout.Name = "theYoutubeQualityLayout";
            theYoutubeQualityLayout.RowCount = 5;
            theYoutubeQualityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theYoutubeQualityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theYoutubeQualityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theYoutubeQualityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theYoutubeQualityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theYoutubeQualityLayout.Size = new System.Drawing.Size(259, 125);
            theYoutubeQualityLayout.TabIndex = 11;
            // 
            // theYouTubeQualityIcon
            // 
            theYouTubeQualityIcon.Image = global::SaveMedia.Properties.Resources.Youtube48x48;
            theYouTubeQualityIcon.Location = new System.Drawing.Point(3, 3);
            theYouTubeQualityIcon.Name = "theYouTubeQualityIcon";
            theYoutubeQualityLayout.SetRowSpan(theYouTubeQualityIcon, 3);
            theYouTubeQualityIcon.Size = new System.Drawing.Size(48, 48);
            theYouTubeQualityIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            theYouTubeQualityIcon.TabIndex = 10;
            theYouTubeQualityIcon.TabStop = false;
            // 
            // mFmt37
            // 
            this.mFmt37.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mFmt37.AutoSize = true;
            this.mFmt37.Location = new System.Drawing.Point(77, 103);
            this.mFmt37.Name = "mFmt37";
            this.mFmt37.Size = new System.Drawing.Size(84, 19);
            this.mFmt37.TabIndex = 5;
            this.mFmt37.Tag = "37";
            this.mFmt37.Text = "HD (1080p)";
            this.mFmt37.UseVisualStyleBackColor = true;
            this.mFmt37.CheckedChanged += new System.EventHandler(this.HandleYoutubeQualityChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(167, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = " - 1920 x 1080";
            // 
            // mFmt22
            // 
            this.mFmt22.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mFmt22.AutoSize = true;
            this.mFmt22.Location = new System.Drawing.Point(77, 78);
            this.mFmt22.Name = "mFmt22";
            this.mFmt22.Size = new System.Drawing.Size(78, 19);
            this.mFmt22.TabIndex = 4;
            this.mFmt22.Tag = "22";
            this.mFmt22.Text = "HD (720p)";
            this.mFmt22.UseVisualStyleBackColor = true;
            this.mFmt22.CheckedChanged += new System.EventHandler(this.HandleYoutubeQualityChanged);
            // 
            // mNormalQuality
            // 
            this.mNormalQuality.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mNormalQuality.AutoSize = true;
            this.mNormalQuality.Checked = true;
            this.mNormalQuality.Location = new System.Drawing.Point(77, 3);
            this.mNormalQuality.Name = "mNormalQuality";
            this.mNormalQuality.Size = new System.Drawing.Size(72, 19);
            this.mNormalQuality.TabIndex = 2;
            this.mNormalQuality.TabStop = true;
            this.mNormalQuality.Tag = "34";
            this.mNormalQuality.Text = "Standard";
            this.mNormalQuality.UseVisualStyleBackColor = true;
            this.mNormalQuality.CheckedChanged += new System.EventHandler(this.HandleYoutubeQualityChanged);
            // 
            // mFmt35
            // 
            this.mFmt35.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mFmt35.AutoSize = true;
            this.mFmt35.Location = new System.Drawing.Point(77, 53);
            this.mFmt35.Name = "mFmt35";
            this.mFmt35.Size = new System.Drawing.Size(43, 19);
            this.mFmt35.TabIndex = 3;
            this.mFmt35.Tag = "35";
            this.mFmt35.Text = "HQ";
            this.mFmt35.UseVisualStyleBackColor = true;
            this.mFmt35.CheckedChanged += new System.EventHandler(this.HandleYoutubeQualityChanged);
            // 
            // mFmt18
            // 
            this.mFmt18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mFmt18.AutoSize = true;
            this.mFmt18.Location = new System.Drawing.Point(77, 28);
            this.mFmt18.Name = "mFmt18";
            this.mFmt18.Size = new System.Drawing.Size(49, 19);
            this.mFmt18.TabIndex = 5;
            this.mFmt18.Tag = "18";
            this.mFmt18.Text = "iPod";
            this.mFmt18.UseVisualStyleBackColor = true;
            this.mFmt18.CheckedChanged += new System.EventHandler(this.HandleYoutubeQualityChanged);
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(167, 5);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(71, 15);
            label1.TabIndex = 6;
            label1.Text = " -   640 x 360";
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(167, 80);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(71, 15);
            label4.TabIndex = 9;
            label4.Text = " - 1280 x 720";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(167, 30);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(71, 15);
            label2.TabIndex = 7;
            label2.Text = " -   480 x 360";
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(167, 55);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(71, 15);
            label3.TabIndex = 8;
            label3.Text = " -   854 x 480";
            // 
            // theAutoUpdateGroup
            // 
            theAutoUpdateGroup.Controls.Add(theCheckForUpdatesIcon);
            theAutoUpdateGroup.Controls.Add(this.mNeverUpdate);
            theAutoUpdateGroup.Controls.Add(this.mAutoUpdate);
            theAutoUpdateGroup.Controls.Add(this.mUpdateWhenFails);
            theAutoUpdateGroup.Location = new System.Drawing.Point(25, 9);
            theAutoUpdateGroup.Name = "theAutoUpdateGroup";
            theAutoUpdateGroup.Size = new System.Drawing.Size(272, 133);
            theAutoUpdateGroup.TabIndex = 0;
            theAutoUpdateGroup.TabStop = false;
            theAutoUpdateGroup.Text = "Check for Updates";
            theAutoUpdateGroup.Visible = false;
            // 
            // theCheckForUpdatesIcon
            // 
            theCheckForUpdatesIcon.Image = global::SaveMedia.Properties.Resources.SaveMedia;
            theCheckForUpdatesIcon.Location = new System.Drawing.Point(10, 24);
            theCheckForUpdatesIcon.Name = "theCheckForUpdatesIcon";
            theCheckForUpdatesIcon.Size = new System.Drawing.Size(48, 48);
            theCheckForUpdatesIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            theCheckForUpdatesIcon.TabIndex = 2;
            theCheckForUpdatesIcon.TabStop = false;
            // 
            // mNeverUpdate
            // 
            this.mNeverUpdate.AutoSize = true;
            this.mNeverUpdate.Location = new System.Drawing.Point(97, 77);
            this.mNeverUpdate.Name = "mNeverUpdate";
            this.mNeverUpdate.Size = new System.Drawing.Size(54, 17);
            this.mNeverUpdate.TabIndex = 2;
            this.mNeverUpdate.Tag = "never";
            this.mNeverUpdate.Text = "Never";
            this.mNeverUpdate.UseVisualStyleBackColor = true;
            this.mNeverUpdate.CheckedChanged += new System.EventHandler(this.HandleCheckForUpdatesChanged);
            // 
            // mAutoUpdate
            // 
            this.mAutoUpdate.AutoSize = true;
            this.mAutoUpdate.Checked = true;
            this.mAutoUpdate.Location = new System.Drawing.Point(97, 24);
            this.mAutoUpdate.Name = "mAutoUpdate";
            this.mAutoUpdate.Size = new System.Drawing.Size(87, 17);
            this.mAutoUpdate.TabIndex = 0;
            this.mAutoUpdate.TabStop = true;
            this.mAutoUpdate.Tag = "auto";
            this.mAutoUpdate.Text = "Automatically";
            this.mAutoUpdate.UseVisualStyleBackColor = true;
            this.mAutoUpdate.CheckedChanged += new System.EventHandler(this.HandleCheckForUpdatesChanged);
            // 
            // mUpdateWhenFails
            // 
            this.mUpdateWhenFails.AutoSize = true;
            this.mUpdateWhenFails.Enabled = false;
            this.mUpdateWhenFails.Location = new System.Drawing.Point(97, 51);
            this.mUpdateWhenFails.Name = "mUpdateWhenFails";
            this.mUpdateWhenFails.Size = new System.Drawing.Size(124, 17);
            this.mUpdateWhenFails.TabIndex = 1;
            this.mUpdateWhenFails.Tag = "when fails";
            this.mUpdateWhenFails.Text = "When download fails";
            this.mUpdateWhenFails.UseVisualStyleBackColor = true;
            this.mUpdateWhenFails.CheckedChanged += new System.EventHandler(this.HandleCheckForUpdatesChanged);
            // 
            // theMainLayout
            // 
            theMainLayout.AutoSize = true;
            theMainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            theMainLayout.ColumnCount = 1;
            theMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            theMainLayout.Controls.Add(theAutoUpdateGroup, 0, 0);
            theMainLayout.Controls.Add(theYouTubeGroup, 0, 1);
            theMainLayout.Location = new System.Drawing.Point(0, 0);
            theMainLayout.Margin = new System.Windows.Forms.Padding(0);
            theMainLayout.Name = "theMainLayout";
            theMainLayout.Padding = new System.Windows.Forms.Padding(22, 6, 22, 12);
            theMainLayout.RowCount = 2;
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            theMainLayout.Size = new System.Drawing.Size(322, 329);
            theMainLayout.TabIndex = 2;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(322, 329);
            this.Controls.Add(theMainLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            theYouTubeGroup.ResumeLayout(false);
            theYouTubeGroup.PerformLayout();
            theYoutubeQualityLayout.ResumeLayout(false);
            theYoutubeQualityLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(theYouTubeQualityIcon)).EndInit();
            theAutoUpdateGroup.ResumeLayout(false);
            theAutoUpdateGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(theCheckForUpdatesIcon)).EndInit();
            theMainLayout.ResumeLayout(false);
            theMainLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton mFmt35;
        private System.Windows.Forms.RadioButton mNormalQuality;
        private System.Windows.Forms.RadioButton mFmt18;
        private System.Windows.Forms.RadioButton mFmt22;
        private System.Windows.Forms.RadioButton mNeverUpdate;
        private System.Windows.Forms.RadioButton mUpdateWhenFails;
        private System.Windows.Forms.RadioButton mAutoUpdate;
        private System.Windows.Forms.RadioButton mFmt37;
        private System.Windows.Forms.Label label5;
    }
}