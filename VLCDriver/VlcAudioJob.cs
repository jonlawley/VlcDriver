using System;

namespace VLCDriver
{
    public class VlcAudioJob : VlcJob
    {
        internal VlcAudioJob(IAudioConfiguration audioConfig, IPortAllocator allocator, IStatusParser statusParser, IVlcStatusSource statusSouce, ITimeSouce timeSouce)
            : base(allocator, statusParser, statusSouce, timeSouce)
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
