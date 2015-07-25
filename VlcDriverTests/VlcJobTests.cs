using NUnit.Framework;
using System.IO;
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
        public void TestAudioJobIsNullWhenNoInputFileInGiven()
        {
            var job = new VlcAudioJob(new AudioConfiguration());
            Assert.IsNull(job.GetVlcArguments());
        }

        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionUsingDefaults()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mp3;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,acodec=mp3,ab=192,channels=2,samplerate=44100}}:std{{dst='{1}\\output.mp3',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionChangeBitrateTo64()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mp3;
            job.AudioConfiguration.AudioBitrateInkbps = 64;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,acodec=mp3,ab=64,channels=2,samplerate=44100}}:std{{dst='{1}\\output.mp3',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionChangeChannelsTo1()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mp3;
            job.AudioConfiguration.Channels = 1;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,acodec=mp3,ab=192,channels=1,samplerate=44100}}:std{{dst='{1}\\output.mp3',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionChangeSampleRateTo44()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWav.wav");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp3");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mp3;
            job.AudioConfiguration.SampleRateHertz = 44;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWav.wav\" \":sout=#transcode{{vcodec=none,acodec=mp3,ab=192,channels=2,samplerate=44}}:std{{dst='{1}\\output.mp3',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleMp3ToWavConversionUsingDefaults()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Wav;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWavMp3128.mp3\" \":sout=#transcode{{vcodec=none,acodec=s16l,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.wav',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestQuitAfterIsPassedToArguments()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Wav;
            job.QuitAfer = true;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWavMp3128.mp3\" \":sout=#transcode{{vcodec=none,acodec=s16l,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.wav',access=file}}\" vlc://quit", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleMp3ToWavConversionChangeChannelsTo1()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Wav;
            job.AudioConfiguration.Channels = 1;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWavMp3128.mp3\" \":sout=#transcode{{vcodec=none,acodec=s16l,ab=128,channels=1,samplerate=44100}}:std{{dst='{1}\\output.wav',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleMp3ToWavConversionChangeChannelsSampleRate()
        {
            var file = TestUtilities.GetTestFile("NeedinYou2SecWavMp3128.mp3");
            var job = new VlcAudioJob(new AudioConfiguration())
            {
                InputFile = file
            };
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.wav");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Wav;
            job.AudioConfiguration.SampleRateHertz = 50;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\NeedinYou2SecWavMp3128.mp3\" \":sout=#transcode{{vcodec=none,acodec=s16l,ab=128,channels=2,samplerate=50}}:std{{dst='{1}\\output.wav',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion1UsingH264AndMpgAudio()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());

            var file = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);

            job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.h264;
            job.VideoConfiguration.Scale = VideoConfiguration.VideoScale.Auto;

            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;
            job.AudioConfiguration.AudioBitrateInkbps = 128;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{vcodec=h264,scale=Auto,acodec=mpga,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion2UsingMpg2AndMpgAudio()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());

            var file = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.ts");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.Mpeg2;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;
            job.AudioConfiguration.AudioBitrateInkbps = 128;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{vcodec=mp2v,vb=800,acodec=mpga,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.ts',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion3UsingG264AndScaleVideoByCustomWidth()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());

            var file = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = file;
            job.VideoConfiguration.XScale = 320;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;
            job.AudioConfiguration.AudioBitrateInkbps = 128;

            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{vcodec=h264,scale=Auto,width=320,acodec=mpga,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion4UsingG264AndScaleVideoByCustomHeight()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());

            var file = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = file;
            job.VideoConfiguration.YScale = 100;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;
            job.AudioConfiguration.AudioBitrateInkbps = 128;

            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{vcodec=h264,scale=Auto,height=100,acodec=mpga,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion5UsingG264AndScaleByHalf()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());

            var file = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.VideoConfiguration.Scale = VideoConfiguration.VideoScale.half;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;
            job.AudioConfiguration.AudioBitrateInkbps = 128;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{vcodec=h264,scale=0.5,acodec=mpga,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion6UsingMpg2AndSetCustomBitrate()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());

            var file = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);
            job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.Mpeg2;
            job.VideoConfiguration.Mpg2Vb = 432;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;
            job.AudioConfiguration.AudioBitrateInkbps = 128;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{vcodec=mp2v,vb=432,acodec=mpga,ab=128,channels=2,samplerate=44100}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());

            Assert.AreEqual(expectedArguments, arguments);
        } 

        [Test]
        public void TestConversionWithoutAudio()
        {
            var job = new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration());

            var file = TestUtilities.GetTestFile("SampleVideo_720x480_1mbH264.mp4");
            job.InputFile = file;
            var expectedOutputFile = Path.Combine(TestUtilities.GetTestOutputDir(), "output.mp4");
            job.OutputFile = new FileInfo(expectedOutputFile);

            job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.h264;
            job.VideoConfiguration.Scale = VideoConfiguration.VideoScale.Auto;

            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.None;

            var arguments = job.GetVlcArguments();
            var expectedArguments = string.Format("-I dummy \"{0}\\SampleVideo_720x480_1mbH264.mp4\" \":sout=#transcode{{vcodec=h264,scale=Auto,acodec=none}}:std{{dst='{1}\\output.mp4',access=file}}\"", TestUtilities.GetTestDir(), TestUtilities.GetTestOutputDir());
            Assert.AreEqual(expectedArguments, arguments);
        }
    }
}
