using NLog;

namespace VLCDriver
{
    public class VlcVideoJob : VlcJob
    {
        public VlcVideoJob(IVideoConfiguration videoConfiguration, IAudioConfiguration audioConfiguration, IPortAllocator allocator, IStatusParser statusParser, IVlcStatusSource statusSouce, ITimeSouce timeSouce, ILogger logger)
            : base(allocator, statusParser, statusSouce, timeSouce, logger)
        {
            VideoConfiguration = videoConfiguration;
            AudioConfiguration = audioConfiguration;
        }

        public IVideoConfiguration VideoConfiguration { get; set; }

        protected override string GetSpecificJobTypeArguments()
        {
            return string.Format("{0},{1}",VideoConfiguration.GetPartArguments(), AudioConfiguration.GetPartArguments());
        }
    }
}
