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
    public partial class MainForm : Form
    {
        private const String gcFFmpegPath = "Plugin\\bin\\ffmpeg.exe";
        private const double gcOneMB = 1024*1024;

        private System.Net.WebClient mWebClient;
        private System.Net.WebClient mThumbnailClient;

        private String mDefaultTitle;
        private String mThumbnailPath;
        private String mDownloadDestination;
        private String mConversionDestination;
        private String mConversionTempInPath;
        private String mConversionTempOutPath;

        private System.Diagnostics.Process mPlugin;

        private long    mFileSize;
        private double  mFileSizeInMB;
        private String  mFileSizeString;

        private double  mDuration;
        private int     mPercentage;

        private String  mPlaylistDestination;

        private int     mWaitingTime;
        private String  mDelayDownloadDestination;
        private Timer   mDelayTimer;

        private System.Collections.Generic.List< DownloadTag > mDownloadQueue;
        private System.Collections.Generic.List< String > mConvertQueue;

        delegate void ChangeLayoutCallBack( String aPhase );
        delegate void NotifyUserCallBack();

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

            ( (CustomComboBox)mConversion ).BuildImageList();

            mDefaultTitle = SaveMedia.Program.Title + " " + SaveMedia.Program.TitleVersion;
            this.Text = mDefaultTitle;

            if( !System.IO.File.Exists( gcFFmpegPath ) )
            {
                mConversion.Items.Clear();
                mConversion.Items.Add( "Plug-in not found" );
                mConversion.Enabled = false;
            }
            mConversion.SelectedIndex = 0;
            ShowStatus( String.Empty );

            mWebClient = new System.Net.WebClient();
            //mWebClient.CachePolicy = new System.Net.Cache.RequestCachePolicy( System.Net.Cache.RequestCacheLevel.Revalidate );
            mWebClient.Encoding = System.Text.Encoding.UTF8;
            mWebClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            mWebClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler( DownloadCompleted );
            mWebClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler( DownloadProgressChanged );

            mThumbnailClient = new System.Net.WebClient();
            //mThumbnailClient.CachePolicy = new System.Net.Cache.RequestCachePolicy( System.Net.Cache.RequestCacheLevel.Revalidate );            
            mThumbnailClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            mThumbnailClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler( ThumbnailDownloadCompleted );

            mPlugin = null;

            mDuration = 0;
            mPercentage = 0;

            mWaitingTime = 0;
            mDelayDownloadDestination = String.Empty;
            mDelayTimer = new Timer();
            mDelayTimer.Interval = 1000;
            mDelayTimer.Tick += new EventHandler( mDelayTimer_Tick );

            mDownloadQueue = new List< DownloadTag >();
            mConvertQueue = new List< String >();
        }

        private void mDelayTimer_Tick( object sender, EventArgs e )
        {
            --mWaitingTime;

            if( mWaitingTime <= 0 )
            {
                mDelayTimer.Stop();

                if( mDownloadQueue.Count != 0 )
                {
                    DownloadFile( mDownloadQueue[ 0 ] );
                }
                else
                {
                    ChangeLayout( "Download failed" );
                }
            }
            else
            {
                ShowStatus( "Download begins in " + mWaitingTime + " secs" );
            }
        }

        private void mOkButton_Click( object sender, EventArgs e )
        {
            ChangeLayout( "OK clicked" );
            ClearTemporaryFiles();
        }

        private void mCancelButton_Click( object sender, EventArgs e )
        {
            if( mWaitingTime != 0 )
            {
                mWaitingTime = 0;
                mDelayTimer.Stop();
                ChangeLayout( "Download cancelled" );
            }
            else if( mPlugin != null )
            {
                mPlugin.Kill();
                ChangeLayout( "Conversion cancelled" );
            }
            else
            {
                mThumbnailClient.CancelAsync();
                mWebClient.CancelAsync();
            }
        }

        private void mDownloadButton_Click( object sender, EventArgs e )
        {
            InputEnabled( false );
            mDownloadQueue.Clear();
            mConvertQueue.Clear();
            mPlaylistDestination = String.Empty;
            mProgressBar.Value = 0;

            Uri theUrl;
            bool isValid = Uri.TryCreate( mUrl.Text, UriKind.Absolute, out theUrl );

            if( !isValid )
            {
                ShowStatus( "Unsupported URL" );
                InputEnabled( true );
                return;
            }

            UrlParser( ref theUrl );
        }

        private void UrlParser( ref Uri aUrl )
        {
            if( aUrl.OriginalString.StartsWith( "http://www.youtube.com" ) )
            {
                YouTubeVideoParser( ref aUrl );
            }
            else if( aUrl.OriginalString.StartsWith( "http://www.tudou.com" ) )
            {
                DownloadTudouVideo( ref aUrl );
            }
            else if( aUrl.OriginalString.StartsWith( "http://www.newgrounds.com" ) )
            {
                DownloadNewgroundsVideo( ref aUrl );
            }
            else if( aUrl.OriginalString.StartsWith( "http://vimeo.com" ) )
            {
                DownloadVimeoVideo( ref aUrl );
            }
            else if( aUrl.OriginalString.StartsWith( "http://www.collegehumor.com" ) )
            {
                DownloadCollegeHumorVideo( ref aUrl );
            }
            else if( aUrl.OriginalString.StartsWith( "http://link.brightcove.com" ) )
            {
                //DownloadBrightcoveVideo( ref theUrl );
                mStatus.Text = "Sorry, this site is not supported yet";
                InputEnabled( true );
                return;
            }
            else if( aUrl.Host.EndsWith( ".rapidshare.com" ) )
            {
                DownloadRapidShareFile( ref aUrl );
            }
            else if( aUrl.OriginalString.StartsWith( "http://" ) )
            {
                String theFilename = System.IO.Path.GetFileName( aUrl.OriginalString );
                theFilename = FileUtils.FilenameCheck( theFilename );

                String theFileExt = System.IO.Path.GetExtension( aUrl.OriginalString );
                String theFilePath = FileUtils.SaveFile( theFilename, theFileExt + "|*" + theFileExt, this );

                DownloadFile( aUrl, theFilePath );
            }
            else
            {
                if( mConversion.SelectedIndex == 0 )
                {
                    InputEnabled( true );
                    return;
                }
                else if( !String.IsNullOrEmpty( aUrl.OriginalString ) &&
                         System.IO.File.Exists( aUrl.OriginalString ) )
                {
                    ConvertFile( mUrl.Text, mUrl.Text );
                }
            }
        }

        private void HandleConversionCompleted( object sender, System.EventArgs e )
        {
            mPlugin.CancelErrorRead();
            mPlugin.CancelOutputRead();

            bool isSuccess = false;

            if( System.IO.File.Exists( mConversionTempOutPath ) )
            {
                if( mPlugin.ExitCode == 0 )
                {
                    isSuccess = true;
                }
                else
                {
                    System.IO.FileInfo theFileInfo = new System.IO.FileInfo( mConversionTempOutPath );
                    isSuccess = ( theFileInfo.Length > 0 );
                }
            }

            mPlugin.Close();
            mPlugin = null;

            if( isSuccess )
            {
                System.IO.File.Delete( mConversionDestination );
                System.IO.File.Move( mConversionTempOutPath, mConversionDestination );
            }

            if( mConvertQueue.Count > 0 )
            {
                mConvertQueue.RemoveAt( 0 );
            }

            if( mConvertQueue.Count != 0 )
            {
                ConvertFile( mConvertQueue[ 0 ], mConvertQueue[ 0 ] );
            }
            else if( isSuccess )
            {
                ChangeLayout( "Conversion completed" );
            }
            else
            {
                ChangeLayout( "Conversion failed" );
            }
        }

        private void HandleStandardOutputData( object sendingProcess, System.Diagnostics.DataReceivedEventArgs outLine )
        {
            if( !String.IsNullOrEmpty( outLine.Data ) )
            {
                if( mDuration == 0 )
                {
                    String thePattern = "Duration: (\\d{2}):(\\d{2}):([\\d|.]+),";
                    Match theMatch = Regex.Match( outLine.Data, thePattern );
                    if( theMatch.Success && theMatch.Groups.Count == 4 )
                    {
                        double theHours = System.Convert.ToDouble( theMatch.Groups[ 1 ].ToString() );
                        double theMinutes = System.Convert.ToDouble( theMatch.Groups[ 2 ].ToString() );
                        double theSeconds = System.Convert.ToDouble( theMatch.Groups[ 3 ].ToString() );
                        mDuration = theHours * 3600 + theMinutes * 60 + theSeconds;
                    }
                }
                else
                {
                    String thePattern = "time=([\\d|.]+) ";
                    Match theMatch = Regex.Match( outLine.Data, thePattern );
                    if( theMatch.Success && theMatch.Groups.Count == 2 )
                    {
                        double theProgress = System.Convert.ToDouble( theMatch.Groups[ 1 ].ToString() );
                        mPercentage = (int)( theProgress / mDuration * 100 );
                        if( mPercentage > 100 )
                        {
                            mPercentage = 100;
                        }

                        ChangeLayout( "Converting..." );
                    }
                }
            }
        }

        private void DownloadRapidShareFile( ref Uri aUrl )
        {
            mDelayTimer.Stop();

            ShowStatus( "Connecting to " + aUrl.Host );

            DownloadTag theTag;

            Sites.RapidShare.TryParse( ref aUrl, out theTag );

            if( !String.IsNullOrEmpty( theTag.Error ) )
            {
                ShowStatus( theTag.Error );
                InputEnabled( true );
                return;
            }

            mWaitingTime = theTag.WaitingTime;
            mDelayTimer.Start();

            theTag.DownloadDestination = FileUtils.SaveFile( theTag.Filename, theTag.FileExtension + "|*" + theTag.FileExtension, this );

            if( String.IsNullOrEmpty( theTag.DownloadDestination ) )
            {
                ChangeLayout( "Cancel clicked" );
                mDelayTimer.Stop();
                ClearTemporaryFiles();
                return;
            }

            mDownloadQueue.Add( theTag );

            mDownloadButton.Visible = false;
            mCancelButton.Visible = true;
        }

        private void YouTubeVideoParser( ref Uri aUrl )
        {
            if( aUrl.OriginalString.StartsWith( "http://www.youtube.com/view_play_list?" ) )
            {
                int thePageNumber = 1;
                DownloadYouTubePlaylist( ref aUrl, thePageNumber );
            }
            else if( aUrl.OriginalString.StartsWith( "http://www.youtube.com/v/" ) )
            {
                String theNewUrlString = aUrl.OriginalString;
                theNewUrlString = theNewUrlString.Replace( "http://www.youtube.com/v/", "http://www.youtube.com/watch?v=" );

                Uri theNewUrl = new Uri( theNewUrlString );
                UrlParser( ref theNewUrl );
            }
            else
            {
                DownloadYouTubeVideo( ref aUrl );
            }
        }

        private void DownloadYouTubePlaylist( ref Uri aUrl, int aPageNumber )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( aUrl.Query );
            String thePlaylistId = theQueryStrings[ "p" ];
            String thePageNumber = theQueryStrings[ "page" ];

            if( String.IsNullOrEmpty( thePageNumber ) )
            {
                thePageNumber = "1";
            }

            if( aPageNumber == 1 &&
                !thePageNumber.Equals( aPageNumber.ToString() ) )
            {
                Uri theFirstPage = new Uri( "http://www.youtube.com/view_play_list?p=" + thePlaylistId );
                DownloadYouTubePlaylist( ref theFirstPage, 1 );
                return;
            }

            String theSourceCode;
            if( !NetUtils.DownloadString( aUrl, out theSourceCode ) )
            {
                ShowStatus( "Failed to connect to " + aUrl.Host );
                InputEnabled( true );
                return;
            }

            String thePlaylistTitle;
            if( !StringUtils.StringBetween( theSourceCode, "<h1>", "</h1>", out thePlaylistTitle ) )
            {
                ShowStatus( "Failed to analyze playlist" );
                InputEnabled( true );
                return;
            }

            // http://msdn.microsoft.com/en-us/library/az24scfc.aspx
            String theUrlPattern = "<div\\s+class=\"video-short-title\"\\s*>(\\s|\n)+<a\\s.*href=\"([^\"]+)\"";
            Match theUrlMatch = Regex.Match( theSourceCode, theUrlPattern );

            if( !theUrlMatch.Success )
            {
                ShowStatus( "Failed to analyze playlist" );
                InputEnabled( true );
                return;
            }

            if( String.IsNullOrEmpty( mPlaylistDestination ) )
            {
                FolderBrowserDialog theDialog = new FolderBrowserDialog();
                theDialog.Description = "Please select the destination for videos from:\n\n" + thePlaylistTitle;
                if( theDialog.ShowDialog( this ) != DialogResult.OK )
                {
                    ChangeLayout( "Cancel clicked" );
                    ClearTemporaryFiles();
                    return;
                }
                mPlaylistDestination = theDialog.SelectedPath;
            }

            while( theUrlMatch.Success )
            {
                String thePartialUrl = theUrlMatch.Groups[ 2 ].ToString();
                Uri theVideoUrl = new Uri( "http://" + aUrl.Host + thePartialUrl );

                DownloadTag theTag;

                Sites.YouTube.TryParse( ref theVideoUrl, out theTag );

                if( !String.IsNullOrEmpty( theTag.Error ) )
                {
                    ShowStatus( theTag.Error );
                    InputEnabled( true );
                    return;
                }

                theTag.DownloadDestination = mPlaylistDestination + "\\" + FileUtils.FilenameCheck( theTag.VideoTitle ) + ".flv";

                mDownloadQueue.Add( theTag );

                theUrlMatch = theUrlMatch.NextMatch();
            }

            int theCurrentPageIndex = theSourceCode.IndexOf( "class=\"pagerCurrent\"" );
            int theNextPageIndex = theSourceCode.LastIndexOf( "class=\"pagerNotCurrent\"" );

            if( theCurrentPageIndex != -1 && theNextPageIndex > theCurrentPageIndex )
            {
                int theNextPageNumber = Convert.ToInt32( thePageNumber ) + 1;
                Uri theNextPage = new Uri( "http://www.youtube.com/view_play_list?p=" + thePlaylistId +
                                           "&page=" + theNextPageNumber );
                DownloadYouTubePlaylist( ref theNextPage, theNextPageNumber );
                return;
            }
            else if( mDownloadQueue.Count != 0 )
            {
                DownloadFile( mDownloadQueue[ 0 ] );
            }
        }

        private void DownloadYouTubeVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            DownloadTag theTag;

            Sites.YouTube.TryParse( ref aUrl, out theTag );

            if( !String.IsNullOrEmpty( theTag.Error ) )
            {
                ShowStatus( theTag.Error );
                InputEnabled( true );
                return;
            }

            DownloadThumbnail( theTag.ThumbnailUrl );
            DisplayMediaInfo( theTag.VideoTitle );

            String theFilePath = FileUtils.SaveFile( theTag.Filename, theTag.FileExtension, this );

            DownloadFile( theTag.VideoUrl, theFilePath );
        }

        private void DownloadTudouVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            DownloadTag theTag;

            Sites.Tudou.TryParse( ref aUrl, out theTag );

            if( !String.IsNullOrEmpty( theTag.Error ) )
            {
                ShowStatus( theTag.Error );
                InputEnabled( true );
                return;
            }

            DownloadThumbnail( theTag.ThumbnailUrl );
            DisplayMediaInfo( theTag.VideoTitle );

            String theFilePath = FileUtils.SaveFile( theTag.Filename, theTag.FileExtension, this );

            DownloadFile( theTag.VideoUrl, theFilePath );
        }

        private void DownloadNewgroundsVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            DownloadTag theTag;

            Sites.NewGrounds.TryParse( ref aUrl, out theTag );

            if( !String.IsNullOrEmpty( theTag.Error ) )
            {
                ShowStatus( theTag.Error );
                InputEnabled( true );
                return;
            }

            DownloadThumbnail( theTag.ThumbnailUrl );
            DisplayMediaInfo( theTag.VideoTitle );

            String theFilePath = FileUtils.SaveFile( theTag.Filename, theTag.FileExtension, this );

            DownloadFile( theTag.VideoUrl, theFilePath );
        }

        private void DownloadVimeoVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            DownloadTag theTag;

            Sites.Vimeo.TryParse( ref aUrl, out theTag );

            if( !String.IsNullOrEmpty( theTag.Error ) )
            {
                ShowStatus( theTag.Error );
                InputEnabled( true );
                return;
            }

            DownloadThumbnail( theTag.ThumbnailUrl );
            DisplayMediaInfo( theTag.VideoTitle );

            String theFilePath = FileUtils.SaveFile( theTag.Filename, theTag.FileExtension, this );

            DownloadFile( theTag.VideoUrl, theFilePath );
        }

        private void DownloadCollegeHumorVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            DownloadTag theTag;

            Sites.CollegeHumor.TryParse( ref aUrl, out theTag );

            if( !String.IsNullOrEmpty( theTag.Error ) )
            {
                ShowStatus( theTag.Error );
                InputEnabled( true );
                return;
            }

            DownloadThumbnail( theTag.ThumbnailUrl );
            DisplayMediaInfo( theTag.VideoTitle );

            String theFilePath = FileUtils.SaveFile( theTag.Filename, theTag.FileExtension, this );

            DownloadFile( theTag.VideoUrl, theFilePath );
        }

        private void DisplayMediaInfo( String aMediaTitle )
        {
            if( String.IsNullOrEmpty( aMediaTitle ) )
            {
                return;
            }

            mTitleLabel.Text = "Title: " + aMediaTitle.Replace( "\\", "" );
            mSizeLabel.Text = "Size: " + mFileSizeString;

            mLocationLabel.Text = String.Empty;
            
            ShowStatus( "Ready to download" );
            MediaInfoVisible( true );

            System.Windows.Forms.Application.DoEvents();
        }

        private void DownloadThumbnail( Uri aUrl )
        {
            if( aUrl == null )
            {
                return;
            }

            mThumbnailPath = System.IO.Path.GetTempFileName();
            mThumbnailClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
            mThumbnailClient.DownloadFileAsync( aUrl, mThumbnailPath );
        }

        private void DownloadFile( DownloadTag aTag )
        {
            DownloadThumbnail( aTag.ThumbnailUrl );
            DisplayMediaInfo( aTag.VideoTitle );

            DownloadFile( aTag.VideoUrl, aTag.DownloadDestination );
        }

        private void DownloadFile( Uri aUrl, String aDestination )
        {
            if( String.IsNullOrEmpty( aDestination ) )
            {
                ChangeLayout( "Cancel clicked" );
                ClearTemporaryFiles();
                return;
            }

            mFileSize = 0;
            mFileSizeString = "??? MB";

            mProgressBar.Value = 0;
            mDownloadDestination = aDestination;

            mWebClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
            mWebClient.DownloadFileAsync( aUrl, mDownloadDestination );

            this.Text = "0% - " + mDefaultTitle;
            ShowStatus( "Downloading..." );
            //mLocationLabel.Text = "Location: " + mDownloadDestination;

            mDownloadButton.Visible = false;
            mCancelButton.Visible = true;
        }

        private void DownloadProgressChanged( object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            if( mFileSize == 0 )
            {
                mFileSize = e.TotalBytesToReceive;
                mFileSizeInMB = mFileSize;
                mFileSizeInMB /= gcOneMB;
                mFileSizeString = mFileSizeInMB.ToString( "0.000" ) + " MB";
                mSizeLabel.Text = "Size: " + mFileSizeString;
            }

            this.Text = e.ProgressPercentage + "% - " + mDefaultTitle;
            mProgressBar.Value = e.ProgressPercentage;

            double theReceivedFileSizeInMB = e.BytesReceived;
            theReceivedFileSizeInMB /= gcOneMB;
            ShowStatus( "Downloading..." +
                        theReceivedFileSizeInMB.ToString( "0.000" ) +
                        "/" +
                        mFileSizeString );
        }

        private void DownloadCompleted( object sender, System.ComponentModel.AsyncCompletedEventArgs e )
        {
            if( e.Cancelled )
            {
                ChangeLayout( "Download cancelled" );
            }
            else if( e.Error != null )
            {
                ChangeLayout( "Download failed" );
            }
            else
            {
                if( mConversion.SelectedIndex != 0 )
                {
                    mConvertQueue.Add( mDownloadDestination );
                }

                if( mDownloadQueue.Count > 0 )
                {
                    mDownloadQueue.RemoveAt( 0 );
                }

                if( mDownloadQueue.Count != 0 )
                {
                    //Uri theNextUrl = mDownloadQueue[ 0 ];
                    //UrlParser( ref theNextUrl );
                    DownloadFile( mDownloadQueue[ 0 ] );
                }
                else
                {
                    if( mConversion.SelectedIndex == 0 ||
                        mConvertQueue.Count == 0 )
                    {
                        ChangeLayout( "Download completed" );
                    }
                    else
                    {
                        ConvertFile( mConvertQueue[ 0 ], mConvertQueue[ 0 ] );
                    }
                }
            }
        }

        private void ConvertFile( String aSource, String aDestination )
        {
            if( mConversion.SelectedIndex == 0 )
            {
                return;
            }

            if( !System.IO.File.Exists( gcFFmpegPath ) )
            {
                ChangeLayout( "Conversion failed, plug-in not found" );
                return;
            }

            mPercentage = 0;
            ChangeLayout( "Converting..." );

            mConversionDestination = aDestination;

            String theExtension = String.Empty;
            String theArguments = String.Empty;

            if( mConversion.SelectedIndex == 1 )
            {
                theExtension = ".mp3";
                theArguments = "-y -i \"{0}\" -ar 44100 -ab 192k -ac 2 \"{1}\"";
            }
            else if( mConversion.SelectedIndex == 2 )
            {
                theExtension = ".wmv";
                theArguments = "-y -i \"{0}\" -vcodec wmv2 -sameq -acodec mp2 -ar 44100 -ab 192k -f avi \"{1}\"";
            }

            mConversionTempInPath = System.IO.Path.GetTempFileName();
            mConversionTempOutPath = System.IO.Path.GetTempFileName();

            mConversionDestination = System.IO.Path.ChangeExtension( aDestination, theExtension );
            mConversionTempOutPath = System.IO.Path.ChangeExtension( mConversionTempOutPath, theExtension );

            System.IO.File.Copy( aSource, mConversionTempInPath, true );

            System.Diagnostics.ProcessStartInfo theStartInfo = new System.Diagnostics.ProcessStartInfo( gcFFmpegPath );

            theStartInfo.Arguments = String.Format( theArguments, mConversionTempInPath, mConversionTempOutPath );
            theStartInfo.CreateNoWindow = true;
            theStartInfo.UseShellExecute = false;
            theStartInfo.RedirectStandardError = true;
            theStartInfo.RedirectStandardOutput = true;
            theStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            mPlugin = new System.Diagnostics.Process();
            mPlugin.Exited += new EventHandler( HandleConversionCompleted );
            mPlugin.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler( HandleStandardOutputData );
            mPlugin.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler( HandleStandardOutputData );
            mPlugin.EnableRaisingEvents = true;
            mPlugin.StartInfo = theStartInfo;

            mPlugin.Start();
            mPlugin.BeginErrorReadLine();
            mPlugin.BeginOutputReadLine();

            mDownloadButton.Visible = false;
            mCancelButton.Visible = true;
        }

        private void ThumbnailDownloadCompleted( object sender, System.ComponentModel.AsyncCompletedEventArgs e )
        {
            if( !e.Cancelled && e.Error == null )
            {
                mThumbnail.ImageLocation = mThumbnailPath;
            }
        }

        private void ChangeLayout( String aPhase )
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

                    ShowStatus( "Download cancelled" );

                    MediaInfoVisible( false );
                    InputEnabled( true );
                    break;

                case "Converting...":

                    this.Text = mPercentage.ToString() + "% - " + mDefaultTitle;
                    mProgressBar.Value = mPercentage;

                    ShowStatus( aPhase + mPercentage.ToString() + "%" );
                    break;

                case "Conversion cancelled":
                case "Conversion completed":
                case "Conversion failed":
                case "Conversion failed, plug-in not found":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    ShowStatus( aPhase );

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

                    ShowStatus( aPhase );

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;
                
                case "Download completed":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    ShowStatus( aPhase );

                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                case "Download failed":

                    this.Text = mDefaultTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    ShowStatus( aPhase );

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    NotifyUser();
                    break;

                case "OK clicked":

                    InputEnabled( true );
                    MediaInfoVisible( false );
                    mProgressBar.Value = 0;
                    ShowStatus( String.Empty );

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

        private void InputEnabled( bool aIsEnabled )
        {
            mUrl.Enabled = aIsEnabled;
            mConversion.Enabled = aIsEnabled && System.IO.File.Exists( gcFFmpegPath );

            if( aIsEnabled )
            {
                String theNewUrl = ClipboardUtils.ReadClipboardUrl();
                if( !mUrl.Text.Equals( theNewUrl ) && !String.IsNullOrEmpty( theNewUrl ) )
                {
                    mUrl.Text = ClipboardUtils.ReadClipboardUrl();
                }
                mUrl_TextChanged( this, new EventArgs() );
            }
            else
            {
                mDownloadButton.Enabled = false;
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
            this.ResumeLayout( false );
            this.PerformLayout();
        }

        private void ShowStatus( String aMessage )
        {
            mStatus.Text = aMessage;
            System.Windows.Forms.Application.DoEvents();
        }

        private void MainForm_FormClosed( object sender, FormClosedEventArgs e )
        {
            ClearTemporaryFiles();
        }

        private void DeleteFile( String aFilePath )
        {
            if( !String.IsNullOrEmpty( aFilePath ) )
            {
                try
                {
                    System.IO.File.Delete( aFilePath );
                }
                catch( System.Exception /*ex*/ )
                {
                    // TODO: write error to log
                }
            }
        }

        private void ClearTemporaryFiles()
        {
            DeleteFile( mThumbnailPath );
            DeleteFile( mConversionTempInPath );
            DeleteFile( mConversionTempOutPath );
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
            String theNewUrl = ClipboardUtils.ReadClipboardUrl();
            if( mUrl.Enabled && !mUrl.Text.Equals( theNewUrl ) && !String.IsNullOrEmpty( theNewUrl ) )
            {
                mUrl.Text = theNewUrl;
            }
        }
    }
}