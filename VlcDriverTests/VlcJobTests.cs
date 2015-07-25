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
            var job = new VlcAudioJob(audioConfig);
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,!!FOO!!}}:std{{dst='{1}\\output.mp3',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
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

            var job = new VlcVideoJob(videoConfig,audioConfig);
            var inputfile = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{!!VFOO!!,!!AFOO!!}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            var actualArguments = job.GetVlcArguments();
            Assert.AreEqual(expectedArguments, actualArguments);
        }

        [Test]
        public void EnsureQuitAfterIsPassedToArguments()
        {
            var audioConfig = MockRepository.GenerateMock<IAudioConfiguration>();
            audioConfig.Expect(x => x.GetPartArguments()).Return("!!FOO!!");
            var job = new VlcAudioJob(audioConfig)
            {
                QuitAfer = true
            };
            var inputfile = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            job.InputFile = inputfile;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,!!FOO!!}}:std{{dst='{1}\\output.mp3',access=file}}\" vlc://quit", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            var actualArguments = job.GetVlcArguments();
            Assert.AreEqual(expectedArguments, actualArguments);
        }

        [Test]
        public void EnsureAudioJobIsNullWhenNoInputFileInGiven()
        {
            var job = new VlcAudioJob(new AudioConfiguration());
            Assert.IsNull(job.GetVlcArguments());
        }

        [Test]
        public void EnsureVideoJobIsNullWhenNoInputFileInGiven()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());
            Assert.IsNull(job.GetVlcArguments());
        }
    }
}
