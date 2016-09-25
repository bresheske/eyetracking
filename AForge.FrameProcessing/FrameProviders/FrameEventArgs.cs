using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AForge.FrameProcessing.FrameProviders
{
    public class FrameEventArgs : EventArgs
    {
        public Bitmap Frame { get; set; }
        public string FileName { get; set; }
    }
}
