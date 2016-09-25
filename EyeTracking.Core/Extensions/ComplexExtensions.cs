using EyeTracking.Core.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Extensions
{
    public static class ComplexExtensions
    {
        public static int[][] ToIntegerArray(this Complex[][] complex)
        {
            int width = complex.Length;
		    int height = complex[0].Length;
		    int[][] data = new int[width][];

		    for (int x = 0; x < width; x++)
            {
                data[x] = new int[height];
			    for (int y = 0; y < height; y++)
				    data[x][y] = (int) (complex[x][y].Magnitude);
            }

		    return data;
        }

        public static Complex[][] ToComplexArray(this Bitmap data)
        {
            return data.ToIntegerArray()
                .ToComplexArray();
        }

        public static Complex[][] ToComplexArray(this int[][] data)
        {
            int width = data.Length;
            int height = data[0].Length;
		    Complex[][] complex = new Complex[width][];

		    for (int i = 0; i < width; i++)
            {
                complex[i] = new Complex[height];
			    for (int j = 0; j < height; j++)
				    complex[i][j] = new Complex(data[i][j]);
            }

		    return complex;
        }

        public static Bitmap ToBitmap(this Complex[][] complex, double scale)
        {
            int[][] data = complex.ToIntegerArray();
            return data.ToBitmap(scale);
        }

        public static double Max(this int[][] data)
        {
            int width = data.Length;
            int height = data[0].Length;
            double max = 0;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (max < data[x][y])
                        max = data[x][y];

            return max;
        }

        public static int[][] ToIntegerArray(this Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
		    int[][] data = new int[width][];

		    for (int x = 0; x < width; x++)
            {
                data[x] = new int[height];
                for (int y = 0; y < height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    data[x][y] = (int)(pixel.GetBrightness() * 255 * Math.Pow(-1, x + y));
                }
            }
			    

		    return data;
        }

        public static Bitmap ToBitmap(this int[][] data, double scale)
        {
            double max = data.Max();
		    int width = data.Length;
		    int height = data[0].Length;
		    int[][] copy = new int[width][];
		    double temp = 255 / Math.Log(1 + Math.Abs(max));
            temp *= scale;
		    for (int x = 0; x < width; x++)
            {
                copy[x] = new int[height];
                for (int y = 0; y < height; y++)
                {
                    copy[x][y] = (int)(temp * Math.Log(1 + Math.Abs(data[x][y]) ));
                }
                    
            }
			
		    Bitmap image = new Bitmap(width, height);

            // todo - can be greatly increased on efficiency if we unlock the bits in an unsafe scope.
		    for (int x = 0; x < width; x++)
			    for (int y = 0; y < height; y++) {
				    int value = copy[x][y] << 24 | copy[x][y] << 16 | copy[x][y] << 8 | copy[x][y];
				    image.SetPixel(x, y, Color.FromArgb(value));
			    }

		    return image;
        }
    }
}
