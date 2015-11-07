using System;
using NLog;

namespace VLCDriver
{
    public class VlcAudioJob : VlcJob
    {
        public VlcAudioJob(IAudioConfiguration audioConfig, IPortAllocator allocator, IStatusParser statusParser, IVlcStatusSource statusSouce, ITimeSouce timeSouce, ILogger logger)
            : base(allocator, statusParser, statusSouce, timeSouce, logger)
        {
            if(audioConfig == null)
            {
                var argumentNullException = new ArgumentNullException("audioConfig");
                logger.Error(argumentNullException);
                throw argumentNullException;
            }
            AudioConfiguration = audioConfig;
        }

        protected override string GetSpecificJobTypeArguments()
        {
            return string.Format("vcodec=none,{0}", AudioConfiguration.GetPartArguments());
        }
    }
}
