using System;

using Utility;

namespace SaveMedia
{
    static class Settings
    {
        private const String gcSettingsFileName = "Settings.xml";
        private static System.Collections.Generic.Dictionary< String, String > mSettings;

        public static String YouTubeQuality()
        {
            Load();

            if( !mSettings.ContainsKey( "YouTubeQuality" ) )
            {
                YouTubeQuality( "34" );
            }

            String theFmt = mSettings[ "YouTubeQuality" ];

            if( theFmt.Equals( "0", StringComparison.Ordinal ) )
            {
                YouTubeQuality( "34" );     // new default
            }
            else if( theFmt.Equals( "6", StringComparison.Ordinal ) )
            {
                YouTubeQuality( "35" );     // no more fmt 6
            }

            return mSettings[ "YouTubeQuality" ];
        }

        public static void YouTubeQuality( String aValue )
        {
            Set( "YouTubeQuality", aValue );
        }

        public static DateTime LastTimeCheckedForUpdates()
        {
            Load();

            DateTime theYesterday = DateTime.Today.AddDays( -1 );

            if( !mSettings.ContainsKey( "LastTimeCheckedForUpdates" ) )
            {
                LastTimeCheckedForUpdates( theYesterday );
            }

            DateTime theDate;
            if( DateTime.TryParse( mSettings[ "LastTimeCheckedForUpdates" ], out theDate ) )
            {
                return theDate;
            }

            return theYesterday;
        }

        public static void LastTimeCheckedForUpdates( DateTime aValue )
        {
            Set( "LastTimeCheckedForUpdates", aValue.ToShortDateString() );
            Save();
        }

        public static String CheckForUpdates()
        {
            Load();

            if( !mSettings.ContainsKey( "CheckForUpdates" ) )
            {
                CheckForUpdates( "auto" );
            }

            switch( mSettings[ "CheckForUpdates" ] )
            {
                case "auto":
                case "when fails":
                case "never":
                    break;
                default:
                    CheckForUpdates( "auto" );
                    break;
            }

            return mSettings[ "CheckForUpdates" ];
        }

        public static void CheckForUpdates( String aValue )
        {
            Set( "CheckForUpdates", aValue );
        }

        public static void Load()
        {
            if( mSettings == null )
            {
                mSettings = new System.Collections.Generic.Dictionary<String, String>();
            }

            mSettings = FileUtils.ReadXml( gcSettingsFileName );
        }

        public static void Save()
        {
            if( mSettings == null )
            {
                return;
            }

            FileUtils.WriteXml( gcSettingsFileName, ref mSettings );
        }

        public static void Set( String aKey, String aValue )
        {
            if( mSettings == null )
            {
                return;
            }

            if( mSettings.ContainsKey( aKey ) )
            {
                mSettings[ aKey ] = aValue;
            }
            else
            {
                mSettings.Add( aKey, aValue );
            }
        }
    }
}
