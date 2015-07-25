using System;

namespace VLCDriver
{
    public class VlcAudioJob : VlcJob
    {
        public VlcAudioJob(AudioConfiguration audioConfig)
        {
            if(audioConfig == null)
            {
                throw new ArgumentNullException("audioConfig");
            }
            AudioConfiguration = audioConfig;
        }

        protected override string GetSpecificJobTypeArguments()
        {
            return string.Format("vcodec=none,{0}", AudioConfiguration.Arguments());
        }
    }
}
