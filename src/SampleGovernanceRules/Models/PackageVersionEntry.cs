using Semver;

namespace SampleGovernanceRules.Models
{
    internal class PackageVersionEntry
    {
        public string Name { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }

        public bool AllowPrerelease { get; set; } = false;

        public bool TryGetMaxSemVersion(out SemVersion semVersion)
        {
            return TryParseSemVersion(this.Max, out semVersion);
        }

        public bool TryGetMinSemVersion(out SemVersion semVersion)
        {
            return TryParseSemVersion(this.Min, out semVersion);
        }
        

        private static bool TryParseSemVersion(string version, out SemVersion semVersion)
        {
            if (!string.IsNullOrEmpty(version) && SemVersion.TryParse(version, out semVersion))
            {
                return true;
            }

            semVersion = null;
            return false;
        }


    }
}
