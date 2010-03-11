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
            System.Windows.Forms.Label label4;
            System.Windows.Forms.PictureBox theYouTubeQualityIcon;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox theAutoUpdateGroup;
            System.Windows.Forms.PictureBox theCheckForUpdatesIcon;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( OptionsForm ) );
            this.label5 = new System.Windows.Forms.Label();
            this.mFmt37 = new System.Windows.Forms.RadioButton();
            this.mNormalQuality = new System.Windows.Forms.RadioButton();
            this.mFmt22 = new System.Windows.Forms.RadioButton();
            this.mFmt18 = new System.Windows.Forms.RadioButton();
            this.mFmt35 = new System.Windows.Forms.RadioButton();
            this.mNeverUpdate = new System.Windows.Forms.RadioButton();
            this.mAutoUpdate = new System.Windows.Forms.RadioButton();
            this.mUpdateWhenFails = new System.Windows.Forms.RadioButton();
            theYouTubeGroup = new System.Windows.Forms.GroupBox();
            label4 = new System.Windows.Forms.Label();
            theYouTubeQualityIcon = new System.Windows.Forms.PictureBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            theAutoUpdateGroup = new System.Windows.Forms.GroupBox();
            theCheckForUpdatesIcon = new System.Windows.Forms.PictureBox();
            theYouTubeGroup.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( theYouTubeQualityIcon ) ).BeginInit();
            theAutoUpdateGroup.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( theCheckForUpdatesIcon ) ).BeginInit();
            this.SuspendLayout();
            // 
            // theYouTubeGroup
            // 
            theYouTubeGroup.AutoSize = true;
            theYouTubeGroup.Controls.Add( this.label5 );
            theYouTubeGroup.Controls.Add( this.mFmt37 );
            theYouTubeGroup.Controls.Add( label4 );
            theYouTubeGroup.Controls.Add( theYouTubeQualityIcon );
            theYouTubeGroup.Controls.Add( this.mNormalQuality );
            theYouTubeGroup.Controls.Add( label3 );
            theYouTubeGroup.Controls.Add( this.mFmt22 );
            theYouTubeGroup.Controls.Add( this.mFmt18 );
            theYouTubeGroup.Controls.Add( label2 );
            theYouTubeGroup.Controls.Add( this.mFmt35 );
            theYouTubeGroup.Controls.Add( label1 );
            theYouTubeGroup.Location = new System.Drawing.Point( 12, 136 );
            theYouTubeGroup.Margin = new System.Windows.Forms.Padding( 12 );
            theYouTubeGroup.Name = "theYouTubeGroup";
            theYouTubeGroup.Size = new System.Drawing.Size( 277, 157 );
            theYouTubeGroup.TabIndex = 1;
            theYouTubeGroup.TabStop = false;
            theYouTubeGroup.Text = "Prefered Quality for YouTube Videos:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 164, 116 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 75, 13 );
            this.label5.TabIndex = 10;
            this.label5.Text = " - 1920 x 1080";
            // 
            // mFmt37
            // 
            this.mFmt37.AutoSize = true;
            this.mFmt37.Location = new System.Drawing.Point( 83, 114 );
            this.mFmt37.Name = "mFmt37";
            this.mFmt37.Size = new System.Drawing.Size( 80, 17 );
            this.mFmt37.TabIndex = 5;
            this.mFmt37.Tag = "37";
            this.mFmt37.Text = "HD (1080p)";
            this.mFmt37.UseVisualStyleBackColor = true;
            this.mFmt37.CheckedChanged += new System.EventHandler( this.HandleYoutubeQualityChanged );
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point( 164, 93 );
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size( 69, 13 );
            label4.TabIndex = 9;
            label4.Text = " - 1280 x 720";
            // 
            // theYouTubeQualityIcon
            // 
            theYouTubeQualityIcon.Image = global::SaveMedia.Properties.Resources.Youtube48x48;
            theYouTubeQualityIcon.Location = new System.Drawing.Point( 8, 22 );
            theYouTubeQualityIcon.Name = "theYouTubeQualityIcon";
            theYouTubeQualityIcon.Size = new System.Drawing.Size( 48, 48 );
            theYouTubeQualityIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            theYouTubeQualityIcon.TabIndex = 10;
            theYouTubeQualityIcon.TabStop = false;
            // 
            // mNormalQuality
            // 
            this.mNormalQuality.AutoSize = true;
            this.mNormalQuality.Checked = true;
            this.mNormalQuality.Location = new System.Drawing.Point( 83, 22 );
            this.mNormalQuality.Name = "mNormalQuality";
            this.mNormalQuality.Size = new System.Drawing.Size( 68, 17 );
            this.mNormalQuality.TabIndex = 2;
            this.mNormalQuality.TabStop = true;
            this.mNormalQuality.Tag = "34";
            this.mNormalQuality.Text = "Standard";
            this.mNormalQuality.UseVisualStyleBackColor = true;
            this.mNormalQuality.CheckedChanged += new System.EventHandler( this.HandleYoutubeQualityChanged );
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point( 164, 70 );
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size( 69, 13 );
            label3.TabIndex = 8;
            label3.Text = " -   854 x 480";
            // 
            // mFmt22
            // 
            this.mFmt22.AutoSize = true;
            this.mFmt22.Location = new System.Drawing.Point( 83, 91 );
            this.mFmt22.Name = "mFmt22";
            this.mFmt22.Size = new System.Drawing.Size( 74, 17 );
            this.mFmt22.TabIndex = 4;
            this.mFmt22.Tag = "22";
            this.mFmt22.Text = "HD (720p)";
            this.mFmt22.UseVisualStyleBackColor = true;
            this.mFmt22.CheckedChanged += new System.EventHandler( this.HandleYoutubeQualityChanged );
            // 
            // mFmt18
            // 
            this.mFmt18.AutoSize = true;
            this.mFmt18.Location = new System.Drawing.Point( 83, 45 );
            this.mFmt18.Name = "mFmt18";
            this.mFmt18.Size = new System.Drawing.Size( 46, 17 );
            this.mFmt18.TabIndex = 5;
            this.mFmt18.Tag = "18";
            this.mFmt18.Text = "iPod";
            this.mFmt18.UseVisualStyleBackColor = true;
            this.mFmt18.CheckedChanged += new System.EventHandler( this.HandleYoutubeQualityChanged );
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point( 164, 47 );
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size( 69, 13 );
            label2.TabIndex = 7;
            label2.Text = " -   480 x 360";
            // 
            // mFmt35
            // 
            this.mFmt35.AutoSize = true;
            this.mFmt35.Location = new System.Drawing.Point( 83, 68 );
            this.mFmt35.Name = "mFmt35";
            this.mFmt35.Size = new System.Drawing.Size( 41, 17 );
            this.mFmt35.TabIndex = 3;
            this.mFmt35.Tag = "35";
            this.mFmt35.Text = "HQ";
            this.mFmt35.UseVisualStyleBackColor = true;
            this.mFmt35.CheckedChanged += new System.EventHandler( this.HandleYoutubeQualityChanged );
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point( 164, 24 );
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size( 69, 13 );
            label1.TabIndex = 6;
            label1.Text = " -   640 x 360";
            // 
            // theAutoUpdateGroup
            // 
            theAutoUpdateGroup.Controls.Add( theCheckForUpdatesIcon );
            theAutoUpdateGroup.Controls.Add( this.mNeverUpdate );
            theAutoUpdateGroup.Controls.Add( this.mAutoUpdate );
            theAutoUpdateGroup.Controls.Add( this.mUpdateWhenFails );
            theAutoUpdateGroup.Location = new System.Drawing.Point( 12, 12 );
            theAutoUpdateGroup.Margin = new System.Windows.Forms.Padding( 12 );
            theAutoUpdateGroup.Name = "theAutoUpdateGroup";
            theAutoUpdateGroup.Size = new System.Drawing.Size( 277, 115 );
            theAutoUpdateGroup.TabIndex = 0;
            theAutoUpdateGroup.TabStop = false;
            theAutoUpdateGroup.Text = "Check for Updates";
            // 
            // theCheckForUpdatesIcon
            // 
            theCheckForUpdatesIcon.Image = global::SaveMedia.Properties.Resources.SaveMedia;
            theCheckForUpdatesIcon.Location = new System.Drawing.Point( 9, 21 );
            theCheckForUpdatesIcon.Name = "theCheckForUpdatesIcon";
            theCheckForUpdatesIcon.Size = new System.Drawing.Size( 48, 48 );
            theCheckForUpdatesIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            theCheckForUpdatesIcon.TabIndex = 2;
            theCheckForUpdatesIcon.TabStop = false;
            // 
            // mNeverUpdate
            // 
            this.mNeverUpdate.AutoSize = true;
            this.mNeverUpdate.Location = new System.Drawing.Point( 83, 67 );
            this.mNeverUpdate.Name = "mNeverUpdate";
            this.mNeverUpdate.Size = new System.Drawing.Size( 54, 17 );
            this.mNeverUpdate.TabIndex = 2;
            this.mNeverUpdate.Tag = "never";
            this.mNeverUpdate.Text = "Never";
            this.mNeverUpdate.UseVisualStyleBackColor = true;
            this.mNeverUpdate.CheckedChanged += new System.EventHandler( this.HandleCheckForUpdatesChanged );
            // 
            // mAutoUpdate
            // 
            this.mAutoUpdate.AutoSize = true;
            this.mAutoUpdate.Checked = true;
            this.mAutoUpdate.Location = new System.Drawing.Point( 83, 21 );
            this.mAutoUpdate.Name = "mAutoUpdate";
            this.mAutoUpdate.Size = new System.Drawing.Size( 87, 17 );
            this.mAutoUpdate.TabIndex = 0;
            this.mAutoUpdate.TabStop = true;
            this.mAutoUpdate.Tag = "auto";
            this.mAutoUpdate.Text = "Automatically";
            this.mAutoUpdate.UseVisualStyleBackColor = true;
            this.mAutoUpdate.CheckedChanged += new System.EventHandler( this.HandleCheckForUpdatesChanged );
            // 
            // mUpdateWhenFails
            // 
            this.mUpdateWhenFails.AutoSize = true;
            this.mUpdateWhenFails.Enabled = false;
            this.mUpdateWhenFails.Location = new System.Drawing.Point( 83, 44 );
            this.mUpdateWhenFails.Name = "mUpdateWhenFails";
            this.mUpdateWhenFails.Size = new System.Drawing.Size( 124, 17 );
            this.mUpdateWhenFails.TabIndex = 1;
            this.mUpdateWhenFails.Tag = "when fails";
            this.mUpdateWhenFails.Text = "When download fails";
            this.mUpdateWhenFails.UseVisualStyleBackColor = true;
            this.mUpdateWhenFails.CheckedChanged += new System.EventHandler( this.HandleCheckForUpdatesChanged );
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size( 434, 340 );
            this.Controls.Add( theAutoUpdateGroup );
            this.Controls.Add( theYouTubeGroup );
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.OptionsForm_FormClosing );
            theYouTubeGroup.ResumeLayout( false );
            theYouTubeGroup.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( theYouTubeQualityIcon ) ).EndInit();
            theAutoUpdateGroup.ResumeLayout( false );
            theAutoUpdateGroup.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( theCheckForUpdatesIcon ) ).EndInit();
            this.ResumeLayout( false );
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