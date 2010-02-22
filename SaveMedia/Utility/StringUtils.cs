using System;

namespace Utility
{
    static class StringUtils
    {
        public static bool StringBetween( String aSource, String aStart, String aEnd, out String aResult )
        {
            aResult = String.Empty;

            if( String.IsNullOrEmpty( aSource ) )
            {
                return false;
            }

            int theStartIndex = aSource.IndexOf( aStart );
            if( theStartIndex == -1 )
            {
                return false;
            }
            theStartIndex = theStartIndex + aStart.Length;

            int theEndIndex = aSource.IndexOf( aEnd, theStartIndex );
            if( theEndIndex == -1 )
            {
                return false;
            }

            int theStringLength = theEndIndex - theStartIndex;
            aResult = aSource.Substring( theStartIndex, theStringLength );

            return true;
        }
    }
}
