using EyeTracking.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.TransformProcessors
{
    public class FFTProcessor : ITransformProcessor
    {
        public Objects.Complex[][] Apply(Objects.Complex[][] input)
        {
            return DoFFT(input);
        }

        public Complex[][] DoFFT(Complex[][] input)
        {
            int width = input.Length;
            int height = input[0].Length;
            Complex[][] output = new Complex[width][];

            // not a power of 2 -> cannot do FFT
            if (!IsPowerOf2(width, height))
                return new Complex[width][];

            // columns
            for (int x = 0; x < width; x++)
            {
                Complex[] column = new Complex[height];

                for (int i = 0; i < height; i++)
                    column[i] = input[x][i];

                column = FFT(column);
                output[x] = new Complex[height];
                for (int i = 0; i < height; i++)
                {
                    output[x][i] = column[i];
                }
            }

            // rows
            for (int y = 0; y < width; y++)
            {
                Complex[] row = new Complex[width];

                for (int i = 0; i < height; i++)
                    row[i] = output[i][y];

                row = FFT(row);

                for (int i = 0; i < height; i++)
                    output[i][y] = row[i];
            }

            return output;
        }

        // compute the FFT of x[], assuming its length is a power of 2
        public Complex[] FFT(Complex[] x)
        {
            int N = x.Length;

            // base case
            if (N == 1)
                return new Complex[] { x[0] };

            // radix 2 Cooley-Tukey FFT
            if (N % 2 != 0)
            {
                throw new Exception("N is not a power of 2");
            }

            // fft of even terms
            Complex[] even = new Complex[N / 2];
            for (int k = 0; k < N / 2; k++)
            {
                even[k] = x[2 * k];
            }
            Complex[] q = FFT(even);

            // fft of odd terms
            Complex[] odd = even; // reuse the array
            for (int k = 0; k < N / 2; k++)
            {
                odd[k] = x[2 * k + 1];
            }
            Complex[] r = FFT(odd);

            // combine
            Complex[] y = new Complex[N];
            for (int k = 0; k < N / 2; k++)
            {
                double kth = -2 * k * Math.PI / N;
                Complex wk = new Complex(Math.Cos(kth), Math.Sin(kth));
                y[k] = q[k] + (wk * (r[k]));
                y[k + N / 2] = q[k] - (wk * (r[k]));
            }
            return y;
        }

        // compute the inverse FFT of x[], assuming its length is a power of 2
        public Complex[] InverseFFT(Complex[] x)
        {
            int N = x.Length;
            Complex[] y = new Complex[N];

            // take conjugate
            for (int i = 0; i < N; i++)
            {
                y[i] = x[i].Conjugate;
            }

            // compute forward FFT
            y = FFT(y);

            // take conjugate again
            for (int i = 0; i < N; i++)
            {
                y[i] = y[i].Conjugate;
            }

            // divide by N
            for (int i = 0; i < N; i++)
            {
                y[i] = y[i] * (1.0 / N);
            }

            return y;

        }

        /*
         * This computes an in-place complex-to-complex FFT x and y are the real and
         * imaginary arrays of 2^m points. dir = 1 gives forward transform dir = -1
         * gives reverse transform
         */
        public void FFT(int dir, int m, double[] x, double[] y)
        {
            int n, i, i1, j, k, i2, l, l1, l2;
            double c1, c2, tx, ty, t1, t2, u1, u2, z;

            // Calculate the number of points
            n = 1;
            for (i = 0; i < m; i++)
                n *= 2;

            // Do the bit reversal
            i2 = n >> 1;
            j = 0;
            for (i = 0; i < n - 1; i++)
            {
                if (i < j)
                {
                    tx = x[i];
                    ty = y[i];
                    x[i] = x[j];
                    y[i] = y[j];
                    x[j] = tx;
                    y[j] = ty;
                }
                k = i2;
                while (k <= j)
                {
                    j -= k;
                    k >>= 1;
                }
                j += k;
            }

            // Compute the FFT
            c1 = -1.0;
            c2 = 0.0;
            l2 = 1;
            for (l = 0; l < m; l++)
            {
                l1 = l2;
                l2 <<= 1;
                u1 = 1.0;
                u2 = 0.0;
                for (j = 0; j < l1; j++)
                {
                    for (i = j; i < n; i += l2)
                    {
                        i1 = i + l1;
                        t1 = u1 * x[i1] - u2 * y[i1];
                        t2 = u1 * y[i1] + u2 * x[i1];
                        x[i1] = x[i] - t1;
                        y[i1] = y[i] - t2;
                        x[i] += t1;
                        y[i] += t2;
                    }
                    z = u1 * c1 - u2 * c2;
                    u2 = u1 * c2 + u2 * c1;
                    u1 = z;
                }
                c2 = Math.Sqrt((1.0 - c1) / 2.0);
                if (dir == 1)
                    c2 = -c2;
                c1 = Math.Sqrt((1.0 + c1) / 2.0);
            }

            // Scaling for forward transform
            if (dir == 1)
            {
                for (i = 0; i < n; i++)
                {
                    x[i] /= n;
                    y[i] /= n;
                }
            }

        }

        private bool IsPowerOf2(int width, int height)
        {
            // check if width and height are the same
            if (width != height)
                return false;

            int n = 1;

            // find if the width is a power of 2
            while (n < width)
                n *= 2;

            if (n != width)
                return false;

            n = 1;

            // find if the height is a power of 2
            while (n < height)
                n *= 2;

            if (n != height)
                return false;
            return true;
        }
    }
}
