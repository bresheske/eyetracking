using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class InvertFilter : IFilter
    {

        public InvertFilter()
        {

        }

        public Bitmap Apply(Bitmap image)
        {
            return new Invert()
                .Apply(image);
        }

        public override string ToString()
        {
            return "Invert";
        }

        public object Clone()
        {
            return new InvertFilter();
        }
    }
}
