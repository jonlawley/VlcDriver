using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using VLCDriver;

namespace VlcDriverTests
{
    [TestFixture]
    public class StatusParserTests
    {
        [Test]
        public void EnsurePositionCanBeCorrectlyParsedFromXml()
        {
            var statusFile = TestUtilities.GetTestFile("status.xml");
            var statusXml = File.ReadAllText(statusFile.FullName);

            var statusSource = MockRepository.GenerateMock<IVlcStatusSource>();
            statusSource.Expect(x => x.GetXml()).Return(statusXml);

            var parser = new StatusParser
            {
                Xml = statusXml
            };
            parser.Parse();
            Assert.AreEqual(0.01635168120265, parser.Position);
        }
    }
}