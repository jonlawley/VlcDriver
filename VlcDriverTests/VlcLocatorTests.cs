using System;
using NUnit.Framework;
using System.IO;
using VLCDriver;

namespace VlcDriverTests
{
    [TestFixture]
    public class VlcLocatorTests
    {
        [Test]
        public void TestCorrectLocationComesBackOnNormalWindowsSystem()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                //We cannot run this test
                return;
            }

            var vlcLocation = new VlcLocator();
            var actualLocation = vlcLocation.Location;

            Assert.IsTrue(File.Exists(actualLocation));
            var info = new FileInfo(actualLocation);
            Assert.True(info.Exists);
            Assert.AreEqual("vlc", Path.GetFileNameWithoutExtension(info.Name));
        }
    }
}
