using SampleGovernanceRules.ActivityRules.WorkflowHelpers;
using SampleGovernanceRules.Extensions;
using SampleGovernanceRules.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace SampleGovernanceRules.ActivityRules
{
    internal static class PropertySettingsRule
    {
        public const string RuleId = "ORG-USG-001";
        private const string ConfigParameterKey = "configuration";

        internal static Rule<IWorkflowModel> Get()
        {

            var rule = new Rule<IWorkflowModel>(Strings.ORG_USG_001_Name, RuleId, Inspect)
            {
                RecommendationMessage = Strings.ORG_USG_001_Recommendation,
                ErrorLevel = TraceLevel.Error,
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

        private static InspectionResult Inspect(IWorkflowModel workflow, Rule ruleInstance)
        {
            var result = new InspectionResult();
            List<ActivityPropertySetting> settings = GetSettingsEntries(ruleInstance);

            if (settings == null)
            {
                return result;
            }

            var propertiesProcessor = new PropertyValueValidator(settings);
            var workflowProcessor = new WorkflowProcessor(propertiesProcessor);
            workflowProcessor.WalkWorkflow(workflow.Root);
            

            if (propertiesProcessor.Messages.Count > 0)
            {
                result.HasErrors = true;
                result.ErrorLevel = TraceLevel.Error;
                result.RecommendationMessage = ruleInstance.RecommendationMessage;
                result.InspectionMessages = propertiesProcessor.Messages;
            }

            return result;
        }

        private static List<ActivityPropertySetting> GetSettingsEntries(Rule ruleInstance)
        {
            if (ruleInstance.Parameters.TryGetValue(ConfigParameterKey, out var parameter) &&
                !string.IsNullOrEmpty(parameter.Value))
            {
                return ParseSettingsEntry(parameter.Value);
            }

            return null;
        }

        internal static List<ActivityPropertySetting> ParseSettingsEntry(string configEntry)
        {
            var settings = new List<ActivityPropertySetting>();

            var entries = configEntry?.Split(';');

            if (entries == null)
            {
                return settings;
            }

            foreach (var entry in entries)
            {
                if (string.IsNullOrEmpty(entry))
                {
                    continue;
                }

                var settingsModel = new ActivityPropertySetting();
                var properties = entry.Split(',');
                foreach (var property in properties)
                {
                    var propertyParts = property.Split(':');
                    settingsModel.SetProperty(propertyParts[0], propertyParts[1]);
                }
                settings.Add(settingsModel);
            }

            return settings;
        }
    }

    internal class PropertyValueValidator : IActivityValidator
    {
        public List<InspectionMessage> Messages { get; protected set; } = new List<InspectionMessage>();

        protected readonly List<ActivityPropertySetting> _propertySettings;

        public PropertyValueValidator(List<ActivityPropertySetting> propertySettings)
        {
            _propertySettings = propertySettings;
        }

        protected bool ActivityBreaksRule(string activityType, IReadOnlyCollection<IPropertyModel> properties, out string message)
        {
            message = null;
            var matchingSettings = _propertySettings.Where(s => s.ActivityTypeMatches(activityType));
            if (matchingSettings.Count() > 0)
            {
                var violatingProperties = properties.Where(p => matchingSettings.Any(s => s.PropertyNameMatches(p.DisplayName) && !s.ValueMatches(p.DefinedExpression)));
                if(violatingProperties.Count() > 0)
                {
                    message = string.Format(Strings.ORG_USG_001_Message, violatingProperties.FirstOrDefault().DisplayName, activityType);
                    return true;
                }
            }

            return false;
        }

        public void ProcessActivity(IActivityModel activity)
        {
            var activityType = activity.Type.SubstringBefore(',');
            if (ActivityBreaksRule(activityType, activity.Properties, out string message))
            {
                this.Messages.Add(new ActivityMessage
                {
                    Message = message, 
                    ActivityId = activity.GetActivityId()
                });
            }
        }
    }

}
