using System;
using System.Collections.Generic;

namespace SaveMedia.Sites
{
    interface ISite
    {
        bool Support( ref Uri aUrl );

        void TryParse( ref Uri aUrl,
                       ref List<DownloadTag> aDownloadQueue,
                       ref IMainForm aUI,
                       out String aError );
    }
}
