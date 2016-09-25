using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class ThresholdFilter : IFilter
    {
        [FilterConfigSetting]
        public int MinExposure { get; set; }

        public ThresholdFilter()
        {
            MinExposure = 150;
        }

        public System.Drawing.Bitmap Apply(System.Drawing.Bitmap image)
        {
            return new Threshold(MinExposure)
                .Apply(image);
        }

        public object Clone()
        {
            return new ThresholdFilter()
            {
                MinExposure = this.MinExposure
            };
        }

        public override string ToString()
        {
            return "Threshold";
        }
    }
}
