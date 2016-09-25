using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Test.Objects
{
    public class TestImageModel
    {
        public string ImageLocation { get; set; }
        public int LeftEyeX { get; set; }
        public int LeftEyeY { get; set; }
        public int RightEyeX { get; set; }
        public int RightEyeY { get; set; }
    }
}
