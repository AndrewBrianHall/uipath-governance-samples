using System.Collections.Generic;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace SampleGovernanceRules.ActivityRules.WorkflowHelpers
{
    internal interface IActivityValidator
    {
        List<InspectionMessage> Messages { get; }
        void ProcessActivity(IActivityModel activity);
    }

}
