using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class ColorLogTransformationFilter : IFilter
    {

        public ColorLogTransformationFilter()
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

                var max = Math.Log10(256);

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
                        var intensity = (blue + green + red) / 3d;
                        var newr = Math.Log10(1 + intensity);
                        var newintensity = (newr / max) * 255d;
                        var diff = (byte)Math.Abs(intensity - newintensity) * .5f;

                        nRow[x * pixelSize] = (byte)(blue + diff); //B
                        nRow[x * pixelSize + 1] = (byte)(green + diff); //G
                        nRow[x * pixelSize + 2] = (byte)(red + diff); //R
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
            return new ColorLogTransformationFilter()
            {
                
            };
        }

        public override string ToString()
        {
            return "ColorLogTransformationFilter";
        }
    }
}
