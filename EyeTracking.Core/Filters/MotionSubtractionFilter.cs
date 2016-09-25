using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class MotionSubtractionFilter : IFilter
    {
        private Bitmap previous;

        public MotionSubtractionFilter()
        {

        }

        public Bitmap Apply(Bitmap image)
        {
            if (previous == null)
            {
                previous = image;
                return image;
            }

            var sub = new Subtract(image);
            var ret =  sub.Apply(previous);
            previous.Dispose();
            previous = image;
            return ret;
        }

        public override string ToString()
        {
            return "BackgroundSubtraction";
        }

        public object Clone()
        {
            return new MotionSubtractionFilter();
        }
    }
}
