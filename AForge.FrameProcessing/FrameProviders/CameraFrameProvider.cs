using AForge.Video.DirectShow;
using System;
using System.Diagnostics;
using System.Drawing;

namespace AForge.FrameProcessing.FrameProviders
{
    public class CameraFrameProvider : IFrameProvider
    {
        private VideoCaptureDevice _source;
        private int _camera;
        private Stopwatch _fpswatch;

        /// <summary>
        /// Frames Per Second.
        /// </summary>
        public int FrameRate { get; set; }

        public CameraFrameProvider()
        {
            FrameRate = 10;
            _camera = 0;
        }

        public CameraFrameProvider(int camera)
            : this()
        {
            _camera = camera;
        }

        public void Start()
        {
            _fpswatch = new Stopwatch();
            _fpswatch.Start();
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _source = new VideoCaptureDevice(videoDevices[_camera].MonikerString);
            _source.NewFrame += (obj, args) =>
            {
                if (OnNextFrame != null && _fpswatch.ElapsedMilliseconds > (1000d / (double)FrameRate))
                {
                    OnNextFrame(this, new FrameEventArgs() { Frame = args.Frame });
                    _fpswatch.Restart();
                }
            };
            _source.Start();
        }

        public void Stop()
        {
            if (_source.IsRunning)
            {
                _fpswatch.Stop();
                _source.Stop();
                if (OnFinished != null)
                    OnFinished(this, new EventArgs());
            }
        }

        public event EventHandler<FrameEventArgs> OnNextFrame;

        public bool IsRunning
        {
            get { return _source != null && _source.IsRunning; }
        }

        public void Dispose()
        {
            Stop();
        }

        public event EventHandler<EventArgs> OnFinished;

        public Bitmap NormalizeFrame(Bitmap frame)
        {
            // Threshold requires 8BPP (grayscale)
            return (Bitmap)frame
                .Clone(new Rectangle(0, 0, frame.Width, frame.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
        }
    }
}