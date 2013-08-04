using System;
using System.Windows.Forms; // Screen
using System.Drawing; // Bitmap, CopyPixelOperation
using System.Drawing.Imaging; // PixelFormat

namespace prjShotMonitor.Actions
{
    class CScreenShotAction : CShotAction
    {
        Screen[] screens = Screen.AllScreens;

        public CScreenShotAction(string p_action_code)
            : base(p_action_code)
        {
            int L = screens.Length;
            DeviceList = new string[L];
            for (int i = 0; i < L; i++)
            {
                DeviceList[i] = screens[i].DeviceName;
            }
        }

        public override void do_action()
        {
            int index = (int)settings.DeviceIndex;
            if (index < 0)
            {
                CShotResult[] shots = get_all_shots();

                foreach (CShotResult shot in shots)
                {
                    save_shot(shot);
                }
            }
            else
            {
                CShotResult shot = get_shot(screens[index]);
                save_shot(shot);
            }
        }

        private CShotResult[] get_all_shots()
        {
            int L = screens.Length;
            CShotResult[] shots = new CShotResult[L];

            for (int i = 0; i < L; i++)
            {
                shots[i] = get_shot(screens[i]);
            }
            return shots;
        }

        private CShotResult get_shot(Screen screen)
        {
            Bitmap bmpShot = new Bitmap(
                screen.Bounds.Width,
                screen.Bounds.Height,
                PixelFormat.Format32bppArgb
            );
            Graphics gfxScreenshot = Graphics.FromImage(bmpShot);
            gfxScreenshot.CopyFromScreen(
                screen.Bounds.X,
                screen.Bounds.Y,
                0, 0,
                screen.Bounds.Size,
                CopyPixelOperation.SourceCopy
            );
            CShotResult result = new CShotResult(screen.DeviceName, bmpShot);
            return result;
        }
    }
}