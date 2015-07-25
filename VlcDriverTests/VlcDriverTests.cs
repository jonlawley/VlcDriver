using System;
using Rhino.Mocks;
using VLCDriver;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace VlcDriverTests
{
    [TestFixture]
    public class VlcDriverTests
    {
        [TestFixtureSetUp]
        public void Init()
        {
            TestUtilities.CleanUpTestArea();
            TestUtilities.CloseAllVlcs();
        }

        [TearDown]
        public void GlobalTeardown()
        {
            var closedVlcs = TestUtilities.CloseAllVlcs();
            Assert.AreEqual(0, closedVlcs, "There should be no remaining instances running at the end of tests");
        }

        [Test]
        public void TestPathAssignment()
        {
            var driver = new VlcDriver(new VlcStarter())
            {
                VlcExePath = new FileInfo(@"c:\Fo.exe")
            };
            Assert.AreEqual(@"c:\Fo.exe", driver.VlcExePath.FullName);
        }

        [Test]
        public void TestVlcConversionWithArguments()
        {
            var driver = new VlcDriver(new VlcStarter());

            var jobArgument = string.Format("-I dummy \"{0}\\NeedinYou2SecWavMp3128.mp3\" \":sout=#transcode{{vcodec=none,acodec=s16l,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.wav',access=file}}\" vlc://quit", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.IsFalse(File.Exists(Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav")));
            var instance = driver.StartInstance(jobArgument);
            var exitedNotificationHappened = false;
            instance.OnExited += (sender, e) =>
            {
                exitedNotificationHappened = true;
            };

            instance.Process.WaitForExit();
            Assert.IsTrue(exitedNotificationHappened);
            var expectedFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav");
            Assert.IsTrue(File.Exists(expectedFile));
            var fileInfo = new FileInfo(expectedFile);
            Assert.That(fileInfo.Length, Is.EqualTo(368684).Within(1).Percent);
        }

        [Test]
        public void TestVlcStarts()
        {
            var driver = new VlcDriver(new VlcStarter());
            var instance = driver.StartInstance();
            Thread.Sleep(500);
            var vlcProcesses = Process.GetProcessesByName("vlc");
            Assert.AreEqual(1, vlcProcesses.Length);
            instance.Process.Kill();
            instance.Process.WaitForExit();
            vlcProcesses = Process.GetProcessesByName("vlc");
            Assert.AreEqual(0, vlcProcesses.Length);
        }

        [Test]
        public void TestVlcJobGetsAddedToCollection()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3
            };

            var job = new VlcAudioJob(audioConfiguration);
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var starter = MockRepository.GenerateMock<IVlcStarter>();
            var instance = MockRepository.GenerateMock<IVlcInstance>();
            starter.Expect(x => x.Start(Arg<string>.Is.Anything, Arg<FileInfo>.Is.Anything)).Return(instance);
            var driver = new VlcDriver(starter);
            Assert.AreEqual(0, driver.JobBag.Count);
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            driver.StartJob(job);
            Assert.AreEqual(1, driver.JobBag.Count);
            Assert.AreEqual(VlcJob.JobState.Started, job.State);
            instance.Raise(x => x.OnExited += null, instance, new EventArgs());
            Assert.AreEqual(1, driver.JobBag.Count);
        }

        [Test]
        public void TestVlcWav2Mp3JobActuallyGetsDone()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3
            };

            var job = new VlcAudioJob(audioConfiguration);
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var driver = new VlcDriver(new VlcStarter());
            Assert.IsFalse(job.OutputFile.Exists);
            driver.StartJob(job);
            Assert.AreEqual(1, driver.JobBag.Count);
            Assert.IsNotNull(job.Instance);
            Assert.IsNotNull(job.Instance.Process);
            Assert.AreEqual(VlcJob.JobState.Started, job.State);
            job.Instance.Process.WaitForExit();
            Assert.AreEqual(VlcJob.JobState.Finished, job.State);
            var newFileInfo = new FileInfo(job.OutputFile.FullName);
            Assert.IsTrue(newFileInfo.Exists);
            Assert.That(newFileInfo.Length, Is.EqualTo(48901).Within(1).Percent);
        }

        [Test]
        public void TestVlcMp32WavJobActuallyGetsDone()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");
            var audioConfiguration = new AudioConfiguration();
            audioConfiguration.Format = AudioConfiguration.ConversionFormats.Wav;

            var job = new VlcAudioJob(audioConfiguration);
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output2.wav");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var driver = new VlcDriver(new VlcStarter());
            Assert.IsFalse(job.OutputFile.Exists, "output file already exists, cannot run test");
            driver.StartJob(job);
            Assert.AreEqual(1, driver.JobBag.Count);
            Assert.IsNotNull(job.Instance);
            Assert.IsNotNull(job.Instance.Process);
            Assert.AreEqual(VlcJob.JobState.Started, job.State);
            job.Instance.Process.WaitForExit();
            Assert.AreEqual(VlcJob.JobState.Finished, job.State, "Job state was not set to finished afterwards");
            var newFileInfo = new FileInfo(job.OutputFile.FullName);
            Assert.IsTrue(newFileInfo.Exists);
            Assert.That(newFileInfo.Length, Is.EqualTo(368684).Within(1).Percent);
        }
    }
}
