using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Persistance
{
    public class SettingsModel
    {
        public int CropWidth { get; set; }
        public int CropHeight { get; set; }
        public int CropX { get; set; }
        public int CropY { get; set; }

        public int IrisHue { get; set; }

        public PointModel ReferencePointTopLeft { get; set; }

        public PointModel ReferencePointBottomRight { get; set; }
    }
}
