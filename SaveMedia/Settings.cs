using System;

using Utility;

namespace SaveMedia
{
    static class Settings
    {
        private const String gcSettingsFilename = "Settings.xml";
        private static System.Collections.Generic.Dictionary< String, String > mSettings;

        public static String YouTubeQuality()
        {
            Load();

            if( mSettings.ContainsKey( "YouTubeQuality" ) &&
                !mSettings[ "YouTubeQuality" ].Equals( "0" ) )
            {
                return mSettings[ "YouTubeQuality" ];
            }

            return String.Empty;
        }

        public static void YouTubeQuality( ref String theValue )
        {
            if( mSettings.ContainsKey( "YouTubeQuality" ) )
            {
                mSettings[ "YouTubeQuality" ] = theValue;
            }
            else
            {
                mSettings.Add( "YouTubeQuality", theValue );
            }
        }

        public static void Load()
        {
            if( mSettings == null )
            {
                mSettings = new System.Collections.Generic.Dictionary<String, String>();
            }

            mSettings = FileUtils.ReadXml( gcSettingsFilename );
        }

        public static void Save()
        {
            if( mSettings == null )
            {
                return;
            }

            FileUtils.WriteXml( gcSettingsFilename, ref mSettings );
        }
    }
}
