using SampleGovernanceRules.ActivityRules;
using SampleGovernanceRules.ProjectRules;
using UiPath.Studio.Activities.Api;
using UiPath.Studio.Activities.Api.Analyzer;
//using UiPath.Studio.RulesLibrary.Rules.Naming;

namespace SampleGovernanceRules
{
    public class RegisterAnalyzerConfiguration : IRegisterAnalyzerConfiguration
    {
        public void Initialize(IAnalyzerConfigurationService workflowAnalyzerConfigService)
        {
            workflowAnalyzerConfigService.AddRule(PropertySettingsRule.Get());
            //workflowAnalyzerConfigService.AddRule(ActivitySettingsRule.Get());
            workflowAnalyzerConfigService.AddRule(PackageVersionsRule.Get());
        }
    }
}
