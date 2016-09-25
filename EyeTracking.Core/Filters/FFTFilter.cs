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
    public class FFTFilter : IFilter
    {
        private ITransformProcessor _transform;

        public FFTFilter()
        {
            _transform = new FFTProcessor();
        }

        public System.Drawing.Bitmap Apply(System.Drawing.Bitmap image)
        {
            return _transform.Apply(image
                .ToComplexArray())
                .ToBitmap(1);
        }

        public object Clone()
        {
            return new FFTFilter();
        }

        public override string ToString()
        {
            return "FFTFilter";
        }
    }
}
