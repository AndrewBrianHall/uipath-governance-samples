using SampleGovernanceRules.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace SampleGovernanceRules.ActivityRules
{
    internal static class PropertySettingsRule
    {
        // This should be as unique as possible, and should follow the naming convention.
        public const string RuleId = "ORG-USG-001";
        private const string ConfigParameterKey = "configuration";
        private const string TestConfigValue = "[{ActivityName:\"UiPath.Mail.Activities.Business.SendMailX\", PropertyName:\"Is draft\", ExpectedValue:\"True\"}]";

        internal static Rule<IActivityModel> Get()
        {

            var rule = new Rule<IActivityModel>(Strings.ORG_USG_001_Name, RuleId, Inspect)
            {
                RecommendationMessage = Strings.ORG_USG_001_Recommendation,
                ErrorLevel = TraceLevel.Warning,
                ApplicableScopes = new List<string> { RuleConstants.BusinessRule }
            };
            rule.Parameters.Add(ConfigParameterKey, new Parameter()
            {
                Key = ConfigParameterKey,
                LocalizedDisplayName = Strings.ConfigurationSettingsLabel,
                DefaultValue = TestConfigValue
            });
            return rule;
        }

        private static InspectionResult Inspect(IActivityModel activity, Rule ruleInstance)
        {
            var result = new InspectionResult();
            var settings = GetSettingsEntries(ruleInstance);

            if (settings == null)
            {
                return result;
            }
            foreach (var setting in settings)
            {
                if (!string.IsNullOrEmpty(activity.Type) && activity.Type.StartsWith(setting.ActivityName, StringComparison.OrdinalIgnoreCase))
                {
                    var property = RulesHelper.GetProperty(activity.Properties, setting.PropertyName);
                    if (property != null)
                    {
                        StringComparison caseComparison = setting.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

                        if (!property.DefinedExpression.Equals(setting.ExpectedValue, caseComparison))
                        {
                            result.Messages.Add(string.Format(Strings.ORG_USG_001_Message, activity.DisplayName));
                        }
                    }
                }
            }

            if (result.Messages.Count > 0)
            {
                result.HasErrors = true;
                result.ErrorLevel = TraceLevel.Error;
                result.RecommendationMessage = ruleInstance.RecommendationMessage;
            }

            return result;
        }

        private static ActivityPropertySettingsEntry[] GetSettingsEntries(Rule ruleInstance)
        {
            if (ruleInstance.Parameters.TryGetValue(ConfigParameterKey, out var parameter) &&
                !string.IsNullOrEmpty(parameter.Value))
            {
                var entry = Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityPropertySettingsEntry[]>(parameter.Value);
                return entry;
            }

            return null;
        }
    }
}
