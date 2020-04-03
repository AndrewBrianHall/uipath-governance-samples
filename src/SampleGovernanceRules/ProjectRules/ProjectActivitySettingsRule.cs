using SampleGovernanceRules.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace SampleGovernanceRules.ProjectRules
{
    //WARNING: This rule relies on undocumented and therefore unsupported functionality.
    //Currently there is no officially supported way to access project level activity settings in a Workflow Analyzer Rule.
    //As such this rule demonstrates how it can be accomplished with UiPath Studio 20.4 by parsing the contents of the .settings directory
    //But the diretory structure and file formats are subject to change without notice in future versions.
    internal static class ProjectActivitySettingsRule
    {
        public const string RuleId = "ORG-USG-003";
        private const string ConfigParameterKey = "configuration";
        private const string TestConfigValue = "[{Package:\"UiPath.UIAutomationNext.Activities\", Setting:\"UiPath.UIAutomationNext.Activities.Generic.DelayAfter\", MinValue:\"0.3\"}]";

        internal static Rule<IProjectModel> Get()
        {
            var rule = new Rule<IProjectModel>(Strings.ORG_USG_003_Name, RuleId, Inspect)
            {
                RecommendationMessage = Strings.ORG_USG_003_Recommendation,
                ErrorLevel = TraceLevel.Warning,
                ApplicableScopes = new List<string> { RuleConstants.BusinessRule }
            };
            rule.Parameters.Add(ConfigParameterKey, new Parameter()
            {
                Key = ConfigParameterKey,
                LocalizedDisplayName = Strings.ConfigurationSettingsLabel,
                DefaultValue = string.Empty
            });

            return rule;
        }

        private static InspectionResult Inspect(IProjectModel project, Rule ruleInstance)
        {
            var result = new InspectionResult();
            var settings = GetSettingsEntries(ruleInstance);

            if(settings == null)
            {
                return result;
            }

            var settingsModel = ProjectActivitySettingsModel.LoadSettings(project.ProjectFilePath);

            foreach(var setting in settings)
            {
                var delayBeforeSetting = settingsModel.GetSettingValues(setting.Setting);
                if (PackageReferenced(setting.Package, project.Dependencies) && !CheckDecimalSetting(delayBeforeSetting, setting))
                {
                    result.Messages.Add(string.Format(Strings.ORG_USG_003_Message, "Delay after", setting.MinValue));
                }
            }

            if(result.Messages.Count > 0)
            {
                result.HasErrors = true;
                result.ErrorLevel = TraceLevel.Error;
                result.RecommendationMessage = ruleInstance.RecommendationMessage;
            }
            
            return result;
        }

        private static bool CheckDecimalSetting(ActivitySettingValues value, PackageSettingsEntry settings)
        {
            bool hasValidDebugValue = decimal.TryParse(value.DebugValue, out decimal debugValue);
            bool hasValidReleaseValue = decimal.TryParse(value.ProductionValue, out decimal releaseValue);
            bool hasValidSettingValue = decimal.TryParse(settings.MinValue, out decimal minValue);

            return hasValidSettingValue && hasValidDebugValue && debugValue >= minValue && hasValidReleaseValue && releaseValue >= minValue;
        }

        private static PackageSettingsEntry[] GetSettingsEntries(Rule ruleInstance)
        {
            if (ruleInstance.Parameters.TryGetValue(ConfigParameterKey, out var parameter) &&
                !string.IsNullOrEmpty(parameter.Value))
            {
                var entry = Newtonsoft.Json.JsonConvert.DeserializeObject<PackageSettingsEntry[]>(parameter.Value);
                return entry;
            }

            return null;
        }

        private static bool PackageReferenced(string packageName, IReadOnlyCollection<IDependency> dependencies)
        {
            //Expected package isn't referenced, setting won't exist.
            if (dependencies.Where(d => !string.IsNullOrEmpty(d.Name) && d.Name.Equals(packageName, System.StringComparison.OrdinalIgnoreCase)).Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
