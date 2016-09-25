using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class MedianFilter : IFilter
    {

        public MedianFilter()
        {
 
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

                var boxsize = 9;
                int pixelSize = 3;

                for (int y = 0; y < image.Height; y++)
                {
                    //get the data from the original image
                    byte* oRow = (byte*)originalData.Scan0 + (y * originalData.Stride);

                    //get the data from the new image
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);

                    for (int x = 0; x < image.Width; x++)
                    {
                        var bytes = GetRegionData(originalData, x, y, boxsize);
                        var ordered = bytes.OrderBy(b => b).ToArray();
                        var median = ordered[ordered.Count() / 2];
                        nRow[x * pixelSize] = median;
                        nRow[x * pixelSize + 1] = median;
                        nRow[x * pixelSize + 2] = median;
                    }
                }

                //unlock the bitmaps
                newBitmap.UnlockBits(newData);
                image.UnlockBits(originalData);

                image.Dispose();
                return newBitmap;
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
            return new MedianFilter()
            {
                
            };
        }

        public override string ToString()
        {
            return "MedianFilter";
        }
    }
}
