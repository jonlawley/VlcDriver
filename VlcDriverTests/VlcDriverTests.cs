using System;
using Rhino.Mocks;
using VLCDriver;
using NUnit.Framework;
using System.IO;

namespace VlcDriverTests
{
    [TestFixture]
    public class VlcDriverTests
    {
        [Test]
        public void TestPathAssignment()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Foo.exe");
            var locator = MockRepository.GenerateMock<IVlcLocator>();
            var driver = new VlcDriver(new VlcStarter(), null, locator)
            {
                VlcExePath = new FileInfo(path)
            };
            Assert.AreEqual(path, driver.VlcExePath.FullName);
        }

        [Test]
        public void TestVlcJobGetsAddedToCollection()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3
            };

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcAudioJob(audioConfiguration, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce());
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var starter = MockRepository.GenerateMock<IVlcStarter>();
            var instance = MockRepository.GenerateMock<IVlcInstance>();
            starter.Expect(x => x.Start(Arg<string>.Is.Anything, Arg<FileInfo>.Is.Anything)).Return(instance);
            var driver = new VlcDriver(starter);
            TestUtilities.SetVlcExeLocationOnNonStandardWindowsEnvironments(driver);
            Assert.AreEqual(0, driver.JobBag.Count);
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            driver.StartJob(job);
            Assert.AreEqual(1, driver.JobBag.Count);
            Assert.AreEqual(VlcJob.JobState.Started, job.State);
            instance.Raise(x => x.OnExited += null, instance, new EventArgs());
            Assert.AreEqual(1, driver.JobBag.Count);
        }

        [Test]
        public void TestWeGetJobStateChangedEventWhenConversionEnds()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3
            };

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcAudioJob(audioConfiguration, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce());
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var starter = MockRepository.GenerateMock<IVlcStarter>();
            var instance = MockRepository.GenerateMock<IVlcInstance>();
            starter.Expect(x => x.Start(Arg<string>.Is.Anything, Arg<FileInfo>.Is.Anything)).Return(instance);
            var driver = new VlcDriver(starter);
            TestUtilities.SetVlcExeLocationOnNonStandardWindowsEnvironments(driver);
            Assert.AreEqual(0, driver.JobBag.Count);
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            driver.StartJob(job);
            Assert.AreEqual(1, driver.JobBag.Count);
            Assert.AreEqual(VlcJob.JobState.Started, job.State);
            var eventHandlerWasCalled = false;
            driver.OnJobStateChange += (source, args) =>
            {
                eventHandlerWasCalled = true;
                Assert.AreEqual(job, args.Job);
            };
            instance.Raise(x => x.OnExited += null, instance, new EventArgs());
            Assert.IsTrue(eventHandlerWasCalled);
        }

        [Test]
        public void TestAudioJobCreation()
        {
            var driver = new VlcDriver();
            var job = driver.CreateAudioJob();
            Assert.IsNotNull(job);
            Assert.AreEqual(8081, job.PortAllocator.StartPort);
        }

        [Test]
        public void TestVideoJobCreation()
        {
            var driver = new VlcDriver();
            var job = driver.CreateVideoJob();
            Assert.IsNotNull(job);
            Assert.AreEqual(8081, job.PortAllocator.StartPort);
        }

        [Test]
        public void EnsureTheSameInstanceOfPortAllocatorIsAlwaysUsedWhenOnceIsNotSpecifiedInTheConstructor()
        {
            var driver = new VlcDriver();
            var job1 = driver.CreateAudioJob();
            var job2 = driver.CreateAudioJob();
            Assert.AreNotEqual(job1, job2);
            Assert.AreEqual(job1.PortAllocator, job2.PortAllocator);
        }

        [Test]
        public void EnsureTheSameInstanceOfPortAllocatorIsAlwaysUsedWhenOnceIsSpecifiedInTheConstructor()
        {
            var portAllocator = new PortAllocator();
            var driver = new VlcDriver(null, portAllocator);
            var job = driver.CreateAudioJob();
            Assert.AreEqual(portAllocator, job.PortAllocator);
        }

        [Test]
        public void EnsureVlcLocationPathIsUsedByVlcDriver()
        {
            var locator = MockRepository.GenerateStub<IVlcLocator>();
            var location = Path.Combine(TestUtilities.GetTestDir(), "myfakevlc.exe");
            locator.Location = location;
            var starter = MockRepository.GenerateMock<IVlcStarter>();
            var driver = new VlcDriver(starter, null, locator);
            Assert.AreEqual(location, driver.VlcExePath.FullName);

            starter.Expect(x => x.Start(Arg<string>.Is.Anything, Arg<FileInfo>.Is.Anything)).Repeat.Once();
            driver.StartInstance();

            var args = starter.GetArgumentsForCallsMadeOn(x => x.Start(null, null), x => x.IgnoreArguments());
            var filePathArgument = args[0][1] as FileInfo;
            Assert.AreEqual(location, filePathArgument.FullName);

            starter.VerifyAllExpectations();
        }
    }
}
