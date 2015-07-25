namespace VLCDriver
{
    public class VlcVideoJob : VlcJob
    {
        public VlcVideoJob(IVideoConfiguration videoConfiguration, IAudioConfiguration audioConfiguration)
        {
            VideoConfiguration = videoConfiguration;
            AudioConfiguration = audioConfiguration;
        }

        public IVideoConfiguration VideoConfiguration { get; protected set; }

        protected override string GetSpecificJobTypeArguments()
        {
            return string.Format("{0},{1}",VideoConfiguration.GetPartArguments(), AudioConfiguration.GetPartArguments());
        }
    }
}
