using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class CropFilter : IFilter
    {
        [FilterConfigSetting]
        public int Width { get; set; }
        [FilterConfigSetting]
        public int Height { get; set; }
        [FilterConfigSetting]
        public int X { get; set; }
        [FilterConfigSetting]
        public int Y { get; set; }

        public CropFilter()
        {
            X = 10;
            Y = 10;
            Width = 320;
            Height = 240;
        }

        public Bitmap Apply(Bitmap image)
        {
            var rect = new Rectangle(X, Y, Width, Height);
            var newimg = new Bitmap(rect.Width, rect.Height);
            using (var gfx = Graphics.FromImage(newimg))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, newimg.Width, newimg.Height),
                    rect,
                    GraphicsUnit.Pixel);
            }
            image.Dispose();
            return newimg;
        }

        public override string ToString()
        {
            return "Crop";
        }

        public object Clone()
        {
            return new CropFilter()
            {
                Width = Width,
                Height = Height,
                X = X,
                Y = Y
            };
        }
    }
}
