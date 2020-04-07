﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGovernanceRules
{
    public class Strings
    {
        public const string ConfigurationSettingsLabel = "Rule Configuration";
        public const string AllowPrereleasePackagesLabel = "Allow prelease packages (True/False)";

        public const string ORG_USG_001_Name = "Activity Property Requirements";
        public const string ORG_USG_001_Recommendation = "Follow org guidelines for property settings.";
        public const string ORG_USG_001_Message = "'{0}' property of the '{1}' activity does not meet your organization's guidelines";

        public const string ORG_USG_002_Name = "Package Version Requirements";
        public const string ORG_USG_002_Recommendation = "Follow org guidelines for referenced package versions.";
        public const string ORG_USG_002_Message = "The referenced package '{0}' does not meet your organization's version requirements";
        
        public const string ORG_USG_003_Name = "UI Automation Setting Requirements";
        public const string ORG_USG_003_Recommendation = "Follow org guidelines for project's UI automation delays.";
        public const string ORG_USG_003_Message = "Your project's activity setting '{0}' must have a value of at least {1} seconds";
    }
}
