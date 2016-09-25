using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class SmoothingFilter : IFilter
    {
        [FilterConfigSetting]
        public float Gamma { get; set; }
        [FilterConfigSetting]
        public int Size { get; set; }

        public SmoothingFilter()
        {
            Gamma = .5f;
            Size = 10;
        }

        public Bitmap Apply(Bitmap image)
        {
            return new GaussianBlur(Gamma, Size)
                .Apply(image);
        }

        public override string ToString()
        {
            return "Smoothing";
        }

        public object Clone()
        {
            return new SmoothingFilter();
        }
    }
}