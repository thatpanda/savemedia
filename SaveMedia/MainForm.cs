using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SaveMedia
{
    public partial class MainForm : Form
    {
        private const String gcFFmpegPath = "Plugin\\bin\\ffmpeg.exe";
        private const double gcOneMB = 1000000;

        private System.Net.WebClient mWebClient;
        private System.Net.WebClient mThumbnailClient;

        private String mTitle;
        private String mThumbnailPath;
        private String mDownloadDestination;
        private String mConversionDestination;
        private String mConversionTempInPath;
        private String mConversionTempOutPath;

        private System.Diagnostics.Process mPlugin;

        private long    mVideoFileSize;
        private double  mVideoFileSizeInMB;
        private String  mVideoFileSizeString;

        private double  mDuration;
        private int     mPercentage;

        private bool    mSilentMode;
        private bool    mIsReadyForNextOuery;
        private String  mPlaylistDestination;

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

            ( (CustomComboBox) mConversion ).BuildImageList();


            mTitle = SaveMedia.Program.Title + " " + SaveMedia.Program.TitleVersion;
            this.Text = mTitle;

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

            mDuration = 0;
            mPercentage = 0;
            mSilentMode = false;
            mIsReadyForNextOuery = true;
        }

        private void mOkButton_Click( object sender, EventArgs e )
        {
            ChangeLayout( "OK clicked" );
            ClearTemporaryFiles();
        }

        private void mCancelButton_Click( object sender, EventArgs e )
        {
            mThumbnailClient.CancelAsync();
            mWebClient.CancelAsync();
        }        

        private void mDownloadButton_Click( object sender, EventArgs e )
        {            
            InputEnabled( false );
            ClearTemporaryFiles();

            Uri theUrl;
            bool isValid = Uri.TryCreate( mUrl.Text, UriKind.Absolute, out theUrl );

            if( !isValid )
            {
                ShowStatus( "Unsupported URL" );
                InputEnabled( true );
                return;
            }

            if( theUrl.OriginalString.StartsWith( "http://www.youtube.com" ) )
            {
                DownloadYouTubeHelper( ref theUrl );
            }
            else if( theUrl.OriginalString.StartsWith( "http://www.tudou.com" ) )
            {
                DownloadTudouVideo( ref theUrl );
            }
            else if( theUrl.OriginalString.StartsWith( "http://www.newgrounds.com" ) )
            {
                DownloadNewgroundsVideo( ref theUrl );
            }
            else if( theUrl.OriginalString.StartsWith( "http://vimeo.com" ) )
            {
                DownloadVimeoVideo( ref theUrl );
            }
            else
            {
                if( mConversion.SelectedIndex == 0 )
                {
                    InputEnabled( true );
                    return;
                }
                else if( !String.IsNullOrEmpty( mUrl.Text ) &&
                         System.IO.File.Exists( mUrl.Text ) )
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

            if( isSuccess )
            {
                System.IO.File.Delete( mConversionDestination );
                System.IO.File.Move( mConversionTempOutPath, mConversionDestination );
                ChangeLayout( "Conversion completed" );
            }
            else
            {
                ChangeLayout( "Conversion failed" );
            }

            mPlugin.Close();
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

        private void DownloadYouTubeHelper( ref Uri aUrl )
        {
            if( aUrl.OriginalString.StartsWith( "http://www.youtube.com/view_play_list?" ) )
            {
                int thePageNumber = 1;
                DownloadYouTubePlaylist( ref aUrl, thePageNumber );
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
            if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
            {
                ShowStatus( "Failed to connect to " + aUrl.Host );
                InputEnabled( true );
                return;
            }

            String thePlaylistTitle;
            if( !Utilities.StringBetween( theSourceCode, "<h1>", "</h1>", out thePlaylistTitle ) )
            {
                ShowStatus( "Failed to analyze playlist" );
                InputEnabled( true );
                return;
            }

            String theUrlPattern = "<div\\s+class=\"video-short-title\"\\s*>.*\n.+\n.+href=\"(.*)\"\\s+title";
            Match theUrlMatch = Regex.Match( theSourceCode, theUrlPattern );

            if( !theUrlMatch.Success )
            {
                ShowStatus( "Failed to analyze playlist" );
                InputEnabled( true );
                return;
            }

            if( !mSilentMode )
            {
                FolderBrowserDialog theDialog = new FolderBrowserDialog();
                theDialog.Description = "Please select the destination for videos from:\n\n" + thePlaylistTitle;
                if( theDialog.ShowDialog() != DialogResult.OK )
                {
                    ChangeLayout( "Cancel clicked" );
                    ClearTemporaryFiles();
                    return;
                }
                mPlaylistDestination = theDialog.SelectedPath;
            }

            mSilentMode = true;            

            while( theUrlMatch.Success )
            {
                String thePartialUrl = theUrlMatch.Groups[ 1 ].ToString();
                Uri theVideoUrl = new Uri( "http://" + aUrl.Host + thePartialUrl );

                mIsReadyForNextOuery = false;
                DownloadYouTubeVideo( ref theVideoUrl );

                while( !mIsReadyForNextOuery )
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                if( !mSilentMode )
                {
                    return;
                }

                theUrlMatch = theUrlMatch.NextMatch();
            }

            int theCurrentPageIndex = theSourceCode.IndexOf( "class=\"pagerCurrent\"" );
            if( theCurrentPageIndex != -1 )
            {
                int theNextPageIndex = theSourceCode.LastIndexOf( "class=\"pagerNotCurrent\"" );

                if( theNextPageIndex > theCurrentPageIndex )
                {
                    int theNextPageNumber = Convert.ToInt32( thePageNumber ) + 1;
                    Uri theNextPage = new Uri( "http://www.youtube.com/view_play_list?p=" + thePlaylistId +
                                               "&page=" + theNextPageNumber );
                    DownloadYouTubePlaylist( ref theNextPage, theNextPageNumber );
                    return;
                }
            }

            mSilentMode = false;
            ChangeLayout( "Download completed" );
        }

        private void DownloadYouTubeVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            String theSourceCode;
            if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
            {
                ShowStatus( "Failed to connect to " + aUrl.Host );
                InputEnabled( true );
                return;
            }

            String thePartialUrl;
            if( !Utilities.StringBetween( theSourceCode, "/watch_fullscreen?", "';", out thePartialUrl ) )
            {
                ShowStatus( "Failed to analyze video's URL" );
                InputEnabled( true );
                return;
            }
            Uri theFullScreenUrl = new Uri( "http://" + aUrl.Host + "/watch_fullscreen?" + thePartialUrl );

            System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( theFullScreenUrl.Query );
            
            String theVideoTitle = theQueryStrings[ "title" ];
            String theVideoId    = theQueryStrings[ "video_id" ];
            String theToken      = theQueryStrings[ "t" ];
            String theFmtMap     = theQueryStrings[ "fmt_map" ];

            String thePreferedQuality = Settings.YouTubeQuality();
            String theQuality = Utilities.YouTubeAvailableQuality( theFmtMap, thePreferedQuality );

            Uri theThumbnailUrl = new Uri( "http://img.youtube.com/vi/" + theVideoId + "/default.jpg" );
            Uri theVideoUrl     = new Uri( "http://www.youtube.com/get_video?" +
                                           "video_id=" + theVideoId + 
                                           "&t="       + theToken +
                                           theQuality );

            DownloadThumbnail( ref theThumbnailUrl );
            DisplayMediaInfo( theVideoTitle );

            String theFilename = theVideoTitle;
            String theFilePath;

            if( mSilentMode )
            {
                theFilePath = mPlaylistDestination + "\\" + Utilities.FilenameCheck( theVideoTitle ) + ".flv";
            }
            else
            {
                theFilePath = Utilities.SaveFile( theFilename, "Flash Video (*.flv)|*.flv" );
            }

            DownloadFile( theVideoUrl, theFilePath );
        }

        private void DownloadTudouVideo( ref Uri aUrl )
        {           
            ShowStatus( "Connecting to " + aUrl.Host );
            System.Windows.Forms.Application.UseWaitCursor = true;
            
            String theVideoId = String.Empty;
            
            if( aUrl.OriginalString.ToLower().Contains( "iid=" ) )
            {
                // case 1 -> http://www.tudou.com/playlist/playindex.do?lid=3183353&iid=16030830
                System.Collections.Specialized.NameValueCollection theQueryStrings = System.Web.HttpUtility.ParseQueryString( aUrl.Query );
                theVideoId = theQueryStrings[ "iid" ];
            }
            else
            {
                // case 2 -> http://www.tudou.com/programs/view/vjrv4umAuSk/
                
                String theSourceCode;
                if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
                {
                    ShowStatus( "Failed to connect to " + aUrl.Host );
                    InputEnabled( true );
                    return;
                }
                
                if( !Utilities.StringBetween( theSourceCode, "iid=", "\"", out theVideoId ) )
                {
                    ShowStatus( "Failed to analyze video's URL" );
                    InputEnabled( true );
                    return;
                }
            }

            System.Windows.Forms.Application.UseWaitCursor = false;

            Uri theXmlUrl = new Uri( "http://" + aUrl.Host + "/player/v.php?id=" + theVideoId );
            String theXmlSource = String.Empty;

            if( !Utilities.DownloadString( theXmlUrl, out theXmlSource ) ||
                theXmlSource.Equals( "0" ) )
            {
                ShowStatus( "Video is not found" );
                InputEnabled( true );
                return;
            }

            String theVideoTitle;
            if( !Utilities.StringBetween( theXmlSource, "q='", "'", out theVideoTitle ) )
            {
                ShowStatus( "Failed to read video's title" );
                InputEnabled( true );
                return;
            }

            // this XML file provide the title as well, however the characters are encoded
            // they show up as &#27931;&#22855; &#34909;&#25758;&#36305;&#20301;
            theXmlUrl = new Uri( "http://v2.tudou.com/v2/cdn?id=" + theVideoId );

            if( !Utilities.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                ShowStatus( "Video is not found" );
                InputEnabled( true );
                return;
            }

            // <v time="74000" vi="1" ch="10" nls="0" title="洛奇 衝撞跑位" code="vjrv4umAuSk" enable="1" logo="0" band="0"><a/><b><f w="10" h="0" sha1="5c7b142105e6db7dc7e342b65fda12413c0cb975" size="1736164">http://123.129.251.201/flv/014/770/632/14770632.flv?key=7b83d716aa9581bd59145e49e26fee2422d102</f></b></v>
            String thePartialUrlString;
            if( !Utilities.StringBetween( theXmlSource, "<f", "</f>", out thePartialUrlString ) )
            {
                ShowStatus( "Failed to read video's URL" );
                InputEnabled( true );
                return;
            }

            String theVideoUrlString = thePartialUrlString.Substring( thePartialUrlString.IndexOf( ">" ) + 1 );
            
            // this part was required for AutoIt
            //String theVideoFileSizeString;
            //if( !Utilities.StringBetween( theXmlSource, "size=\"", "\"", out theVideoFileSizeString ) )
            //{
            //    ShowStatus( "Failed to read video's size" );
            //    InputEnabled( true );
            //    return;
            //}

            String theThumbnailUrlString;
            if( !Utilities.StringBetween( thePartialUrlString, "/flv/", ".flv", out theThumbnailUrlString ) )
            {
                ShowStatus( "Failed to read video's URL" );
                InputEnabled( true );
                return;
            }

            int theLastSlashIndex = theThumbnailUrlString.LastIndexOf( "/" );
            if( theLastSlashIndex == -1 )
            {
                ShowStatus( "Failed to read video's URL" );
                InputEnabled( true );
                return;
            }

            theThumbnailUrlString = "http://i01.img.tudou.com/data/imgs/i/" + theThumbnailUrlString.Remove( theLastSlashIndex );
            theThumbnailUrlString += "/p.jpg";
            
            Uri theThumbnailUrl = new Uri( theThumbnailUrlString );
            Uri theVideoUrl     = new Uri( theVideoUrlString );            

            DownloadThumbnail( ref theThumbnailUrl );
            DisplayMediaInfo( theVideoTitle );

            String theFilename = theVideoTitle;
            String theFilePath = Utilities.SaveFile( theFilename, "Flash Video (*.flv)|*.flv" );

            DownloadFile( theVideoUrl, theFilePath );
        }        

        private void DownloadNewgroundsVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            String theSourceCode;
            if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
            {
                ShowStatus( "Failed to connect to " + aUrl.Host );
                InputEnabled( true );
                return;
            }

            String theVideoTitle;
            if( !Utilities.StringBetween( theSourceCode, "<title>", "</title>", out theVideoTitle ) )
            {
                ShowStatus( "Failed to read video's title" );
                InputEnabled( true );
                return;
            }

            String theVideoUrlString;
            if( !Utilities.StringBetween( theSourceCode, "var fw = new FlashWriter(\"", "\"", out theVideoUrlString ) )
            {
                ShowStatus( "Failed to read video's URL" );
                InputEnabled( true );
                return;
            }

            String theThumbnailUrlString;
            if( !Utilities.StringBetween( theSourceCode, "<link rel=\"image_src\" href=\"", "\"", out theThumbnailUrlString ) )
            {
                ShowStatus( "Failed to read video's thumbnail" );
                InputEnabled( true );
                return;
            }

            Uri theThumbnailUrl = new Uri( theThumbnailUrlString );
            Uri theVideoUrl     = new Uri( theVideoUrlString );

            DownloadThumbnail( ref theThumbnailUrl );
            DisplayMediaInfo( theVideoTitle );

            String theFilename = theVideoTitle;
            String theFilePath = Utilities.SaveFile( theFilename, "Flash Movie (*.swf)|*.swf" );

            DownloadFile( theVideoUrl, theFilePath );
        }

        private void DownloadVimeoVideo( ref Uri aUrl )
        {
            ShowStatus( "Connecting to " + aUrl.Host );

            String theSourceCode;
            if( !Utilities.DownloadString( aUrl, out theSourceCode ) )
            {
                ShowStatus( "Failed to connect to " + aUrl.Host );
                InputEnabled( true );
                return;
            }

            String theVideoId;
            if( !Utilities.StringBetween( theSourceCode, "http://vimeo.com/moogaloop.swf?clip_id=", "\"", out theVideoId ) )
            {
                ShowStatus( "Failed to read video's ID" );
                InputEnabled( true );
                return;
            }

            String theThumbnailUrlString;
            if( !Utilities.StringBetween( theSourceCode, "<link rel=\"videothumbnail\" href=\"", "\"", out theThumbnailUrlString ) )
            {
                ShowStatus( "Failed to read video's thumbnail" );
                InputEnabled( true );
                return;
            }

            Uri theXmlUrl = new Uri( "http://vimeo.com/moogaloop/load/clip:" + theVideoId + "/local?param_clip_id=" + theVideoId );
            
            String theXmlSource;
            if( !Utilities.DownloadString( theXmlUrl, out theXmlSource ) )
            {
                ShowStatus( "Video is not found" );
                InputEnabled( true );
                return;
            }

            String theVideoTitle;
            if( !Utilities.StringBetween( theXmlSource, "<caption>", "</caption>", out theVideoTitle ) )
            {
                ShowStatus( "Failed to read video's title" );
                InputEnabled( true );
                return;
            }

            String theRequestSignature;
            if( !Utilities.StringBetween( theXmlSource, "<request_signature>", "</request_signature>", out theRequestSignature ) )
            {
                ShowStatus( "Failed to read video's signature" );
                InputEnabled( true );
                return;
            }

            String theRequestSignatureExpires;
            if( !Utilities.StringBetween( theXmlSource, "<request_signature_expires>", "</request_signature_expires>", out theRequestSignatureExpires ) )
            {
                ShowStatus( "Failed to read video's signature" );
                InputEnabled( true );
                return;
            }

            String theVideoUrlString = "http://vimeo.com/moogaloop/play/clip:" + theVideoId + "/" + theRequestSignature + "/" + theRequestSignatureExpires + "/?q=sd&type=local";

            Uri theThumbnailUrl = new Uri( theThumbnailUrlString );
            Uri theVideoUrl     = new Uri( theVideoUrlString );

            DownloadThumbnail( ref theThumbnailUrl );
            DisplayMediaInfo( theVideoTitle );

            String theFilename = theVideoTitle;
            String theFilePath = Utilities.SaveFile( theFilename, "Flash Video (*.flv)|*.flv" );

            DownloadFile( theVideoUrl, theFilePath );
        }

        private void DisplayMediaInfo( String aMediaTitle )
        {
            mVideoFileSize = 0;
            mVideoFileSizeString = "??? MB";

            mTitleLabel.Text = "Title: " + aMediaTitle.Replace( "\\", "" );
            mSizeLabel.Text = "Size: " + mVideoFileSizeString;

            mLocationLabel.Text = String.Empty;
            mProgressBar.Value = 0;

            ShowStatus( "Ready to download" );
            MediaInfoVisible( true );
        }

        private void DownloadThumbnail( ref Uri aUrl )
        {
            mThumbnailPath = System.IO.Path.GetTempFileName();
            mThumbnailClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
            mThumbnailClient.DownloadFileAsync( aUrl, mThumbnailPath );
        }

        private void DownloadFile( Uri aUrl, String aDestination )
        {
            if( String.IsNullOrEmpty( aDestination ) )
            {
                ChangeLayout( "Cancel clicked" );
                ClearTemporaryFiles();
                return;
            }

            mDownloadDestination = aDestination;

            mWebClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
            mWebClient.DownloadFileAsync( aUrl, mDownloadDestination );

            ShowStatus( "Downloading..." );
            //mLocationLabel.Text = "Location: " + mDownloadDestination;

            mDownloadButton.Visible = false;
            mCancelButton.Visible = true;
        }

        private void DownloadProgressChanged( object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            if( mVideoFileSize == 0 )
            {
                mVideoFileSize = e.TotalBytesToReceive;
                mVideoFileSizeInMB = mVideoFileSize;
                mVideoFileSizeInMB /= gcOneMB;
                mVideoFileSizeString = mVideoFileSizeInMB.ToString( "0.000" ) + " MB";
                mSizeLabel.Text = "Size: " + mVideoFileSizeString;
            }

            this.Text = e.ProgressPercentage + "% - " + mTitle;
            mProgressBar.Value = e.ProgressPercentage;

            double theReceivedFileSizeInMB = e.BytesReceived;
            theReceivedFileSizeInMB /= gcOneMB;
            ShowStatus( "Downloading..." +
                        theReceivedFileSizeInMB.ToString( "0.000" ) +
                        "/" +
                        mVideoFileSizeString );
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
            else if( mConversion.SelectedIndex == 0 )
            {
                ChangeLayout( "Download completed" );
            }
            else
            {
                ConvertFile( mDownloadDestination, mDownloadDestination );
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

            if( mSilentMode )
            {
                switch( aPhase )
                {
                    case "Cancel clicked":

                        mSilentMode = false;
                        mIsReadyForNextOuery = true;
                        break;

                    case "Converting...":
                        break;

                    case "Conversion completed":

                        mIsReadyForNextOuery = true;
                        return;

                    case "Conversion failed":
                    case "Conversion failed, plug-in not found":

                        mSilentMode = false;
                        mIsReadyForNextOuery = true;
                        break;

                    case "Downloading":
                        break;

                    case "Download completed":

                        mIsReadyForNextOuery = true;
                        return;

                    case "Download cancelled":
                    case "Download failed":

                        mSilentMode = false;
                        mIsReadyForNextOuery = true;
                        break;

                    case "OK clicked":

                        mSilentMode = false;
                        mIsReadyForNextOuery = true;
                        break;
                }
            }

            this.SuspendLayout();

            switch( aPhase )
            {
                case "Cancel clicked":

                    this.Text = mTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    ShowStatus( "Download cancelled" );

                    MediaInfoVisible( false );
                    InputEnabled( true );
                    break;

                case "Converting...":
             
                    this.Text = mPercentage.ToString() + "% - " + mTitle;
                    mProgressBar.Value = mPercentage;

                    ShowStatus( aPhase + mPercentage.ToString() + "%" );
                    break;

                case "Conversion completed":
                case "Conversion failed":
                case "Conversion failed, plug-in not found":
            
                    this.Text = mTitle;
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

                case "Download completed":

                    this.Text = mTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;

                    ShowStatus( aPhase );

                    NotifyUser();
                    break;

                case "Download cancelled":

                    this.Text = mTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    ShowStatus( aPhase );

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;
                    break;

                case "Download failed":

                    this.Text = mTitle;
                    mProgressBar.Value = mProgressBar.Maximum;

                    ShowStatus( aPhase );

                    mDownloadButton.Visible = false;
                    mOkButton.Visible = true;
                    mCancelButton.Visible = false;
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
                String theNewUrl = Utilities.ReadClipboardUrl();
                if( !mUrl.Text.Equals( theNewUrl ) && !String.IsNullOrEmpty( theNewUrl ) )
                {
                    mUrl.Text = Utilities.ReadClipboardUrl();
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
                FlashWindow.Flash( this );
            }
        }

        private void mAboutToolStripMenuItem_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Form theForm = new AboutBox();
            theForm.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Form theForm = new OptionsForm();
            theForm.ShowDialog();
        }

        private void MainForm_Activated( object sender, EventArgs e )
        {
            String theNewUrl = Utilities.ReadClipboardUrl();
            if( mUrl.Enabled && !mUrl.Text.Equals( theNewUrl ) && !String.IsNullOrEmpty( theNewUrl ) )
            {
                mUrl.Text = Utilities.ReadClipboardUrl();
            }
        }
    }
}