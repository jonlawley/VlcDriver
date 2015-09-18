using System;
using NUnit.Framework;
using System.IO;
using Rhino.Mocks;
using VLCDriver;

namespace VlcDriverTests
{
    [TestFixture]
    public class VlcJobTests
    {
        [TestFixtureSetUp]
        public void Init()
        {
            TestUtilities.CleanUpTestArea();
        }

        [Test]
        public void EnsureVlcAudioArgumentsArePassedCorrectly()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();
            audioConfig.Expect(x => x.GetPartArguments()).Return("!!FOO!!");

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcAudioJob(audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce());
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I http --http-password goose --http-port 42 \"{0}{2}NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,!!FOO!!}}:std{{dst='{1}{2}output.mp3',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir(), Path.DirectorySeparatorChar);
            var actualArguments = job.GetVlcArguments();
            Assert.AreEqual(expectedArguments, actualArguments);
        }

        [Test]
        public void EnsureVlcVideoArgumentsArePassedCorrectly()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();
            audioConfig.Expect(x => x.GetPartArguments()).Return("!!AFOO!!");

            var videoConfig = MockRepository.GenerateMock<IVideoConfiguration>();
            videoConfig.Expect(x => x.GetPartArguments()).Return("!!VFOO!!");

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcVideoJob(videoConfig, audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce());
            var inputfile = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I http --http-password goose --http-port 42 \"{0}{2}SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{!!VFOO!!,!!AFOO!!}}:std{{dst='{1}{2}output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir(), Path.DirectorySeparatorChar);
            var actualArguments = job.GetVlcArguments();
            Assert.AreEqual(expectedArguments, actualArguments);
        }

        [Test]
        public void EnsureQuitAfterIsPassedToArguments()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();
            audioConfig.Expect(x => x.GetPartArguments()).Return("!!FOO!!");

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcAudioJob(audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce())
            {
                QuitAfer = true
            };
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I http --http-password goose --http-port 42 \"{0}{2}NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,!!FOO!!}}:std{{dst='{1}{2}output.mp3',access=file}}\" vlc://quit", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir(), Path.DirectorySeparatorChar);
            var actualArguments = job.GetVlcArguments();
            Assert.AreEqual(expectedArguments, actualArguments);
        }


        [Test]
        public void EnsurePasswordConfigChangeIsUsed()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();
            audioConfig.Expect(x => x.GetPartArguments()).Return("!!FOO!!");

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcAudioJob(audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce())
            {
                QuitAfer = true
            };
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            VLCDriver.Properties.Settings.Default.VlcHttpPassword = "duck";

            var expectedArguments = string.Format("-I http --http-password duck --http-port 42 \"{0}{2}NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,!!FOO!!}}:std{{dst='{1}{2}output.mp3',access=file}}\" vlc://quit", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir(), Path.DirectorySeparatorChar);
            var actualArguments = job.GetVlcArguments();
            VLCDriver.Properties.Settings.Default.VlcHttpPassword = "goose";
            Assert.AreEqual(expectedArguments, actualArguments);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "No Input File Specified for job")]
        public void EnsureExceptionWhenNoInputFileInGiven()
        {
            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration(), portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce())
            {
                OutputFile = new FileInfo("out.txt")
            };
            job.GetVlcArguments();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnsureExceptionWhenNoOutputFileInGiven()
        {
            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration(), portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce())
            {
                InputFile = new FileInfo("in.txt")
            };
            job.GetVlcArguments();
        }

        [Test]
        public void EnsurePortAllocatorIsCalledAndReleased()
        {
            //TODO. Ensure Port is always delallocated even in error
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();
            audioConfig.Expect(x => x.GetPartArguments()).Return("!!FOO!!");

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(49);

            var job = new VlcAudioJob(audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>(), new TimeSouce());
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            portAllocator.AssertWasNotCalled(x => x.NewPort());
            job.GetVlcArguments();
            portAllocator.AssertWasCalled(x => x.NewPort());
            portAllocator.AssertWasNotCalled(x => x.ReleasePort(Arg<int>.Is.Anything));
            job.SetJobComplete();
            portAllocator.AssertWasCalled(x => x.ReleasePort(49));
        }

        [Test]
        public void EnsureTheLiveStatusIsReadCorrectly()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(89);

            var statusSource = MockRepository.GenerateStub<IVlcStatusSource>();
            statusSource.Expect(x => x.GetXml()).Return("Foo");

            var statusParser = MockRepository.GenerateMock<IStatusParser>();
            statusParser.Expect(x => x.Position).Return(67);

            var timeSource = MockRepository.GenerateMock<ITimeSouce>();
            timeSource.Expect(x => x.getDateTime).Return(new DateTime(1988, 03, 26, 18, 0, 0));

            var testFile = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");

            var job = new VlcAudioJob(audioConfig, portAllocator, statusParser, statusSource, timeSource)
            {
                State = VlcJob.JobState.Started,
                InputFile = testFile,
                OutputFile = new FileInfo(@"C:\Foo2.txt"),
                Instance = MockRepository.GenerateMock<IVlcInstance>()
            };

            job.GetVlcArguments();
            job.UpdateProgress();

            Assert.AreEqual("http://localhost:89/requests/status.xml", statusSource.Url, "Correct Url was not generated");
            Assert.AreEqual(67, job.PercentComplete);
            statusSource.AssertWasCalled(x =>x.GetXml());
            statusParser.AssertWasCalled(x =>x.Parse());
        }

        [Test]
        [TestCase(VlcJob.JobState.Error)]
        [TestCase(VlcJob.JobState.NotStarted)]
        [TestCase(VlcJob.JobState.Finished)]
        public void EnsureTheLiveStatusIsNotUpdatedWhenStateIsNotStarted(VlcJob.JobState state)
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(89);

            var statusSource = MockRepository.GenerateStub<IVlcStatusSource>();
            statusSource.Expect(x => x.GetXml()).Return("Foo");

            var statusParser = MockRepository.GenerateMock<IStatusParser>();
            statusParser.Expect(x => x.Position).Return(67);

            var job = new VlcAudioJob(audioConfig, portAllocator, statusParser, statusSource, new TimeSouce())
            {
                State = state,
            };

            job.UpdateProgress();
            statusSource.AssertWasNotCalled(x => x.GetXml());
            statusParser.AssertWasNotCalled(x => x.Parse());
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void EnsureExceptionHappensWhenTryingToConvertIfInputFileIsMissing()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(89);

            var statusSource = MockRepository.GenerateStub<IVlcStatusSource>();
            statusSource.Expect(x => x.GetXml()).Return("Foo");

            var statusParser = MockRepository.GenerateMock<IStatusParser>();
            statusParser.Expect(x => x.Position).Return(67);

            var job = new VlcAudioJob(audioConfig, portAllocator, statusParser, statusSource, new TimeSouce())
            {
                State = VlcJob.JobState.Started,
                InputFile = new FileInfo(@"C:\Foo.txt"),
                OutputFile = new FileInfo(@"C:\Foo2.txt")
            };

            job.GetVlcArguments();
        }

        [Test]
        public void EnsureExpectedCompletionTimeIsEstimatedCorectly()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();

            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();

            var statusSource = MockRepository.GenerateMock<IVlcStatusSource>();

            var statusParser = MockRepository.GenerateMock<IStatusParser>();
            statusParser.Expect(x => x.Position).Return(0).Repeat.Once();

            var timeSouce = MockRepository.GenerateMock<ITimeSouce>();
            var testTime = new DateTime(1988, 3, 26, 18, 0, 0).ToUniversalTime();
            timeSouce.Expect(x => x.getDateTime).Return(testTime.AddSeconds(1)).Repeat.Once();

            var instance = MockRepository.GenerateMock<IVlcInstance>();
            instance.Expect(x => x.StartTime).Return(testTime);

            var testFile = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");

            var job = new VlcAudioJob(audioConfig, portAllocator, statusParser, statusSource, timeSouce)
            {
                State = VlcJob.JobState.Started,
                InputFile = testFile,
                OutputFile = new FileInfo(@"C:\Foo2.txt"),
                Instance = instance
            };

            job.GetVlcArguments();
            //To begin with ensure the estimated completion time is 0
            Assert.AreEqual(new TimeSpan(0,0,0), job.EstimatedTimeToCompletion);

            //Ensure the updating the progress calculates 0 as no progress has been made
            job.UpdateProgress();
            Assert.AreEqual(new TimeSpan(0, 0, 0), job.EstimatedTimeToCompletion);

            // If 30 seconds have elapsed and the job is 25% complete, the estimated finish time should be 1m 30s
            statusParser.Expect(x => x.Position).Return(0.25).Repeat.Once();
            timeSouce.Expect(x => x.getDateTime).Return(testTime.AddSeconds(30)).Repeat.Once();
            job.UpdateProgress();
            Assert.AreEqual(new TimeSpan(0, 1, 30), job.EstimatedTimeToCompletion);

            //If 1 minute has elapsed and the job is 50% complete, the estimated time should be 1 minute
            statusParser.Expect(x => x.Position).Return(0.5).Repeat.Once();
            timeSouce.Expect(x => x.getDateTime).Return(testTime.AddSeconds(60)).Repeat.Once();
            job.UpdateProgress();
            Assert.AreEqual(new TimeSpan(0, 1, 0), job.EstimatedTimeToCompletion);

            //If 2 minutes have elapsed and the job is 100% complete, the estimated time left is 0
            statusParser.Expect(x => x.Position).Return(1).Repeat.Once();
            timeSouce.Expect(x => x.getDateTime).Return(testTime.AddSeconds(120)).Repeat.Once();
            job.UpdateProgress();
            Assert.AreEqual(new TimeSpan(0, 0, 0), job.EstimatedTimeToCompletion);
        }

    }
}
