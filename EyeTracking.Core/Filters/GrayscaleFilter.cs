using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class GrayscaleFilter : IFilter
    {
        [FilterConfigSetting]
        public double Red { get; set; }
        [FilterConfigSetting]
        public double Green { get; set; }
        [FilterConfigSetting]
        public double Blue { get; set; }

        public GrayscaleFilter()
        {
            Red = .2d;
            Green = .2d;
            Blue = .2d;
        }

        public Bitmap Apply(Bitmap image)
        {
            return new Grayscale(Red, Green, Blue)
                .Apply(image);
        }

        public override string ToString()
        {
            return "Grayscale";
        }

        public object Clone()
        {
            return new GrayscaleFilter()
            {
                Red = this.Red,
                Green = this.Green,
                Blue = this.Blue
            };
        }
    }
}
