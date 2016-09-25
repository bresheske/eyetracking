using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EyeTracking.Core.Persistance
{
    public class InitializationSettings
    {

        public string FileName { get; set; }

        public InitializationSettings()
            : this("settings.json")
        {

        }

        public InitializationSettings(string filename)
        {
            FileName = filename;
        }

        public SettingsModel LoadFromFile()
        {
            return new JavaScriptSerializer().Deserialize<SettingsModel>(File.ReadAllText(FileName));
        }

        public void SaveToFile(SettingsModel model)
        {
            File.WriteAllText(FileName, new JavaScriptSerializer().Serialize(model));
        }
    }
}
