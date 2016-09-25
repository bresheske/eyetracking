using EyeTracking.Core.Filters;
using EyeTracking.Test.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Test.Tests
{
    public class BaseTestFixture
    {
        private CropFilter _croppingfilter;
        private HueFilter _huefilter;
        private BlobCounterFilter _blobcounter;

        public BaseTestFixture()
        {
            _croppingfilter = new CropFilter();
            _huefilter = new HueFilter();
            _blobcounter = new BlobCounterFilter();
        }

        private void SetSettings(TestCriteria criteria)
        {
            _croppingfilter.X = criteria.Settings.CropX;
            _croppingfilter.Y = criteria.Settings.CropY;
            _croppingfilter.Width = criteria.Settings.CropWidth;
            _croppingfilter.Height = criteria.Settings.CropHeight;
            _huefilter.TargetHue = criteria.Settings.IrisHue;
            _huefilter.MaxDeltaHue = 35;
            _blobcounter.MaxHeight = (int)(_croppingfilter.Height / 4f);
            _blobcounter.MinHeight = (int)(_croppingfilter.Height / 15f);
            _blobcounter.MaxWidth = (int)(_croppingfilter.Width / 4f);
            _blobcounter.MinWidth = (int)(_croppingfilter.Width / 15f);
        }

        protected TestResult RunTest(TestCriteria criteria)
        {
            var result = new TestResult();
            SetSettings(criteria);

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            foreach(var t in criteria.TestImages)
            {
                var b = (Bitmap)Bitmap.FromFile(t.ImageLocation);
                var fullframe = new Rectangle(0, 0, b.Width, b.Height);
                b = _croppingfilter.Apply(b);
                //b.Save(@"D:\Eyes\Ciera\Single\crop.jpg");
                b = _huefilter.Apply(b);
                //b.Save(@"D:\Eyes\Ciera\Single\hue.jpg");
                var eyes = _blobcounter
                    .GetBlobPositions(b)
                    .ToArray();

                result.NumLeftEyeTotal++;
                result.NumRightEyeTotal++;

                if (eyes.Length > 0)
                {
                    //var eyepos = ProjectPoint(new Rectangle(_croppingfilter.X, _croppingfilter.Y, _croppingfilter.Width, _croppingfilter.Height), new Point(eyes[0].X, eyes[0].Y), fullframe);
                    //var eyepos = PlacePoint(new Point(eyes[0].X, eyes[0].Y), fullframe);
                    var eyepos = PlacePointOntoLargerFrame(new Rectangle(_croppingfilter.X, _croppingfilter.Y, _croppingfilter.Width, _croppingfilter.Height), new Point(eyes[0].X, eyes[0].Y));
                    var xerror = (float)Math.Abs(t.LeftEyeX - eyepos.X) / (float)fullframe.Width;
                    var yerror = (float)Math.Abs(t.LeftEyeY - eyepos.Y) / (float)fullframe.Height;
                    result.TotalLeftEyeX += t.LeftEyeX;
                    result.TotalLeftEyeY += t.LeftEyeY;
                    result.TotalLeftEyeXError += xerror;
                    result.TotalLeftEyeYError += yerror;
                }
                else
                {
                    result.TotalLeftEyeXError += 1f;
                    result.TotalLeftEyeYError += 1f;
                    result.TotalLeftEyeNotFound++;
                }

                if (eyes.Length > 1)
                {
                    //var eyepos = ProjectPoint(new Rectangle(_croppingfilter.X, _croppingfilter.Y, _croppingfilter.Width, _croppingfilter.Height), new Point(eyes[1].X, eyes[1].Y), fullframe);
                    var eyepos = PlacePointOntoLargerFrame(new Rectangle(_croppingfilter.X, _croppingfilter.Y, _croppingfilter.Width, _croppingfilter.Height), new Point(eyes[1].X, eyes[1].Y));
                    var xerror = (float)Math.Abs(t.RightEyeX - eyepos.X) / (float)fullframe.Width;
                    var yerror = (float)Math.Abs(t.RightEyeY - eyepos.Y) / (float)fullframe.Height;
                    result.TotalRightEyeX += t.RightEyeX;
                    result.TotalRightEyeY += t.RightEyeY;
                    result.TotalRightEyeXError += xerror;
                    result.TotalRightEyeYError += yerror;
                }
                else
                {
                    result.TotalRightEyeXError += 1f;
                    result.TotalRightEyeYError += 1f;
                    result.TotalRightEyeNotFound++;
                }


                //b = _blobcounter.Apply(b);
                //b.Save(Path.Combine(@"D:\Eyes\output", Path.GetFileName(t.ImageLocation)));

                b.Dispose();
            }

            sw.Stop();
            result.TotalSeconds = sw.ElapsedMilliseconds / 1000d;

            return result;
        }

        private System.Drawing.Point PlacePoint(System.Drawing.Point originalpoint,
            System.Drawing.Rectangle newframe)
        {
            return new System.Drawing.Point(
                originalpoint.X - newframe.X,
                originalpoint.Y - newframe.Y
                );
        }

        private System.Drawing.Point ProjectPoint(System.Drawing.Rectangle originalframe,
            System.Drawing.Point originalpoint,
            System.Drawing.Rectangle newframe)
        {
            var xratedelta = (float)originalpoint.X / originalframe.Width;
            var yratedelta = (float)originalpoint.Y / originalframe.Height;
            return new System.Drawing.Point()
            {
                X = (int)(xratedelta * newframe.Width),
                Y = (int)(yratedelta * newframe.Height)
            };
        }

        private System.Drawing.Point PlacePointOntoLargerFrame(System.Drawing.Rectangle originalframe,
            System.Drawing.Point originalpoint)
        {
            return new System.Drawing.Point()
            {
                X = (int)originalframe.X + originalpoint.X,
                Y = (int)originalframe.Y + originalpoint.Y
            };
        } 

        protected void PrintResults(TestCriteria test, TestResult results)
        {
            var avg = results.TotalSeconds / test.TestImages.Count;

            var res = ((results.TotalLeftEyeXError / (results.NumLeftEyeTotal))
                + (results.TotalLeftEyeYError / (results.NumLeftEyeTotal))
                + (results.TotalRightEyeXError / (results.NumRightEyeTotal))
                + (results.TotalRightEyeYError / (results.NumRightEyeTotal)))
                / 4f;

            var fps = test.TestImages.Count / results.TotalSeconds;

            Console.Out.WriteLine("Test Completed. Run time: {0:0.000} seconds, Average per Image: {1:0.000} seconds, Total Images: {2}", results.TotalSeconds, avg, test.TestImages.Count);
            Console.Out.WriteLine("Frames Per Second: {0:0.00}.", fps);
            Console.Out.WriteLine("Average Error: {0:p}, Accuracy: {1:p}.", res, 1 - res);
            Console.Out.WriteLine("Eyes Not Found: {0}, {1}.", results.TotalLeftEyeNotFound, results.TotalRightEyeNotFound);
        }
    }
}
