//using UiPath.Studio.RulesLibrary.Rules.Naming;

namespace SampleGovernanceRules.Models
{
    public class ActivitySettingValues
    {
        public string Name { get; protected set; }
        public string DebugValue { get; protected set; }
        public string ProductionValue { get; protected set; }

        public ActivitySettingValues(string name, string debugValue, string releaseValue)
        {
            this.Name = name;
            this.DebugValue = debugValue;
            this.ProductionValue = releaseValue;
        }

    }
}
