using EyeTracking.Core.Persistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EyeTracking.Test.Objects
{
    public class TestCriteria
    {
        public SettingsModel Settings { get; set; }
        public List<TestImageModel> TestImages { get; set; }

        public static TestCriteria FromJsonFile(string file)
        {
            return new JavaScriptSerializer()
                .Deserialize<TestCriteria>(File.ReadAllText(file));
        }
    }
}
