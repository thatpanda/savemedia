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
        public const String Copyright       = "© 2013 Eric Choy";
        public const String AssemblyVersion = "1.0.0.0";
        public const String FileVersion     = "1.7.8.0";
        public const String TitleVersion    = "1.7.8";
        public const String UserAgent       = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";

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
