namespace VLCDriver
{
    public class VlcVideoJob : VlcJob
    {
        public VlcVideoJob(IVideoConfiguration videoConfiguration, IAudioConfiguration audioConfiguration, IPortAllocator allocator, IStatusParser statusParser, IVlcStatusSource statusSouce, ITimeSouce timeSouce)
            : base(allocator, statusParser, statusSouce, timeSouce)
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
