using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleGovernanceRules.Models;
using SampleGovernanceRules.ProjectRules;

namespace SampleGovernanceRules.Tests
{
    [TestClass]
    public class PackageVersionsRuleTests
    {
        const string ComplextScenariosPackage = "UiPath.ComplexScenarios.Activities";
        const string ExcelPackage = "UiPath.Excel.Activities";
        const string UiAutomationNextPackage = "UiPath.UIAutomationNext.Activities";

        static readonly string SettingsJson = "[" +
            $"{{Name:\"{UiAutomationNextPackage}\", MinVersion:\"20.4.0-beta.853134\"}}," +
            $"{{Name:\"{ExcelPackage}\", MinVersion:\"1.4.2\", MaxVersion:\"1.4.2\"}}," +
            "{Name:\"UiPath.Word.Activities\", MinVersion:\"1.4.2\"}," +
            $"{{Name:\"{ComplextScenariosPackage}\", MinVersion:\"1.0.0\", AllowPrerelease:\"True\"}}" +
            "]";

        bool _settingsParsed;
        Dictionary<string, PackageVersionEntry> _settings;

        public PackageVersionsRuleTests()
        {
            _settingsParsed = PackageVersionsRule.TryParseSettingsJson(SettingsJson, out _settings);
        }

        [TestMethod]
        public void AboveMaxVersion()
        {
            bool aboveMaxVersion = PackageVersionsRule.IsPackageValid(ExcelPackage, "1.5.0", _settings, true);

            Assert.IsFalse(aboveMaxVersion);
        }
        
        [TestMethod]
        public void BelowMinVersion()
        {
            bool belowMinVersion = PackageVersionsRule.IsPackageValid(UiAutomationNextPackage, "20.3.0", _settings, true);

            Assert.IsFalse(belowMinVersion);
        }
        
        [TestMethod]
        public void ExactVersion()
        {
            bool exactVersion = PackageVersionsRule.IsPackageValid(ExcelPackage, "1.4.2", _settings, true);

            Assert.IsTrue(exactVersion);
        }

        [TestMethod]
        public void ParsingStatus()
        {
            Assert.IsTrue(_settingsParsed);
            Assert.IsTrue(_settings.ContainsKey("UiPath.UIAutomationNext.Activities".ToLowerInvariant()));
            Assert.IsFalse(_settings.ContainsKey("UiPath.DoesNoteExist"));
        }

        [TestMethod]
        public void InvalidConfig()
        {
            const string badSettingsJson = "Name:\"UiPath.UIAutomation.Next.Activities\", MinVersion:\"20.4.0\"";
            bool success = PackageVersionsRule.TryParseSettingsJson(badSettingsJson, out Dictionary<string, PackageVersionEntry> settings);

            Assert.IsFalse(success);
            Assert.IsNull(settings);
        }

        [TestMethod]
        public void AllowPrereleasePackage()
        {
            bool globallyPermitted = PackageVersionsRule.IsPackageValid(UiAutomationNextPackage, "20.4.0-beta.853134", _settings, true);
            bool individuallyPermitted = PackageVersionsRule.IsPackageValid(ComplextScenariosPackage, "1.0.2-beta.852560", _settings, false);

            Assert.IsTrue(globallyPermitted);
            Assert.IsTrue(individuallyPermitted);
        }

        [TestMethod]
        public void NoPrereleasePackage()
        {
            bool nonPrereleaseVersion = PackageVersionsRule.IsPackageValid(UiAutomationNextPackage, "20.4.0", _settings, true);
            bool prereleaseVersion = PackageVersionsRule.IsPackageValid(UiAutomationNextPackage, "20.3.0", _settings, false);

            Assert.IsTrue(nonPrereleaseVersion);
            Assert.IsFalse(prereleaseVersion);
        }
       
    }
}
