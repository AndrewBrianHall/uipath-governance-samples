using System.Collections.Generic;
using System.Linq;
using UiPath.Studio.Analyzer.Models;
//using UiPath.Studio.RulesLibrary.Rules.Naming;

namespace SampleGovernanceRules
{
    internal static class RulesHelper
    {
        public static IPropertyModel GetProperty(IReadOnlyCollection<IPropertyModel> properties, string propertyName) =>
            properties?.FirstOrDefault(a => Equals(a.DisplayName, propertyName));
    }
}
