using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SampleGovernanceRules.Models
{
    internal class ActivityPropertySetting
    {
        public string Activity { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
        public bool IgnoreCase { get; set; } = true;

        protected string _activityRegex;
        protected string _propertyRegex;
        protected string _valueRegex;

        public void SetProperty(string property, string value)
        {
            property = property.ToLowerInvariant();

            switch (property)
            {
                case "property":
                    this.Property = value;
                    _propertyRegex = Regex.Escape(value).Replace("\\*", ".*");
                    break;
                case "activity":
                    this.Activity = value;
                    _activityRegex = Regex.Escape(value).Replace("\\*", ".*");
                    break;
                case "value":
                    this.Value = value;
                    _valueRegex = Regex.Escape(value).Replace("\\*", ".*");
                    break;
                case "ignorecase":
                    bool boolValue = true;
                    bool.TryParse(value, out boolValue);
                    this.IgnoreCase = boolValue;
                    break;
            }
        }

        internal bool ActivityTypeMatches(string activityType)
        {
            return PropertyMatchesRegex(activityType,this.Activity, _activityRegex);
        }

        internal bool PropertyNameMatches(string property)
        {
            return PropertyMatchesRegex(property, this.Property, _propertyRegex);
        }

        internal bool ValueMatches(string expression)
        {
            return PropertyMatchesRegex(expression, this.Value, _valueRegex);
        }

        private static bool PropertyMatchesRegex(string input, string property, string regex, bool matchCase = false)
        {
            // An empty value does not match
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // An exact match
            if (property == input)
            {
                return true;
            }

            RegexOptions options = !matchCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            return Regex.IsMatch(input, $"^{regex}$", options);
        }

    }
}
