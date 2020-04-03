using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using UiPath.Studio.RulesLibrary.Rules.Naming;

namespace SampleGovernanceRules.Models
{
    public class ProjectActivitySettingsModel
    {
        Dictionary<string, string> DebugSettings { get; set; } = new Dictionary<string, string>();
        Dictionary<string, string> ReleaseSettings { get; set; } = new Dictionary<string, string>();

        protected ProjectActivitySettingsModel()
        {

        }

        public ActivitySettingValues GetSettingValues(string name)
        {
            this.DebugSettings.TryGetValue(name, out string debugValue);
            this.ReleaseSettings.TryGetValue(name, out string releaseValue);
            var settings = new ActivitySettingValues(name, debugValue, releaseValue);
            return settings;
        }

        public static ProjectActivitySettingsModel LoadSettings(string projectFilePath)
        {
            var model = new ProjectActivitySettingsModel();
            var projectFolder = Path.GetDirectoryName(projectFilePath);
            var debugSettingsFolder = Path.Combine(projectFolder, ".settings", "Debug");
            var releaseSettingsFolder = Path.Combine(projectFolder, ".settings", "Release");
            var debugSettingFiles = GetJsonFiles(debugSettingsFolder);
            var releaseSettingsFiles = GetJsonFiles(releaseSettingsFolder);
            foreach(var debugFile in debugSettingFiles)
            {
                model.parseFile(debugFile, model.DebugSettings);
            }
            foreach(var releaseFile in releaseSettingsFiles)
            {
                model.parseFile(releaseFile, model.ReleaseSettings);
            }

            return model;
        }

        protected void parseFile(string file, Dictionary<string, string> dictionary)
        {
            using(StreamReader jsonStream = File.OpenText(file))
            {
                using (JsonTextReader reader = new JsonTextReader(jsonStream))
                {
                    var jObj = JToken.ReadFrom(reader);
                    foreach(var token in jObj.Children())
                    {
                        JProperty property = (JProperty)token;
                        dictionary.Add(property.Name, property.Value.Value<string>());
                    }

                }
            }
        }

        private static string[] GetJsonFiles(string path)
        {
            return Directory.GetFiles(path, "*.json");
        }
    }
}
