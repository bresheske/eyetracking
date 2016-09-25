using AForge.FrameProcessing.FrameProviders;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.FrameProcessors
{
    public class FilterFrameProcessor : IFrameProcessor
    {
        private readonly IFrameProvider _provider;

        public FilterFrameProcessor(IFrameProvider provider)
        {
            _provider = provider;
            Filters = new List<EyeTracking.Core.Filters.IFilter>();
        }

        public List<EyeTracking.Core.Filters.IFilter> Filters { get; private set; }

        public void Start()
        {
            if (_provider.IsRunning)
                throw new InvalidOperationException("The processor is already running.");

            _provider.OnNextFrame += _provider_OnNextFrame;
            _provider.Start();
        }

        private void _provider_OnNextFrame(object sender, FrameEventArgs e)
        {
            // Apply the filters.
            var image = (Bitmap)e.Frame.Clone();
            // todo: might cause disposing issues.
            lock(Filters)
            {
                foreach (var f in Filters)
                {
                    image = f.Apply(image);
                }
            }
            

            OnNextFrame(this, new FrameEventArgs() { Frame = image });
        }

        public void Stop()
        {
            if (!_provider.IsRunning)
                throw new InvalidOperationException("The processor is not running.");

            _provider.OnNextFrame -= _provider_OnNextFrame;
            _provider.Stop();
        }

        public event EventHandler<AForge.FrameProcessing.FrameProviders.FrameEventArgs> OnNextFrame;

        public bool IsRunning
        {
            get { return _provider.IsRunning; }
        }
    }
}
