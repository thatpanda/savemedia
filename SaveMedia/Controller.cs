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
    public class Controller
    {
        private IMainForm mUI = null;

        private const String gcFFmpegPath = "Plugin\\bin\\ffmpeg.exe";
        private const double gcOneKB = 1024;
        private const double gcOneMB = gcOneKB * 1024;

        private System.Net.WebClient mWebClient;
        private System.Net.WebClient mThumbnailClient;

        private String mThumbnailPath;
        private String mDownloadDestination;
        private String mConversionDestination;
        private String mConversionTempInPath;
        private String mConversionTempOutPath;

        private System.Diagnostics.Process mPlugin;

        private long    mFileSize;
        private double  mFileSizeInMB;
        private String  mFileSizeString;
        private double  mBytesReceived;

        private double  mDuration;

        private String  mPlaylistDestination;

        private int     mWaitingTime;
        private Timer   mDelayTimer;

        private System.Collections.Generic.List< DownloadTag > mDownloadQueue;
        private System.Collections.Generic.List< String > mConvertQueue;

        public Controller( IMainForm aUI )
        {
            mUI = aUI;

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

            mDelayTimer = new Timer();
            mDelayTimer.Interval = 1000;
            mDelayTimer.Tick += new EventHandler( mDelayTimer_Tick );

            mDownloadQueue = new List<DownloadTag>();
            mConvertQueue = new List<String>();

            UpdateUtils.StartupCheckIfNeeded( mUI );
        }

        public bool ConverterExists
        {
            get{ return System.IO.File.Exists( this.ConverterPath ); }
        }

        public String ConverterPath
        {
            get{ return gcFFmpegPath; }
        }

        private void mDelayTimer_Tick( object sender, EventArgs e )
        {
            --mWaitingTime;

            if( mWaitingTime <= 0 )
            {
                mDelayTimer.Stop();

                if( mDownloadQueue.Count != 0 )
                {
                    StartDownload();
                }
                else
                {
                    mUI.ChangeLayout( "Download failed" );
                }
            }
            else
            {
                mUI.StatusMessage = "Download begins in " + mWaitingTime + " secs";
            }
        }

        public void Abort()
        {
            if( mWaitingTime != 0 )
            {
                mWaitingTime = 0;
                mDelayTimer.Stop();
                mUI.ChangeLayout( "Download cancelled" );
            }
            else if( mPlugin != null )
            {
                mPlugin.Kill();
                mUI.ChangeLayout( "Conversion cancelled" );
            }
            else
            {
                mThumbnailClient.CancelAsync();
                mWebClient.CancelAsync();
            }
        }

        public void ParseUrl( String aUrl )
        {

            Uri theUrl;
            bool isValid = Uri.TryCreate( aUrl, UriKind.Absolute, out theUrl );

            if( !isValid )
            {
                mUI.StatusMessage = "Unsupported URL";
                mUI.InputEnabled = true;
                return;
            }
			
			mDownloadQueue.Clear();
            mConvertQueue.Clear();
            mPlaylistDestination = String.Empty;

            ParseUrl( ref theUrl );
        }

        public void ParseUrl( ref Uri aUrl )
        {
            if( aUrl.OriginalString.StartsWith( "http://" ) )
            {
                if( aUrl.OriginalString.StartsWith( "http://www.youtube.com" ) )
                {
                    YouTubeVideoParser( ref aUrl );
                }
                else if( aUrl.Host.EndsWith( ".rapidshare.com" ) )
                {
                    //DownloadRapidShareFile( ref aUrl );
                    mUI.StatusMessage = "Sorry, this site is not supported";
                    mUI.InputEnabled = true;
                    return;
                }
                else
                {
                    mUI.StatusMessage = "Connecting to " + aUrl.Host;

                    String theError;

                    if( aUrl.OriginalString.StartsWith( "http://www.tudou.com" ) )
                    {
                        //Sites.Tudou.TryParse( ref aUrl, ref mDownloadQueue, out theError );
                        mUI.StatusMessage = "Sorry, this site is no longer supported";
                        mUI.InputEnabled = true;
                        return;
                    }
                    else if( aUrl.OriginalString.StartsWith( "http://v.youku.com" ) )
                    {
                        Sites.Youku.TryParse( ref aUrl, ref mDownloadQueue, out theError );
                    }
                    else if( aUrl.OriginalString.StartsWith( "http://www.newgrounds.com" ) )
                    {
                        Sites.NewGrounds.TryParse( ref aUrl, ref mDownloadQueue, out theError );
                    }
                    else if( aUrl.OriginalString.StartsWith( "http://vimeo.com" ) ||
                             aUrl.OriginalString.StartsWith( "http://www.vimeo.com" ) )
                    {
                        Sites.Vimeo.TryParse( ref aUrl, ref mDownloadQueue, out theError );
                    }
                    else if( aUrl.OriginalString.StartsWith( "http://www.collegehumor.com" ) )
                    {
                        Sites.CollegeHumor.TryParse( ref aUrl, ref mDownloadQueue, out theError );
                    }
                    else if( aUrl.OriginalString.StartsWith( "http://link.brightcove.com" ) )
                    {
                        //DownloadBrightcoveVideo( ref theUrl );
                        mUI.StatusMessage = "Sorry, this site is not supported yet";
                        mUI.InputEnabled = true;
                        return;
                    }
                    else
                    {
                        String theFilename = System.IO.Path.GetFileName( aUrl.OriginalString );
                        String theFileExt = System.IO.Path.GetExtension( aUrl.OriginalString );
                        String theFilePath = FileUtils.SaveFile( theFilename, theFileExt + "|*" + theFileExt, mUI.Win32Window );

                        DownloadFile( aUrl, theFilePath );
                        return;
                    }

                    if( !String.IsNullOrEmpty( theError ) )
                    {
                        mUI.StatusMessage = theError;
                        mUI.InputEnabled = true;
                        return;
                    }

                    StartDownload();
                }
            }
            else
            {
                if( mUI.ConversionComboBox.SelectedIndex == 0 )
                {
                    mUI.InputEnabled = true;
                    return;
                }
                else if( !String.IsNullOrEmpty( aUrl.OriginalString ) &&
                         System.IO.File.Exists( aUrl.OriginalString ) )
                {
                    ConvertFile( aUrl.OriginalString, aUrl.OriginalString );
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

            if( mConvertQueue.Count != 0 )
            {
                String thePath = mConvertQueue[ 0 ];
                mConvertQueue.RemoveAt( 0 );
                ConvertFile( thePath, thePath );
            }
            else if( isSuccess )
            {
                mUI.ChangeLayout( "Conversion completed" );
            }
            else
            {
                mUI.ChangeLayout( "Conversion failed" );
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
                        int theConversionPercentage = (int) ( theProgress / mDuration * 100 );
                        theConversionPercentage = Math.Min( theConversionPercentage, 100 );

                        mUI.ConversionProgress = theConversionPercentage;
                    }
                }
            }
        }

        private void DownloadRapidShareFile( ref Uri aUrl )
        {
            mDelayTimer.Stop();

            mUI.StatusMessage = "Connecting to " + aUrl.Host;

            DownloadTag theTag;

            Sites.RapidShare.TryParse( ref aUrl, out theTag );

            if( !String.IsNullOrEmpty( theTag.Error ) )
            {
                mUI.StatusMessage = theTag.Error;
                mUI.InputEnabled = true;
                return;
            }

            mWaitingTime = theTag.WaitingTime;
            mDelayTimer.Start();

            theTag.DownloadDestination = FileUtils.SaveFile( theTag.FileName, theTag.FileExtension + "|*" + theTag.FileExtension, mUI.Win32Window );

            if( String.IsNullOrEmpty( theTag.DownloadDestination ) )
            {
                mUI.ChangeLayout( "Cancel clicked" );
                mDelayTimer.Stop();
                ClearTemporaryFiles();
                return;
            }

            mDownloadQueue.Add( theTag );

            //mDownloadButton.Visible = false;
            //mCancelButton.Visible = true;
        }

        private void YouTubeVideoParser( ref Uri aUrl )
        {
            if( aUrl.OriginalString.StartsWith( "http://www.youtube.com/view_play_list?" ) )
            {
                int thePageNumber = 1;
                DownloadYouTubePlaylist( ref aUrl, thePageNumber );
            }
            else if( aUrl.OriginalString.StartsWith( "http://www.youtube.com/user/" ) )
            {
                int theLastSlashIndex = aUrl.OriginalString.LastIndexOf( "/" );

                if( theLastSlashIndex == -1 )
                {
                    DownloadYouTubeVideo( ref aUrl );
                }
                else
                {
                    String thePlaylistId = aUrl.OriginalString.Substring( theLastSlashIndex + 1 );
                    Uri thePlaylistUrl = new Uri( "http://www.youtube.com/view_play_list?p=" + thePlaylistId );
                    YouTubeVideoParser( ref thePlaylistUrl );
                }
            }
            else if( aUrl.OriginalString.StartsWith( "http://www.youtube.com/v/" ) )
            {
                String theNewUrlString = aUrl.OriginalString;
                theNewUrlString = theNewUrlString.Replace( "http://www.youtube.com/v/", "http://www.youtube.com/watch?v=" );

                Uri theNewUrl = new Uri( theNewUrlString );
                ParseUrl( ref theNewUrl );
            }
            else
            {
                DownloadYouTubeVideo( ref aUrl );
            }
        }

        private void DownloadYouTubePlaylist( ref Uri aUrl, int aPageNumber )
        {
            mUI.StatusMessage = "Connecting to " + aUrl.Host;

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
                mUI.StatusMessage = "Failed to connect to " + aUrl.Host;
                mUI.InputEnabled = true;
                return;
            }

            String thePlaylistTitle;
            if( !StringUtils.StringBetween( theSourceCode, "<h1>", "</h1>", out thePlaylistTitle ) )
            {
                mUI.StatusMessage = "Failed to analyze playlist";
                mUI.InputEnabled = true;
                return;
            }

            // http://msdn.microsoft.com/en-us/library/az24scfc.aspx
            String theUrlPattern = "<div\\s+class=\"video-short-title\"\\s*>(\\s|\n)+<a\\s.*href=\"([^\"]+)\"";
            Match theUrlMatch = Regex.Match( theSourceCode, theUrlPattern );

            if( !theUrlMatch.Success )
            {
                mUI.StatusMessage = "Failed to analyze playlist";
                mUI.InputEnabled = true;
                return;
            }

            if( String.IsNullOrEmpty( mPlaylistDestination ) )
            {
                FolderBrowserDialog theDialog = new FolderBrowserDialog();
                theDialog.Description = "Please select the destination for videos from:\n\n" + thePlaylistTitle;
                if( theDialog.ShowDialog( mUI.Win32Window ) != DialogResult.OK )
                {
                    mUI.ChangeLayout( "Cancel clicked" );
                    ClearTemporaryFiles();
                    return;
                }
                mPlaylistDestination = theDialog.SelectedPath;
            }

            while( theUrlMatch.Success )
            {
                String thePartialUrl = theUrlMatch.Groups[ 2 ].ToString();
                Uri theVideoUrl = new Uri( "http://" + aUrl.Host + thePartialUrl );

                String theError;

                Sites.YouTube.TryParse( ref theVideoUrl, ref mDownloadQueue, out theError );

                if( !String.IsNullOrEmpty( theError ) )
                {
                    mUI.StatusMessage = theError;
                    mUI.InputEnabled = true;
                    return;
                }

                DownloadTag theTag = mDownloadQueue[ mDownloadQueue.Count - 1 ];
                theTag.DownloadDestination = mPlaylistDestination + "\\" + FileUtils.FilenameCheck( theTag.FileName ) + ".flv";

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
            else
            {
                StartDownload();
            }
        }

        private void DownloadYouTubeVideo( ref Uri aUrl )
        {
            mUI.StatusMessage = "Connecting to " + aUrl.Host;

            String theError;

            Sites.YouTube.TryParse( ref aUrl, ref mDownloadQueue, out theError );

            if( !String.IsNullOrEmpty( theError ) )
            {
                mUI.StatusMessage = theError;
                mUI.InputEnabled = true;
                return;
            }

            StartDownload();
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

        private void StartDownload()
        {
            if( mDownloadQueue.Count != 0 )
            {
                DownloadTag theTag = mDownloadQueue[ 0 ];
                mDownloadQueue.RemoveAt( 0 );
                DownloadFile( theTag );
            }
        }

        private void DownloadFile( DownloadTag aTag )
        {
            DownloadThumbnail( aTag.ThumbnailUrl );
            mUI.DisplayMediaInfo( aTag );

            if( String.IsNullOrEmpty( aTag.DownloadDestination ) )
            {
                aTag.DownloadDestination = FileUtils.SaveFile( aTag.FileName, aTag.FileExtension, mUI.Win32Window );
            }

            DownloadFile( aTag.VideoUrl, aTag.DownloadDestination );
        }

        public void DownloadFile( Uri aUrl, String aDestination )
        {
            if( String.IsNullOrEmpty( aDestination ) )
            {
                mUI.ChangeLayout( "Cancel clicked" );
                ClearTemporaryFiles();
                return;
            }

            mFileSize = 0;
            mFileSizeString = "??? MB";

            mDownloadDestination = aDestination;

            mWebClient.Headers.Add( "user-agent", SaveMedia.Program.UserAgent );
            mWebClient.DownloadFileAsync( aUrl, mDownloadDestination );

            mUI.DownloadStarted( aDestination );
        }

        private void DownloadProgressChanged( object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            if( mFileSize == 0 )
            {
                mFileSize = e.TotalBytesToReceive;
                mFileSizeInMB = mFileSize;
                mFileSizeInMB /= gcOneMB;
                mFileSizeString = mFileSizeInMB.ToString( "0.00" ) + " MB";
                mUI.FileSize( mFileSizeString );
                mBytesReceived = 0;
            }

            if( ( e.BytesReceived - mBytesReceived ) / ( 20 * gcOneKB ) >= 1 )
            {
                mBytesReceived = e.BytesReceived;

                mUI.DownloadProgress = e.ProgressPercentage;

                double theReceivedFileSizeInMB = e.BytesReceived;
                theReceivedFileSizeInMB /= gcOneMB;
                mUI.StatusMessage = "Downloading..." +
                                    theReceivedFileSizeInMB.ToString( "0.00" ) +
                                    "/" +
                                    mFileSizeString;
            }
        }

        private void DownloadCompleted( object sender, System.ComponentModel.AsyncCompletedEventArgs e )
        {
            if( e.Cancelled )
            {
                mUI.ChangeLayout( "Download cancelled" );
            }
            else if( e.Error != null )
            {
                mUI.ChangeLayout( "Download failed" );
                mUI.StatusMessage = e.Error.Message;
            }
            else
            {
                if( mUI.ConversionComboBox.SelectedIndex != 0 )
                {
                    mConvertQueue.Add( mDownloadDestination );
                }

                if( mDownloadQueue.Count != 0 )
                {
                    DownloadTag theTag = mDownloadQueue[ 0 ];
                    mDownloadQueue.RemoveAt( 0 );
                    DownloadFile( theTag );
                }
                else if( mConvertQueue.Count != 0 )
                {
                    String thePath = mConvertQueue[ 0 ];
                    mConvertQueue.RemoveAt( 0 );
                    ConvertFile( thePath, thePath );
                }
                else
                {
                    mUI.ChangeLayout( "Download completed" );
                }
            }
        }

        private void ConvertFile( String aSource, String aDestination )
        {
            if( mUI.ConversionComboBox.SelectedIndex == 0 )
            {
                return;
            }

            if( !this.ConverterExists )
            {
                mUI.ChangeLayout( "Conversion failed, plug-in not found" );
                return;
            }

            mConversionDestination = aDestination;

            String theExtension = String.Empty;
            String theArguments = String.Empty;

            if( mUI.ConversionComboBox.SelectedIndex == 1 )
            {
                theExtension = ".mp3";
                theArguments = "-y -i \"{0}\" -ar 44100 -ab 192k -ac 2 \"{1}\"";
            }
            else if( mUI.ConversionComboBox.SelectedIndex == 2 )
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

            mUI.ConvertStarted();
        }

        private void ThumbnailDownloadCompleted( object sender, System.ComponentModel.AsyncCompletedEventArgs e )
        {
            if( !e.Cancelled && e.Error == null )
            {
                mUI.ThumbnailPath = mThumbnailPath;
            }
        }

        public void ClearTemporaryFiles()
        {
            FileUtils.DeleteFile( mThumbnailPath );
            FileUtils.DeleteFile( mConversionTempInPath );
            FileUtils.DeleteFile( mConversionTempOutPath );
        }
    }
}