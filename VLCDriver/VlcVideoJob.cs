namespace VLCDriver
{
    public class VlcVideoJob : VlcJob
    {
        public VlcVideoJob(VideoConfiguration videoConfiguration, AudioConfiguration audioConfiguration)
        {
            VideoConfiguration = videoConfiguration;
            AudioConfiguration = audioConfiguration;
        }

        public VideoConfiguration VideoConfiguration { get; protected set; }

        protected override string GetSpecificJobTypeArguments()
        {
            return string.Format("{0},{1}",VideoConfiguration.Arguments(), AudioConfiguration.Arguments());
        }
    }
}
