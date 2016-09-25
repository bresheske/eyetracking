using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class ScleraScanningFilter : IFilter
    {
        [FilterConfigSetting]
        public int DeltaY { get; set; }
        [FilterConfigSetting]
        public int DeltaX { get; set; }
        [FilterConfigSetting]
        public float MinBrightness { get; set; }

        private Rectangle lefteye;
        private Rectangle righteye;

        public ScleraScanningFilter()
        {
            DeltaY = 10;
            DeltaX = 5;
            MinBrightness = .105f;
            lefteye = new Rectangle();
            righteye = new Rectangle();
        }

        public Bitmap Apply(Bitmap original)
        {

            return original;

            
        }

        public override string ToString()
        {
            return "ScleraScanningFilter";
        }

        public object Clone()
        {
            return new ScleraScanningFilter()
            {
                DeltaY = this.DeltaY,
                DeltaX = this.DeltaX,
                MinBrightness = this.MinBrightness
            };
        }
    }
}
