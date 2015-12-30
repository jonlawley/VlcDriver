using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;

namespace VlcDriverTests
{
    [TestFixture]
    public class SlnConfigurationTests
    {

        /// <summary>
        /// In order that VlcDriver solution can be used as a submodule, to keep nuget packages referenced correctly,
        /// we must reference Dlls by Solution reference
        /// </summary>
        [Test]
        public void EnsureAllNugetPackageReferencesAreViaSolutionReference()
        {
            var badProjReferences = new List<string>();

            var vlcDriverProjPath = Path.Combine(TestUtilities.GetProjectDir(), @"..\VLCDriver\VLCDriver.csproj");
            Assert.IsTrue(File.Exists(vlcDriverProjPath), "Problem with test, vlc driver csproj file not found in expected location");

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(vlcDriverProjPath);

            XmlNamespaceManager mgr = new XmlNamespaceManager(xmldoc.NameTable);
            mgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

            var xmlNodeList = xmldoc.SelectNodes("//x:Reference", mgr);
            if (xmlNodeList != null)
            {
                foreach (XmlNode item in xmlNodeList)
                {
                    var innerText = item.InnerText;
                    if (innerText.ToUpper().Contains(".DLL") && !innerText.StartsWith("$(SolutionDir)"))
                    {
                        badProjReferences.Add(innerText);
                    }
                }
            }

            Assert.IsEmpty(badProjReferences, "The following assembly references did not use a $(SolutionDir) reference, Nuget will fail on other slns using vlcdriver proj. Example: '$(SolutionDir)\\packages\\...'");
        }
    }
}