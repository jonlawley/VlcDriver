using System;

namespace VLCDriver
{
    public class VlcAudioJob : VlcJob
    {
        public VlcAudioJob(IAudioConfiguration audioConfig, IPortAllocator allocator, IStatusParser statusParser, IVlcStatusSource statusSouce)
            : base(allocator, statusParser, statusSouce)
        {
            if(audioConfig == null)
            {
                throw new ArgumentNullException("audioConfig");
            }
            AudioConfiguration = audioConfig;
        }

        protected override string GetSpecificJobTypeArguments()
        {
            return string.Format("vcodec=none,{0}", AudioConfiguration.GetPartArguments());
        }
    }
}
