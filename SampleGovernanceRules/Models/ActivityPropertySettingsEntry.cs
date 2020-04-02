using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGovernanceRules.Models
{
    internal class ActivityPropertySettingsEntry
    {
        public string ActivityName { get; set; }
        public string PropertyName { get; set; }
        public string ExpectedValue { get; set; }
        public bool IgnoreCase { get; set; } = true;
    }
}
