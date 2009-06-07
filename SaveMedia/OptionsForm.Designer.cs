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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( OptionsForm ) );
            this.mFmt18 = new System.Windows.Forms.RadioButton();
            this.mFmt22 = new System.Windows.Forms.RadioButton();
            this.mFmt6 = new System.Windows.Forms.RadioButton();
            this.mNormalQuality = new System.Windows.Forms.RadioButton();
            theYouTubeGroup = new System.Windows.Forms.GroupBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            theYouTubeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // theYouTubeGroup
            // 
            theYouTubeGroup.AutoSize = true;
            theYouTubeGroup.Controls.Add( label4 );
            theYouTubeGroup.Controls.Add( label3 );
            theYouTubeGroup.Controls.Add( label2 );
            theYouTubeGroup.Controls.Add( label1 );
            theYouTubeGroup.Controls.Add( this.mFmt18 );
            theYouTubeGroup.Controls.Add( this.mFmt22 );
            theYouTubeGroup.Controls.Add( this.mFmt6 );
            theYouTubeGroup.Controls.Add( this.mNormalQuality );
            theYouTubeGroup.Location = new System.Drawing.Point( 12, 12 );
            theYouTubeGroup.Margin = new System.Windows.Forms.Padding( 3, 3, 12, 12 );
            theYouTubeGroup.Name = "theYouTubeGroup";
            theYouTubeGroup.Size = new System.Drawing.Size( 201, 129 );
            theYouTubeGroup.TabIndex = 0;
            theYouTubeGroup.TabStop = false;
            theYouTubeGroup.Text = "Prefered Quality for YouTube Videos:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point( 98, 26 );
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size( 69, 13 );
            label4.TabIndex = 9;
            label4.Text = " -   320 x 240";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point( 98, 49 );
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size( 69, 13 );
            label3.TabIndex = 8;
            label3.Text = " -   480 x 360";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point( 98, 72 );
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size( 69, 13 );
            label2.TabIndex = 7;
            label2.Text = " -   480 x 360";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point( 98, 95 );
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size( 69, 13 );
            label1.TabIndex = 6;
            label1.Text = " - 1280 x 720";
            // 
            // mFmt18
            // 
            this.mFmt18.AutoSize = true;
            this.mFmt18.Location = new System.Drawing.Point( 17, 70 );
            this.mFmt18.Name = "mFmt18";
            this.mFmt18.Size = new System.Drawing.Size( 72, 17 );
            this.mFmt18.TabIndex = 5;
            this.mFmt18.Text = "HQ (MP4)";
            this.mFmt18.UseVisualStyleBackColor = true;
            this.mFmt18.CheckedChanged += new System.EventHandler( this.HandleQualitySettingsChanged );
            // 
            // mFmt22
            // 
            this.mFmt22.AutoSize = true;
            this.mFmt22.Location = new System.Drawing.Point( 17, 93 );
            this.mFmt22.Name = "mFmt22";
            this.mFmt22.Size = new System.Drawing.Size( 41, 17 );
            this.mFmt22.TabIndex = 4;
            this.mFmt22.Text = "HD";
            this.mFmt22.UseVisualStyleBackColor = true;
            this.mFmt22.CheckedChanged += new System.EventHandler( this.HandleQualitySettingsChanged );
            // 
            // mFmt6
            // 
            this.mFmt6.AutoSize = true;
            this.mFmt6.Location = new System.Drawing.Point( 17, 47 );
            this.mFmt6.Name = "mFmt6";
            this.mFmt6.Size = new System.Drawing.Size( 69, 17 );
            this.mFmt6.TabIndex = 3;
            this.mFmt6.Text = "HQ (FLV)";
            this.mFmt6.UseVisualStyleBackColor = true;
            this.mFmt6.CheckedChanged += new System.EventHandler( this.HandleQualitySettingsChanged );
            // 
            // mNormalQuality
            // 
            this.mNormalQuality.AutoSize = true;
            this.mNormalQuality.Checked = true;
            this.mNormalQuality.Location = new System.Drawing.Point( 17, 24 );
            this.mNormalQuality.Name = "mNormalQuality";
            this.mNormalQuality.Size = new System.Drawing.Size( 58, 17 );
            this.mNormalQuality.TabIndex = 2;
            this.mNormalQuality.TabStop = true;
            this.mNormalQuality.Text = "Normal";
            this.mNormalQuality.UseVisualStyleBackColor = true;
            this.mNormalQuality.CheckedChanged += new System.EventHandler( this.HandleQualitySettingsChanged );
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size( 324, 208 );
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
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton mFmt6;
        private System.Windows.Forms.RadioButton mNormalQuality;
        private System.Windows.Forms.RadioButton mFmt18;
        private System.Windows.Forms.RadioButton mFmt22;
    }
}