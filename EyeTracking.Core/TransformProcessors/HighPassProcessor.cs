using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.TransformProcessors
{
    public class HighPassProcessor : FFTProcessor
    {

        public int CircleX { get; set; }
        public int CircleY { get; set; }
        public double CircleRadius { get; set; }

        public Objects.Complex[][] Apply(Objects.Complex[][] input)
        {
            // First, perform the FFT and get the spectrum data.
            var fftdata = base.Apply(input);

            // Now, we need do filter based on the X and Y positions to the Circle.
            for(int x = 0; x < input.Length; x++)
            {
                for(int y = 0; y < input[x].Length; y++)
                {
                    // Calculate distance from circle.
                    var dist = CalculateDistance(x, y, CircleX, CircleY);

                    // Replace if out of radius.
                    if (dist <= CircleRadius)
                        fftdata[x][y] = new Objects.Complex(0, 0);
                }
            }

            return DoFFT(fftdata);
        }

        private double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

    }
}
