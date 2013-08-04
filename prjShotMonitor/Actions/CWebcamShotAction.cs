using System;
using System.Drawing; // Bitmap
using AForge.Video;
using AForge.Video.DirectShow;

namespace prjShotMonitor.Actions
{
    class CWebcamShotAction : CShotAction
    {
        private class CMyVideoCaptureDevice : VideoCaptureDevice
        {
            public string DeviceName { get; set; }

            public CMyVideoCaptureDevice(string MonikerString)
                : base(MonikerString)
            {

            }
        }

        public FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

        public CWebcamShotAction(string p_action_code)
            : base(p_action_code)
        {
            int L = VideoDevices.Count;
            DeviceList = new string[L];
            for (int i = 0; i < L; i++)
            {
                DeviceList[i] = VideoDevices[i].Name;
            }
        }

        public override void do_action()
        {
            Start((int)settings.DeviceIndex);
        }

        private void Start(int index = -1)
        {
            if (index < 0)
            {
                foreach (FilterInfo device in VideoDevices)
                {
                    StartDevice(device);
                }
            }
            else
            {
                StartDevice(VideoDevices[index]);
            }
        }

        private void StartDevice(FilterInfo device)
        {
            CMyVideoCaptureDevice videoSource = new CMyVideoCaptureDevice(device.MonikerString);
            videoSource.DeviceName = device.Name;
            videoSource.NewFrame += new NewFrameEventHandler(NewFrame_Handler);
            videoSource.Start();
        }

        private void NewFrame_Handler(object sender, NewFrameEventArgs eventArgs)
        {
            CShotResult shot = new CShotResult((sender as CMyVideoCaptureDevice).DeviceName, eventArgs.Frame);
            save_shot(shot);
            (sender as CMyVideoCaptureDevice).SignalToStop();
        }
    }
}