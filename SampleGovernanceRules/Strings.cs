using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGovernanceRules
{
    public class Strings
    {
        public const string ORG_USG_001_Name = "Mail Property Requirements";
        public const string ORG_USG_001_Recommendation = "Follow org guidelines for property settings.";
        public const string ORG_USG_001_Message = "All emails created by {0} activities must have 'Is draft' set to true";

        public const string ConfigurationSettingsLabel = "Rule Configuration";
        public const string ORG_USG_002_Name = "UI Automation Setting Requirements";
        public const string ORG_USG_002_Recommendation = "Follow org guidelines for project's UI automation delays.";
        public const string ORG_USG_002_Message = "Your project's activity setting '{0}' must have a value of at least {1} seconds";
    }
}
