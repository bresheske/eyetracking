using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class HighboostFilter : IFilter
    {
        [FilterConfigSetting]
        public float Weight { get; set; }

        public HighboostFilter()
        {
            Weight = 1;
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

                var boxsize = 3;
                var radius = (boxsize - 1) / 2;

                // First, blur image.
                for (int y = radius; y < image.Height - radius; y++)
                {
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);
                    for (int x = radius; x < image.Width - radius; x++)
                    {
                        var bytes = GetRegionData(originalData, x, y, boxsize);
                        var avg = (byte)bytes.Average(b => (int)b);
                        SetRegionData(newData, x, y, boxsize, avg);
                    }
                }

                int pixelSize = 3;
                for (int y = 0; y < image.Height; y++)
                {
                    byte* oRow = (byte*)originalData.Scan0 + (y * originalData.Stride);
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);
                    for (int x = 0; x < image.Width; x++)
                    {
                        // original intensity
                        byte oi = (byte)((oRow[x * pixelSize] + oRow[x * pixelSize + 1] + oRow[x * pixelSize + 2]) / 3);
                        // blurred intensity
                        byte ni = (byte)((nRow[x * pixelSize] + nRow[x * pixelSize + 1] + nRow[x * pixelSize + 2]) / 3);
                        // difference (mask)
                        int di = (int)(Weight * Math.Abs(oi - ni));
                        // Add mask to original.
                        int hbfi = (di + oi);
                        if (hbfi > 255)
                            hbfi = 255;

                        nRow[x * pixelSize] = (byte)hbfi; //B
                        nRow[x * pixelSize + 1] = (byte)hbfi; //G
                        nRow[x * pixelSize + 2] = (byte)hbfi; //R
                    }
                }

                //unlock the bitmaps
                newBitmap.UnlockBits(newData);
                image.UnlockBits(originalData);

                image.Dispose();
                return newBitmap;
            }
        }

        private void SetRegionData(BitmapData image, int xpos, int ypos, int boxsize, byte color)
        {
            unsafe
            {
                var radius = (boxsize - 1) / 2;
                int pixelSize = 3;
                for (int h = ypos - radius; h <= ypos + radius; h++)
                {
                    byte* nRow = (byte*)image.Scan0 + (h * image.Stride);
                    for (int w = xpos - radius; w <= xpos + radius; w++)
                    {
                        nRow[w * pixelSize] = color; //B
                        nRow[w * pixelSize + 1] = color; //G
                        nRow[w * pixelSize + 2] = color; //R
                    }
                }

            }
        }

        private byte[] GetRegionData(BitmapData image, int xpos, int ypos, int boxsize)
        {
            unsafe
            {
                var radius = (boxsize - 1) / 2;
                var count = (int)Math.Pow(boxsize, 2);
                var output = new byte[count];
                var index = 0;
                int pixelSize = 3;
                // For now, we're only retreiving the intensity values.
                for (int h = ypos - radius; h <= ypos + radius; h++)
                {
                    byte* nRow = (byte*)image.Scan0 + (h * image.Stride);
                    for (int w = xpos - radius; w <= xpos + radius; w++)
                    {
                        byte blue = nRow[w * pixelSize];
                        byte green = nRow[w * pixelSize + 1];
                        byte red = nRow[w * pixelSize + 2];
                        var intensity = (int)(blue + green + red) / 3d;
                        output[index++] = (byte)intensity;
                    }
                }

                return output;
            }
        }

        public object Clone()
        {
            return new HighboostFilter()
            {
                Weight = this.Weight
            };
        }

        public override string ToString()
        {
            return "HighboostFilter";
        }
    }
}
