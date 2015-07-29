namespace VLCDriver
{
    public class VlcVideoJob : VlcJob
    {
        internal VlcVideoJob(IVideoConfiguration videoConfiguration, IAudioConfiguration audioConfiguration, IPortAllocator allocator, IStatusParser statusParser, IVlcStatusSource statusSouce)
            : base(allocator, statusParser, statusSouce)
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
