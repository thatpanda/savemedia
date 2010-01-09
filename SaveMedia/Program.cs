﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SaveMedia
{
    static class Program
    {
        public const String Title           = "SaveMedia";  // Description
        public const String Description     = "";           // Comments
        public const String Product         = "SaveMedia";  // Product Name
        public const String Company         = "";
        public const String Copyright       = "© 2009 Eric Choy";
        public const String AssemblyVersion = "1.0.0.0";
        public const String FileVersion     = "1.6.1.0";
        public const String TitleVersion    = "1.6.1b";
        public const String Date            = "October 23rd, 2009";
        public const String UserAgent       = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
