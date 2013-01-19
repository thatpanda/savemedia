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

        private int     mWaitingTime;
        private Timer   mDelayTimer;

        private System.Collections.Generic.List< Sites.ISite > mSupportedSites;
        private System.Collections.Generic.List< DownloadTag > mDownloadQueue;
        private System.Collections.Generic.List< String > mConvertQueue;

        public Controller()
        {
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

            mSupportedSites = new List<Sites.ISite>();
            mSupportedSites.Add( new Sites.YouTube() );
            mSupportedSites.Add( new Sites.Youku() );
            mSupportedSites.Add( new Sites.Vimeo() );

            mDownloadQueue = new List< DownloadTag >();
            mConvertQueue = new List< String >();
        }

        public void Initialize( IMainForm aUI )
        {
            mUI = aUI;

            if( this.ConverterExists )
            {
                // TODO: configurable conversions
                mUI.Initialize( this,
                                new ConverterTag( "Do not convert file",
                                                  String.Empty,
                                                  String.Empty ),
                                new ConverterTag( "MPEG-1 Audio Layer 3 (*.mp3)",
                                                  ".mp3",
                                                  "-y -i \"{0}\" -ar 44100 -ab 192k -ac 2 \"{1}\"" ),
                                new ConverterTag( "Windows Media Video (*.wmv)",
                                                  ".wmv",
                                                  "-y -i \"{0}\" -vcodec wmv2 -sameq -acodec mp2 -ar 44100 -ab 192k -f avi \"{1}\"" ) );
            }
            else
            {
                mUI.Initialize( this,
                                new ConverterTag( "Plug-in not found",
                                                  String.Empty,
                                                  String.Empty ) );
            }

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

            ParseUrl( ref theUrl );
        }

        private void ParseUrl( ref Uri aUrl )
        {
            foreach( Sites.ISite theSite in mSupportedSites )
            {
                if( theSite.Support( ref aUrl ) )
                {
                    mUI.StatusMessage = "Connecting to " + aUrl.Host;

                    String theError;
                    theSite.TryParse( ref aUrl, ref mDownloadQueue, ref mUI, out theError );

                    if( String.IsNullOrEmpty( theError ) )
                    {
                        StartDownload();
                    }
                    else
                    {
                        mUI.StatusMessage = theError;
                        mUI.InputEnabled = true;
                    }

                    return;
                }
            }

            if( aUrl.OriginalString.StartsWith( "http://" ) )
            {
                mUI.StatusMessage = "Connecting to " + aUrl.Host;

                String theFilename = System.IO.Path.GetFileName( aUrl.OriginalString );
                String theFileExt = System.IO.Path.GetExtension( aUrl.OriginalString );
                String theFilePath = FileUtils.SaveFile( theFilename, theFileExt + "|*" + theFileExt, mUI.Win32Window );

                DownloadFile( aUrl, theFilePath );
            }
            else if( mUI.SelectedConverter.IsValid &&
                     !String.IsNullOrEmpty( aUrl.OriginalString ) &&
                     System.IO.File.Exists( aUrl.OriginalString ) )
            {
                ConvertFile( aUrl.OriginalString, aUrl.OriginalString );
            }
            else
            {
                mUI.InputEnabled = true;
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
                if( mUI.SelectedConverter.IsValid )
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
            ConverterTag theConverter = mUI.SelectedConverter;
            if( !theConverter.IsValid )
            {
                return;
            }

            if( !this.ConverterExists )
            {
                mUI.ChangeLayout( "Conversion failed, plug-in not found" );
                return;
            }

            mConversionTempInPath = System.IO.Path.GetTempFileName();
            mConversionTempOutPath = System.IO.Path.GetTempFileName();

            mConversionDestination = System.IO.Path.ChangeExtension( aDestination, theConverter.FileExtension );
            mConversionTempOutPath = System.IO.Path.ChangeExtension( mConversionTempOutPath, theConverter.FileExtension );

            System.IO.File.Copy( aSource, mConversionTempInPath, true );

            System.Diagnostics.ProcessStartInfo theStartInfo = new System.Diagnostics.ProcessStartInfo( gcFFmpegPath );

            theStartInfo.Arguments = String.Format( theConverter.FFmpegArg, mConversionTempInPath, mConversionTempOutPath );
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