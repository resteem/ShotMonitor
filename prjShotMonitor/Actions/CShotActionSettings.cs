using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjShotMonitor.Actions
{
    public class CShotActionSettings
    {
        /*** CONSTS ***/

        // image type
        public const int IT_JPG = 0;
        public const int IT_PNG = 1;
        // extensions
        public const string IT_EXT_PNG = ".PNG";
        public const string IT_EXT_JPG = ".JPG";

        /*** FIELDS ***/

        public byte FileFormat { get; set; }
        public long JpegQuality { get; set; }
        public int DeviceIndex { get; set; }

        private int _TimerInterval;
        public int TimerInterval
        {
            get { return _TimerInterval; }
            set
            {
                // секунды
                _TimerInterval = value * 1000;
            }
        }

        private string _DestinationFolder;
        public string DestinationFolder
        {
            get { return _DestinationFolder; }
            set
            {
                string path = value;
                try
                {
                    // если путь относительный (без ":"), считаем, что относительно текущего пользователя
                    if (path.IndexOf(':') < 0)
                    {
                        // C:\Users\Username
                        path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + path;
                    }
                    // создать папку, если не существует
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    // если уже существовала или создалась
                    if (System.IO.Directory.Exists(path))
                    {
                        _DestinationFolder = path;
                    }
                }
                catch
                {
                    _DestinationFolder = String.Empty;
                }
                finally { }
            }
        }

        public CShotActionSettings()
        {
            set_defaults();
        }

        public void set_defaults()
        {
            TimerInterval = Properties.Settings.Default.TimerInterval;
            DestinationFolder = Properties.Settings.Default.DestinationFolder;
            FileFormat = Properties.Settings.Default.FileFormat;
            JpegQuality = Properties.Settings.Default.JpegQuality;
            DeviceIndex = Properties.Settings.Default.DeviceIndex;
        }
    }
}