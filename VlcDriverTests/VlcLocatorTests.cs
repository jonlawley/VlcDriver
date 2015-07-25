using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCDriver;

namespace VlcDriverTests
{
    [TestFixture]
    public class VlcLocatorTests
    {
        [Test]
        public void TestCorrectLocationComesBackOnNormalSystem()
        {
            var vlcLocation = new VlcLocator();
            var actualLocation = vlcLocation.Location;
            Assert.IsTrue(File.Exists(actualLocation));
        }
    }
}
