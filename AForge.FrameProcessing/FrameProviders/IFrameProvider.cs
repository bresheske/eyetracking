using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AForge.FrameProcessing.FrameProviders
{
    public interface IFrameProvider : IDisposable
    {
        event EventHandler<FrameEventArgs> OnNextFrame;
        event EventHandler<EventArgs> OnFinished;

        void Start();
        void Stop();

        bool IsRunning { get; }
        Bitmap NormalizeFrame(Bitmap frame);
        int FrameRate { get; set; }
    }
}
