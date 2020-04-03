using Semver;

namespace SampleGovernanceRules.Models
{
    internal class PackageVersionEntry
    {
        public string Name { get; set; }
        public string MinVersion { get; set; }
        public string MaxVersion { get; set; }

        public bool AllowPrerelease { get; set; } = false;

        public bool TryGetMaxSemVersion(out SemVersion semVersion)
        {
            return TryParseSemVersion(this.MaxVersion, out semVersion);
        }

        public bool TryGetMinSemVersion(out SemVersion semVersion)
        {
            return TryParseSemVersion(this.MinVersion, out semVersion);
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
