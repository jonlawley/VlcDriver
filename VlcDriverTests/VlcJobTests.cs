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

            var job = new VlcAudioJob(audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>());
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I http --http-password goose --http-port 42 \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,!!FOO!!}}:std{{dst='{1}\\output.mp3',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
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

            var job = new VlcVideoJob(videoConfig, audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>());
            var inputfile = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I http --http-password goose --http-port 42 \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{!!VFOO!!,!!AFOO!!}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
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

            var job = new VlcAudioJob(audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>())
            {
                QuitAfer = true
            };
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I http --http-password goose --http-port 42 \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,!!FOO!!}}:std{{dst='{1}\\output.mp3',access=file}}\" vlc://quit", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            var actualArguments = job.GetVlcArguments();
            Assert.AreEqual(expectedArguments, actualArguments);
        }

        //[Test]
        //public void EnsureAudioJobIsNullWhenNoInputFileInGiven()
        //{
        //    var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
        //    portAllocator.Expect(x => x.NewPort()).Return(42);

        //    var job = new VlcAudioJob(new AudioConfiguration(), portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>());
        //    Assert.IsNull(job.GetVlcArguments());
        //}

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnsureExceptionWhenNoInputFileInGiven()
        {
            var portAllocator = MockRepository.GenerateMock<IPortAllocator>();
            portAllocator.Expect(x => x.NewPort()).Return(42);

            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration(), portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>())
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

            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration(), portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>())
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

            var job = new VlcAudioJob(audioConfig, portAllocator, MockRepository.GenerateMock<IStatusParser>(), MockRepository.GenerateMock<IVlcStatusSource>());
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

            var testFile = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");

            var job = new VlcAudioJob(audioConfig, portAllocator, statusParser, statusSource)
            {
                State = VlcJob.JobState.Started,
                InputFile = testFile,
                OutputFile = new FileInfo(@"C:\Foo2.txt")
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

            var job = new VlcAudioJob(audioConfig, portAllocator, statusParser, statusSource)
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

            var job = new VlcAudioJob(audioConfig, portAllocator, statusParser, statusSource)
            {
                State = VlcJob.JobState.Started,
                InputFile = new FileInfo(@"C:\Foo.txt"),
                OutputFile = new FileInfo(@"C:\Foo2.txt")
            };

            job.GetVlcArguments();
        }
    }
}
