using NUnit.Framework;
using VLCDriver;

namespace VlcDriverTests
{
    [TestFixture]
    public class VlcAudioConfigurationTests
    {
        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionUsingDefaults()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3
            };

            var arguments = audioConfiguration.GetPartArguments();
            const string expectedArguments = "acodec=mp3,ab=192,channels=2,samplerate=44100";
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleMp3ToWavConversionChangeChannelsTo1()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Wav,
                Channels = 1
            };

            var arguments = audioConfiguration.GetPartArguments();

            const string expectedArguments = "acodec=s16l,ab=128,channels=1,samplerate=44100";
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionChangeBitrateTo64()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3,
                AudioBitrateInkbps = 64
            };

            var arguments = audioConfiguration.GetPartArguments();
            const string expectedArguments = "acodec=mp3,ab=64,channels=2,samplerate=44100";
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionChangeChannelsTo1()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3,
                Channels = 1
            };

            var arguments = audioConfiguration.GetPartArguments();
            const string expectedArguments = "acodec=mp3,ab=192,channels=1,samplerate=44100";
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestAudioJobSimpleWavToMp3ConversionChangeSampleRateTo44()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Mp3,
                SampleRateHertz = 44
            };

            var arguments = audioConfiguration.GetPartArguments();
            const string expectedArguments = "acodec=mp3,ab=192,channels=2,samplerate=44";
            Assert.AreEqual(expectedArguments, arguments);
        }


        [Test]
        public void TestAudioJobSimpleMp3ToWavConversionUsingDefaults()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Wav
            };

            var arguments = audioConfiguration.GetPartArguments();
            const string expectedArguments = "acodec=s16l,ab=128,channels=2,samplerate=44100";
            Assert.AreEqual(expectedArguments, arguments);
        }


        [Test]
        public void TestAudioJobSimpleMp3ToWavConversionChangeChannelsSampleRate()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.Wav,
                SampleRateHertz = 50
            };

            var arguments = audioConfiguration.GetPartArguments();
            const string expectedArguments = "acodec=s16l,ab=128,channels=2,samplerate=50";
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestConversionWithoutAudio()
        {
            var audioConfiguration = new AudioConfiguration
            {
                Format = AudioConfiguration.ConversionFormats.None
            };

            var arguments = audioConfiguration.GetPartArguments();
            const string expectedArguments = "acodec=none";
            Assert.AreEqual(expectedArguments, arguments);
        }
    }
}