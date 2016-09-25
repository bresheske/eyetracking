using AForge.FrameProcessing.FrameProviders;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.FrameProcessors
{
    public interface IFrameProcessor
    {
        bool IsRunning { get; }
        void Start();
        void Stop();

        event EventHandler<FrameEventArgs> OnNextFrame;
    }
}
