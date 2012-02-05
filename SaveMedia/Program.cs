using System;
using System.Windows.Forms;

namespace SaveMedia
{
    static class Program
    {
        public const String Title           = "SaveMedia";  // Description
        public const String Description     = "";           // Comments
        public const String Product         = "SaveMedia";  // Product Name
        public const String Company         = "";
        public const String Copyright       = "© 2012 Eric Choy";
        public const String AssemblyVersion = "1.0.0.0";
        public const String FileVersion     = "1.7.6.0";
        public const String TitleVersion    = "1.7.6";
        public const String Date            = "February 6th, 2012";
        public const String UserAgent       = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15 (.NET CLR 3.5.30729)";

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
