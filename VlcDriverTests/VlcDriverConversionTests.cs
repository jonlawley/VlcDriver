using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using VLCDriver;

namespace VlcDriverTests
{
    /// <summary>
    /// These are unit tests which actually call VLC an test it's output
    /// </summary>
    [TestFixture]
    public class VlcDriverConversionTests
    {
        const int AllowedOutputFileComparePercentage = 3;

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
        public void TestVlcConversionWithArguments()
        {
            var driver = new VlcDriver(new VlcStarter());
            TestUtilities.SetVlcExeLocationOnNonStandardWindowsEnvironments(driver);

            var jobArgument = string.Format("-I dummy \"{0}{2}NeedinYou2SecWavMp3128.mp3\" \":sout=#transcode{{vcodec=none,acodec=s16l,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}{2}output.wav',access=file}}\" vlc://quit", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir(), Path.DirectorySeparatorChar);
            Assert.IsFalse(File.Exists(Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav")));
            var instance = driver.StartInstance(jobArgument);
            var exitedNotificationHappened = false;
            instance.OnExited += (sender, e) =>
            {
                exitedNotificationHappened = true;
            };

            instance.Process.WaitForExit();
            SleepToAllowEventHandler();
            Assert.IsTrue(exitedNotificationHappened);
            var expectedFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav");
            Assert.IsTrue(File.Exists(expectedFile));
            var fileInfo = new FileInfo(expectedFile);
            
            Assert.That(fileInfo.Length, Is.EqualTo(368684).Within(AllowedOutputFileComparePercentage).Percent);
        }

        [Test]
        public void TestVlcStarts()
        {
            var driver = new VlcDriver(new VlcStarter());
            TestUtilities.SetVlcExeLocationOnNonStandardWindowsEnvironments(driver);
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
        public void TestVlcWav2Mp3JobActuallyGetsDone()
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

            var driver = new VlcDriver(new VlcStarter());
            TestUtilities.SetVlcExeLocationOnNonStandardWindowsEnvironments(driver);
            Assert.IsFalse(job.OutputFile.Exists);
            driver.StartJob(job);
            Assert.AreEqual(1, driver.JobBag.Count);
            Assert.IsNotNull(job.Instance);
            Assert.IsNotNull(job.Instance.Process);
            Assert.AreEqual(VlcJob.JobState.Started, job.State);
            job.Instance.Process.WaitForExit();
            SleepToAllowEventHandler();
            Assert.AreEqual(VlcJob.JobState.Finished, job.State);
            var newFileInfo = new FileInfo(job.OutputFile.FullName);
            Assert.IsTrue(newFileInfo.Exists);
            Assert.That(newFileInfo.Length, Is.EqualTo(48901).Within(AllowedOutputFileComparePercentage).Percent);
        }

        [Test]
        public void TestVlcMp32WavJobActuallyGetsDone()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Wav
            };

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcAudioJob(audioConfiguration, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce());
            Assert.AreEqual(VlcJob.JobState.NotStarted, job.State);
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output2.wav");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var driver = new VlcDriver(new VlcStarter());
            TestUtilities.SetVlcExeLocationOnNonStandardWindowsEnvironments(driver);
            Assert.IsFalse(job.OutputFile.Exists, "output file already exists, cannot run test");
            driver.StartJob(job);
            Assert.AreEqual(1, driver.JobBag.Count);
            Assert.IsNotNull(job.Instance);
            Assert.IsNotNull(job.Instance.Process);
            Assert.AreEqual(VlcJob.JobState.Started, job.State);
            job.Instance.Process.WaitForExit();
            SleepToAllowEventHandler();
            Assert.AreEqual(VlcJob.JobState.Finished, job.State, "Job state was not set to finished afterwards");
            var newFileInfo = new FileInfo(job.OutputFile.FullName);
            Assert.IsTrue(newFileInfo.Exists);
            Assert.That(newFileInfo.Length, Is.EqualTo(368684).Within(AllowedOutputFileComparePercentage).Percent);
        }

        private void SleepToAllowEventHandler()
        {
            Thread.Sleep(1000); //Process wait can occur before process exited
        }
    }
}