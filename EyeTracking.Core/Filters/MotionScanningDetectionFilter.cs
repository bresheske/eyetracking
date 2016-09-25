using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class MotionScanningDetectionFilter : IFilter
    {
        [FilterConfigSetting]
        public int DeltaY { get; set; }
        [FilterConfigSetting]
        public int DeltaX { get; set; }
        [FilterConfigSetting]
        public float MinBrightness { get; set; }

        private Rectangle lefteye;
        private Rectangle righteye;

        public MotionScanningDetectionFilter()
        {
            DeltaY = 10;
            DeltaX = 5;
            MinBrightness = .105f;
            lefteye = new Rectangle();
            righteye = new Rectangle();
        }

        public Bitmap Apply(Bitmap image)
        {
            var lefttopleft = new Point();
            var lefttopright = new Point();
            var leftbottomleft = new Point();

            var righttopleft = new Point();
            var righttopright = new Point();
            var rightbottomleft = new Point();

            for (int y = 0; y < image.Height; y += DeltaY)
            {
                for (int x = 0; x < image.Width; x += DeltaX)
                {
                    // if the pixel is above the threshold.
                    var pixel = image.GetPixel(x, y);
                    if (!(pixel.GetBrightness() >= MinBrightness))
                        continue;

                    // Cut the image in half for 2 eyes.
                    if (x < image.Width / 2)
                    {
                        // Left eye
                        if (lefttopleft.IsEmpty)
                        {
                            lefttopleft.X = x;
                            lefttopleft.Y = y;
                        }
                        else if (lefttopright.X < x && y == lefttopleft.Y)
                        {
                            lefttopright.X = x;
                            lefttopright.Y = y;
                        }
                        else if (lefttopleft.Y < y && leftbottomleft.Y < y)
                        {
                            leftbottomleft.X = x;
                            leftbottomleft.Y = y;
                        }
                    }
                    else
                    {
                        // Right eye
                        if (righttopleft.IsEmpty)
                        {
                            righttopleft.X = x;
                            righttopleft.Y = y;
                        }
                        else if (righttopright.X < x && y == righttopleft.Y)
                        {
                            righttopright.X = x;
                            righttopright.Y = y;
                        }
                        else if (righttopleft.Y < y && rightbottomleft.Y < y)
                        {
                            rightbottomleft.X = x;
                            rightbottomleft.Y = y;
                        }
                    }
                }
            }

            var templeft = new Rectangle()
            {
                X = lefttopleft.X,
                Y = lefttopleft.Y,
                Width = lefttopright.X - lefttopleft.X,
                Height = leftbottomleft.Y - lefttopleft.Y
            };

            var tempright = new Rectangle()
            {
                X = righttopleft.X,
                Y = righttopleft.Y,
                Width = righttopright.X - righttopleft.X,
                Height = rightbottomleft.Y - righttopleft.Y
            };

            if (!templeft.IsEmpty)
                lefteye = templeft;
            if (!tempright.IsEmpty)
                righteye = tempright;

            if (!lefteye.IsEmpty && !righteye.IsEmpty)
            {
                using (var gfx = Graphics.FromImage(image))
                {
                    gfx.DrawRectangle(Pens.Blue, lefteye);
                    gfx.DrawRectangle(Pens.Red, righteye);
                }

                Console.WriteLine("L:({0}, {1}), R:({2}, {3})", lefteye.X, lefteye.Y, righteye.X, righteye.Y);
            }

            return image;
        }

        public override string ToString()
        {
            return "MotionScanningDetection";
        }

        public object Clone()
        {
            return new MotionScanningDetectionFilter()
            {
                DeltaY = this.DeltaY
            };
        }
    }
}
