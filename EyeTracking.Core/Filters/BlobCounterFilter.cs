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
    public class BlobCounterFilter : IFilter
    {

        [FilterConfigSetting]
        public int MinWidth { get; set; }
        [FilterConfigSetting]
        public int MaxWidth { get; set; }

        [FilterConfigSetting]
        public int MinHeight { get; set; }
        [FilterConfigSetting]
        public int MaxHeight { get; set; }

        public BlobCounterFilter()
        {
            MinWidth = 15;
            MinHeight = 15;
            MaxWidth = 50;
            MaxHeight = 50;
        }

        public IEnumerable<Point> GetBlobPositions(Bitmap image)
        {
            var bc = new BlobCounter();
            bc.ProcessImage(image);
            var rects = bc.GetObjectsRectangles();
            var eyes = rects.Where(x => x.Width >= MinWidth && x.Width <= MaxWidth && x.Height <= MaxHeight && x.Height >= MinHeight);

            var mw = MinWidth;
            var mh = MinHeight;

            while (eyes.Count() < 2 && (mw > 0 || mh > 0))
            {
                // we didn't find 2 eyes.  We need to broaden our filter.
                if (mw > 0)
                    mw--;
                if (mh > 0)
                    mh--;
                eyes = rects.Where(x => x.Width >= mw && x.Width <= MaxWidth && x.Height <= MaxHeight && x.Height >= mh);
            }

            if (eyes.Count() >= 2)
            {
                // Order our eyes to make sure the right eye is in the [1] spot.
                eyes = eyes.Take(2).OrderBy(x => x.X);
            }

            return eyes.Select(x => new Point(x.X + x.Width / 2, x.Y + x.Height / 2));
        }

        public Bitmap Apply(Bitmap image)
        {
            var points = GetBlobPositions(image);

            using (var gfx = Graphics.FromImage(image))
            {
                foreach (var p in points)
                {
                    gfx.DrawEllipse(Pens.Blue, new Rectangle(p, new Size(10, 10)));
                }
            }
            return image;
        }

        public override string ToString()
        {
            return "BlobCounter";
        }

        public object Clone()
        {
            return new BlobCounterFilter()
            {
                MaxHeight = this.MaxHeight,
                MaxWidth = this.MaxWidth,
                MinHeight = this.MinHeight,
                MinWidth = this.MinWidth
            };
        }
    }

    public class SizeBlobFilter : IBlobsFilter
    {
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }

        public bool Check(Blob blob)
        {
            return blob.Rectangle.Width >= MinWidth
                && blob.Rectangle.Width <= MaxWidth
                && blob.Rectangle.Height >= MinHeight
                && blob.Rectangle.Height <= MaxHeight;
        }
    }
}
