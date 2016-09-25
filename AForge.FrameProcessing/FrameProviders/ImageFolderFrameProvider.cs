using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace AForge.FrameProcessing.FrameProviders
{
    public class ImageFolderFrameProvider : IFrameProvider
    {
        private readonly string _folder;
        private bool _running;
        private Stopwatch _fpswatch;

        public event EventHandler<FrameEventArgs> OnNextFrame;

        public int FrameRate { get; set; }

        public ImageFolderFrameProvider()
        {
            FrameRate = 1;
            _folder = @"D:\Images\river";
        }

        public ImageFolderFrameProvider(string folder)
        {
            _folder = folder;
        }

        public void Start()
        {
            _running = true;
            if (!Directory.Exists(_folder))
            {
                _running = false;
                return;
            }
            new Thread(() =>
            {
                var files = Directory.GetFiles(_folder)
                    .Where(x => x.EndsWith(".bmp") || x.EndsWith(".jpg") || x.EndsWith("png") || x.EndsWith("tif") || x.EndsWith("jpeg"));

                _fpswatch = new Stopwatch();
                _fpswatch.Start();

                for (int i = 0; i < files.Count() && _running; i++)
                {
                    
                    if (OnNextFrame != null && _fpswatch.ElapsedMilliseconds > (1000d / (double)FrameRate))
                    {
                        var image = files.ElementAt(i);
                        var bitmap = (Bitmap)Image.FromFile(image);
                        OnNextFrame(this, new FrameEventArgs() { Frame = bitmap, FileName = files.ElementAt(i) });
                        _fpswatch.Restart();
                    }

                    // Restart the process. 
                    if (i == files.Count() - 1)
                        i = 0;
                }
                
                _running = false;
                if (OnFinished != null)
                    OnFinished(this, new EventArgs());
            }).Start();
        }

        public void Stop()
        {
            _running = false;
        }

        public bool IsRunning
        {
            get { return _running; }
        }

        public void Dispose()
        {
            Stop();
        }

        public event EventHandler<EventArgs> OnFinished;


        public Bitmap NormalizeFrame(Bitmap frame)
        {
            return frame;
        }

    }
}
