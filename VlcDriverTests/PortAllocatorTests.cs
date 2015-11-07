using NLog;
using NUnit.Framework;
using Rhino.Mocks;
using VLCDriver;

namespace VlcDriverTests
{
    [TestFixture]
    public class PortAllocatorTests
    {
        [Test]
        public void EnsurePortAllocatorAlwaysAllocatesTheLowestPortNumber()
        {
            var allocator = new PortAllocator(MockRepository.GenerateMock<ILogger>())
            {
                StartPort = 1
            };
            Assert.AreEqual(1, allocator.NewPort());
            Assert.AreEqual(2, allocator.NewPort());
            Assert.AreEqual(3, allocator.NewPort());
            allocator.ReleasePort(3);
            Assert.AreEqual(3, allocator.NewPort());
            allocator.ReleasePort(1);
            Assert.AreEqual(1, allocator.NewPort());
        }
    }
}