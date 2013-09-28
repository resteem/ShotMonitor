using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace prjShotMonitor.Actions
{
    public class CShotAction
    {
        public CShotActionSettings settings = new CShotActionSettings();
        public string action_code;
        public string[] DeviceList;

        /*** METHODS ***/

        public virtual void do_action() { }

        public CShotAction(string p_action_code)
        {
            action_code = p_action_code;
        }

        public void save_shot(CShotResult ShotResult)
        {
            string dir = get_destination_dir();
            if (dir != String.Empty)
            {
                string filename = dir + '\\' + ShotResult.Name + '_' + DateTime.Now.ToString("yyyyMMdd-HHmmss-fff");
                switch (settings.FileFormat)
                {
                    case CShotActionSettings.IT_JPG:
                        save_as_jpg(ShotResult.Bmp, filename);
                        break;
                    case CShotActionSettings.IT_PNG:
                        save_as_png(ShotResult.Bmp, filename);
                        break;
                    default:
                        save_as_png(ShotResult.Bmp, filename);
                        break;
                }
            }
        }

        private string get_destination_dir()
        {
            string dir = settings.DestinationFolder + '\\' + DateTime.Now.ToString("yyyyMMdd");
            if (settings.DestinationFolder != String.Empty)
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }
            if (!System.IO.Directory.Exists(dir))
            {
                dir = String.Empty;
            }
            return dir;
        }

        private void save_as_png(Bitmap bmp, string filename)
        {
            bmp.Save(filename + CShotActionSettings.IT_EXT_PNG, ImageFormat.Png);
        }

        private void save_as_jpg(Bitmap bmp, string filename)
        {
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, settings.JpegQuality);

            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp.Save(filename + CShotActionSettings.IT_EXT_JPG, myImageCodecInfo, myEncoderParameters);
        }

        // for save_as_jpg()
        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}