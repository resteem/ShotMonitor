using System;
using System.Drawing; // Bitmap
using System.Text.RegularExpressions; // Regexp

namespace prjShotMonitor.Actions
{
    public class CShotResult
    {
        public Bitmap Bmp { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = Regex.Replace(value, "[^a-zA-Z0-9 -]", ""); }
        }

        public CShotResult(string p_name, Bitmap p_bmp)
        {
            Name = p_name;
            Bmp = p_bmp;
        }
    }
}