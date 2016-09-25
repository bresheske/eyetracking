using AForge.Imaging.Filters;
using EyeTracking.Core.TransformProcessors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeTracking.Core.Extensions;

namespace EyeTracking.Core.Filters
{
    public class HighPassFilter : IFilter
    {
        private HighPassProcessor _transform;

        [FilterConfigSetting]
        public int CircleX { get { return _transform.CircleX; } set { _transform.CircleX = value; } }
        [FilterConfigSetting]
        public int CircleY { get { return _transform.CircleY; } set { _transform.CircleY = value; } }
        [FilterConfigSetting]
        public double CircleRadius { get { return _transform.CircleRadius; } set { _transform.CircleRadius = value; } }

        public HighPassFilter()
        {
            _transform = new HighPassProcessor();
            CircleRadius = 5;
            CircleX = 256;
            CircleY = 256;
        }

        public System.Drawing.Bitmap Apply(System.Drawing.Bitmap image)
        {
            return _transform.Apply(image
                .ToComplexArray())
                .ToBitmap(1);
        }

        public object Clone()
        {
            return new HighPassFilter()
            {
                CircleRadius = this.CircleRadius,
                CircleX = this.CircleX,
                CircleY = this.CircleY
            };
        }

        public override string ToString()
        {
            return "HighPassFilter";
        }
    }
}
