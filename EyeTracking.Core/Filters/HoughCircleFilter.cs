using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class HoughCircleFilter : IFilter
    {
        [FilterConfigSetting]
        public int Radius { get; set; }
        [FilterConfigSetting]
        public short MinCircleIntensity { get; set; }
        [FilterConfigSetting]
        public int LocalPeakRadius { get; set; }
        [FilterConfigSetting]
        public int MinRelativeIntensity { get; set; }

        public HoughCircleFilter()
        {
            Radius = 25;
            MinRelativeIntensity = 25;
            MinCircleIntensity = 140;
        }

        public Bitmap Apply(Bitmap image)
        {
            var transform = new HoughCircleTransformation(Radius)
            {
                MinCircleIntensity = this.MinCircleIntensity,
                LocalPeakRadius = this.LocalPeakRadius
            };
            
            var img = new Bitmap(image);
            transform.ProcessImage(image);
            var circles = transform.GetCirclesByRelativeIntensity(MinRelativeIntensity);

            using (var graphics = Graphics.FromImage(img))
            {
                foreach (var c in circles)
                {
                    graphics.DrawEllipse(Pens.Blue, c.X - c.Radius, c.Y - c.Radius, c.Radius * 2, c.Radius * 2);
                }
            }
            return img;
        }

        public override string ToString()
        {
            return "HoughCircleDetector";
        }

        public object Clone()
        {
            return new HoughCircleFilter()
            {
                LocalPeakRadius = this.LocalPeakRadius,
                MinCircleIntensity = this.MinCircleIntensity,
                Radius = this.Radius
            };
        }
    }
}
