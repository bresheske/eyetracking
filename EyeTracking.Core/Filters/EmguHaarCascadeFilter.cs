using AForge.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class EmguHaarCascadeFilter : IFilter
    {
        HaarCascade haar;
        public EmguHaarCascadeFilter()
        {
            haar = new HaarCascade(@"C:\Emgu\emgucv-windows-universal-gpu 2.4.9.1847\opencv\data\haarcascades\haarcascade_eye.xml");
        }

        public Bitmap Apply(Bitmap image)
        {
            using (var img = new Image<Bgr, Byte>(image))
            using (var grey = img.Convert<Gray, Byte>())
            using (var gfx = Graphics.FromImage(image))
            {
                var eyes = grey.DetectHaarCascade(haar)[0];
                foreach (var e in eyes)
                {
                    gfx.DrawRectangle(Pens.Blue, e.rect.X, e.rect.Y, e.rect.Width, e.rect.Height);
                }
            }

            return image;
        }

        public override string ToString()
        {
            return "EmguHaarCascade";
        }

        public object Clone()
        {
            return new EmguHaarCascadeFilter();
        }
    }
}
