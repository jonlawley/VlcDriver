using System;

namespace VLCDriver
{
    public interface IAudioConfiguration
    {
        string GetPartArguments();
    }

    public class AudioConfiguration : IConfiguration, IAudioConfiguration
    {
        public AudioConfiguration()
        {
            Format = ConversionFormats.Wav;
            AudioBitrateInkbps = 192;
            SampleRateHertz = 44100;
            Channels = 2;
        }

        public enum ConversionFormats { Mp3, Wav , Mpg, None}

        public ConversionFormats Format { get; set; }

        public int AudioBitrateInkbps { get; set; }

        public int Channels { get; set; }

        public int SampleRateHertz { get; set; }

        public string GetPartArguments()
        {
            switch (Format)
            {
                case ConversionFormats.Mp3:
                    return string.Format("acodec=mp3,ab={0},channels={1},samplerate={2}", AudioBitrateInkbps, Channels, SampleRateHertz);
                case ConversionFormats.Mpg:
                    return string.Format("acodec=mpga,ab={0},channels={1},samplerate={2}", AudioBitrateInkbps, Channels, SampleRateHertz);
                case ConversionFormats.Wav:
                    return string.Format("acodec=s16l,ab=128,channels={0},samplerate={1}", Channels, SampleRateHertz);
                case ConversionFormats.None:
                    return "acodec=none";
            }
            throw new InvalidOperationException("No format Specified");
        }
    }
}
