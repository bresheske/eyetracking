using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Objects
{
    public class Complex
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public double Magnitude 
        { 
            get
            {
                return Math.Sqrt(Math.Pow(Real, 2) + Math.Pow(Imaginary, 2));
            }
        }

        public double Phase
        {
            get
            {
                return Math.Atan(Imaginary / Real);
            }
        }
        public Complex Conjugate
        {
            get
            {
                return new Complex(Real, -Imaginary);
            }
        }
        
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        public static Complex operator *(Complex c1, double val)
        {
            return new Complex(c1.Real * val, c1.Imaginary * val);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Real * c2.Real - c1.Imaginary
                * c2.Imaginary, c1.Real * c2.Imaginary
                + c1.Imaginary * c2.Real);
        }

        public static Complex operator /(Complex c1, double val)
        {
            return c1 * (1 / val);
        }

        public Complex(Complex comp)
        {
            Real = comp.Real;
            Imaginary = comp.Imaginary;
        }

        public Complex(double r, double i)
        {
            Real = r;
            Imaginary = i;
        }

        public Complex(double r) : this(r, 0) { }
        public Complex() : this(0, 0) { }

        public string toString()
        {
            if (Imaginary == 0)
                return Real.ToString();
            if (Real == 0)
                return Imaginary + "i";
            if (Imaginary < 0)
                return Real + " - " + (-Imaginary) + "i";
            return Real + " + " + Imaginary + "i";
        }
    }
}
