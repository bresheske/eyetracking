using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class FaceCascadeFilter : IFilter
    {

        public FaceCascadeFilter()
        {

        }

        public Bitmap Apply(Bitmap image)
        {
            var cas = new FaceHaarCascade();
            var haar = new HaarObjectDetector(cas, 30);

            haar.SearchMode = ObjectDetectorSearchMode.Average;
            haar.ScalingFactor = 1.5f;
            haar.ScalingMode = ObjectDetectorScalingMode.GreaterToSmaller;
            haar.UseParallelProcessing = true;
            haar.Suppression = 3;

            var img = new Bitmap(image);
            var rects = haar.ProcessFrame(img);

            using (var graphics = Graphics.FromImage(img))
            {
                foreach (var r in rects)
                {
                    graphics.DrawRectangle(Pens.Blue, r);
                }
            }
            return img;
        }

        public override string ToString()
        {
            return "FaceCascade";
        }

        public object Clone()
        {
            return new FaceCascadeFilter();
        }
    }
}
