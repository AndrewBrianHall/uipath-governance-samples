using System.Collections.Generic;
using UiPath.Studio.Analyzer.Models;

namespace SampleGovernanceRules.ActivityRules.WorkflowHelpers
{
    internal class WorkflowProcessor
    {
        private readonly HashSet<IActivityModel> _visitedActivities = new HashSet<IActivityModel>();
        private readonly IActivityValidator _processor;

        public WorkflowProcessor(IActivityValidator processor)
        {
            _processor = processor;
        }

        public void WalkWorkflow(IActivityModel currentNode)
        {
            if (currentNode == null || _visitedActivities.Contains(currentNode))
            {
                return;
            }

            _visitedActivities.Add(currentNode);
            _processor.ProcessActivity(currentNode);

            foreach (var child in currentNode.Children)
            {
                WalkWorkflow(child);
            }
        }
    }

}
