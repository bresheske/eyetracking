using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class HueFilter : IFilter
    {
        [FilterConfigSetting]
        public int TargetHue { get; set; }
        [FilterConfigSetting]
        public int MaxDeltaHue { get; set; }

        public HueFilter()
        {
            // Some defaults for my blue eyes.
            TargetHue = 250;
            MaxDeltaHue = 50;
        }

        public System.Drawing.Bitmap Apply(System.Drawing.Bitmap image)
        {
            unsafe
            {
                //create an empty bitmap the same size as original
                Bitmap newBitmap = new Bitmap(image.Width, image.Height);

                //lock the original bitmap in memory
                BitmapData originalData = image.LockBits(
                   new Rectangle(0, 0, image.Width, image.Height),
                   ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                //lock the new bitmap in memory
                BitmapData newData = newBitmap.LockBits(
                   new Rectangle(0, 0, image.Width, image.Height),
                   ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                //set the number of bytes per pixel
                int pixelSize = 3;
                for (int y = 0; y < image.Height; y++)
                {
                    //get the data from the original image
                    byte* oRow = (byte*)originalData.Scan0 + (y * originalData.Stride);

                    //get the data from the new image
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);

                    for (int x = 0; x < image.Width; x++)
                    {
                        //create the grayscale version
                        byte blue = oRow[x * pixelSize];
                        byte green = oRow[x * pixelSize + 1];
                        byte red = oRow[x * pixelSize + 2];
                        float hue = Color.FromArgb(red, green, blue)
                            .GetHue();

                        byte newcolor =
                            hue <= TargetHue + MaxDeltaHue &&
                            hue >= TargetHue - MaxDeltaHue
                            ? (byte)0xFF
                            : (byte)0x00;

                        nRow[x * pixelSize] = newcolor; //B
                        nRow[x * pixelSize + 1] = newcolor; //G
                        nRow[x * pixelSize + 2] = newcolor; //R
                    }
                }

                //unlock the bitmaps
                newBitmap.UnlockBits(newData);
                image.UnlockBits(originalData);

                image.Dispose();
                return newBitmap;
            }
        }

        public object Clone()
        {
            return new HueFilter()
            {
                MaxDeltaHue = this.MaxDeltaHue,
                TargetHue = this.TargetHue,
            };
        }

        public override string ToString()
        {
            return "HueFilter";
        }
    }
}
