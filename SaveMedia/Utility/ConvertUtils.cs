using System;

namespace Utility
{
    static class ConvertUtils
    {
        public static String BytesToHex( byte[] aBytes )
        {
            String theHexString = String.Empty;
            foreach( byte theByte in aBytes )
            {
                theHexString += theByte.ToString( "X2" );
            }

            return theHexString;
        }

        public static byte[] HexToBytes( String aHex )
        {
            if( aHex.Length % 2 != 0 )
            {
                return new byte[ 0 ];
            }

            byte[] Bytes = new byte[ aHex.Length / 2 ];
            for( int theArrayIndex = 0, theHexIndex = 0;
                 theHexIndex <= aHex.Length - 2;
                 theArrayIndex += 1, theHexIndex += 2 )
            {
                String theHexString = aHex.Substring( theHexIndex, 2 );
                uint theDecimal = uint.Parse( theHexString, System.Globalization.NumberStyles.HexNumber );
                char theChar = System.Convert.ToChar( theDecimal );
                Bytes[ theArrayIndex ] = System.Convert.ToByte( theChar );
            }

            return Bytes;
        }

        public static String AsciiToHex( String aAscii )
        {
            String theHexString = String.Empty;
            foreach( char theChar in aAscii )
            {
                uint theDecimal = System.Convert.ToUInt32( theChar );
                theHexString += theDecimal.ToString( "X2" );
            }
            return theHexString;
        }

        public static String HexToAscii( String aHex )
        {
            if( aHex.Length % 2 != 0 )
            {
                return System.String.Empty;
            }

            String theAsciiString = String.Empty;
            for( int theIndex = 0; theIndex <= aHex.Length - 2; theIndex += 2 )
            {
                String theHexString = aHex.Substring( theIndex, 2 );
                uint theDecimal = uint.Parse( theHexString, System.Globalization.NumberStyles.HexNumber );
                char theChar = System.Convert.ToChar( theDecimal );
                theAsciiString += theChar.ToString();
            }

            return theAsciiString;
        }
    }
}
