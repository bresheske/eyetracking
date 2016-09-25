using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class ResizeFilter : IFilter
    {
        [FilterConfigSetting]
        public int Width { get; set; }
        [FilterConfigSetting]
        public int Height { get; set; }

        public ResizeFilter()
        {
            Width = 320;
            Height = 240;
        }

        public Bitmap Apply(Bitmap image)
        {
            return new Bitmap(image, new Size(Width, Height));
        }

        public override string ToString()
        {
            return "Resize";
        }

        public object Clone()
        {
            return new ResizeFilter()
            {
                Width = Width,
                Height = Height
            };
        }
    }
}
