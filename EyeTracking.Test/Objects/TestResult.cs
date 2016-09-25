using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Test.Objects
{
    public class TestResult
    {
        public float TotalLeftEyeXError { get; set; }
        public float TotalLeftEyeYError { get; set; }
        public float TotalRightEyeXError { get; set; }
        public float TotalRightEyeYError { get; set; }
        public int NumLeftEyeTotal { get; set; }
        public int NumRightEyeTotal { get; set; }
        public int TotalLeftEyeNotFound { get; set; }
        public int TotalRightEyeNotFound { get; set; }
        public int TotalLeftEyeX { get; set; }
        public int TotalLeftEyeY { get; set; }
        public int TotalRightEyeX { get; set; }
        public int TotalRightEyeY { get; set; }
        public double TotalSeconds { get; set; }
    }
}
