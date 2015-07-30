using System;
using System.IO;

namespace VLCDriver
{
    public abstract class VlcJob
    {
        protected VlcJob(IPortAllocator allocator, IStatusParser statusParser, IVlcStatusSource statusSource, ITimeSouce timeSouce)
        {
            PortAllocator = allocator;
            StatusParser = statusParser;
            StatusSource = statusSource;
            TimeSouce = timeSouce;
        }

        public enum JobState { NotStarted = 0, Started = 1, Error = 2 , Finished = 3 }
        public JobState State { get; set; }
        public IVlcInstance Instance { get; set; }
        public FileInfo InputFile { get; set; }
        public FileInfo OutputFile { get; set; }
        public bool QuitAfer { get; set; }

        public IAudioConfiguration AudioConfiguration { get; set; }
        public IPortAllocator PortAllocator { get; private set; }
        private IStatusParser StatusParser { get; set; }
        private IVlcStatusSource StatusSource { get; set; }
        private ITimeSouce TimeSouce { get; set; }
        private int AllocatedPort { get; set; }

        //Todo, Generate New FileName based on input file

        public string GetVlcArguments()
        {
            if (InputFile == null)
                throw new InvalidOperationException("No Input File Specified for job");
            if (OutputFile == null)
                throw new InvalidOperationException("No Output File Specified for job");

            if (!InputFile.Exists)
            {
                throw new FileNotFoundException("Input file didn't exist", InputFile.FullName);
            }

            const string vlcQuitString = " vlc://quit";

            AllocatedPort = PortAllocator.NewPort();
            var interfaceArguments = string.Format("-I http --http-password {0} --http-port {1}", Properties.Settings.Default.VlcHttpPassword, AllocatedPort);

            var transcodeArgs = GetSpecificJobTypeArguments();
            var quitAfter = QuitAfer ? vlcQuitString : string.Empty;
            var arguments = string.Format("{0} \"{1}\" \":sout=#transcode{{{2}}}:std{{dst='{3}',access=file}}\"{4}", interfaceArguments, InputFile.FullName, transcodeArgs, OutputFile.FullName, quitAfter);
            return arguments;
        }

        protected abstract string GetSpecificJobTypeArguments();

        public void SetJobComplete()
        {
            State = JobState.Finished;
            PortAllocator.ReleasePort(AllocatedPort);
        }

        public void UpdateProgress()
        {
            if (State != JobState.Started)
            {
                return;
            }
            //Todo, Error handling

            var statusUrl = string.Format("http://localhost:{0}/requests/status.xml", AllocatedPort);
            StatusSource.Url = statusUrl;
            var xml = StatusSource.GetXml();

            StatusParser.Xml = xml;
            StatusParser.Parse();
            PercentComplete = StatusParser.Position;

            var startTimeUtc = Instance.StartTime.ToUniversalTime();
            var now = TimeSouce.getDateTime.ToUniversalTime();
            var elapsed = now - startTimeUtc;
            var percentPerSecond = PercentComplete / elapsed.TotalSeconds;
            if (Math.Abs(percentPerSecond) < 0.001)
            {
                EstimatedTimeToCompletion = new TimeSpan(0,0,0);
            }
            else
            {
                var secondsToCompletion = (1 - PercentComplete) / percentPerSecond;
                EstimatedTimeToCompletion = TimeSpan.FromSeconds(secondsToCompletion);
            }
        }

        /// <summary>
        /// A percent between 0 and 1
        /// </summary>
        public double PercentComplete { get; private set; }

        public TimeSpan EstimatedTimeToCompletion { get; private set; }
    }
}
