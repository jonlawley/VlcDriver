using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace VLCDriver
{
    public class VlcDriver
    {
        internal VlcDriver(IVlcStarter starter)
        {
            this.starter = starter;
        }

        private readonly IVlcStarter starter;

        public VlcLocator Locator
        {
            get { return locator ?? (locator = new VlcLocator()); }
            protected set { locator = value; }
        }
        private VlcLocator locator;

        public FileInfo VlcExePath
        {
            get { return vlcExePath ?? (vlcExePath = new FileInfo(Locator.Location)); }
            set
            {
                vlcExePath = value;
            }
        }
        private FileInfo vlcExePath;

        public IVlcInstance StartInstance(string parameters = "")
        {
            return starter.Start(parameters, VlcExePath);
        }

        public static VlcDriver CreateVlcDriver()
        {
           return new VlcDriver(new VlcStarter());
        }

        public static VlcVideoJob CreateVideoJob()
        {
            return new VlcVideoJob(new VideoConfiguration(), new AudioConfiguration(), new PortAllocator { StartPort = Properties.Settings.Default.StartPort },
                new StatusParser(), new HttpVlcStatusSource());
        }

        public static VlcAudioJob CreateAudioJob()
        {
            return new VlcAudioJob(new AudioConfiguration(), new PortAllocator{StartPort = Properties.Settings.Default.StartPort}, new StatusParser(), new HttpVlcStatusSource());
        }

        public void StartJob(VlcJob job)
        {
            job.QuitAfer = true; //fairly important if we're tracking it
            var vlcArguments = job.GetVlcArguments();

            job.State = VlcJob.JobState.Started;
            var instance = starter.Start(vlcArguments, VlcExePath);
            job.Instance = instance;
            instance.OnExited += OnVlcInstanceExited;
            JobBag.Add(job);
        }

        private void OnVlcInstanceExited(object source, EventArgs e)
        {
            var instance = source as IVlcInstance;
            if (instance == null)
            {
                throw new InvalidOperationException("Source must be a VLC instance");
            }
            instance.OnExited -= OnVlcInstanceExited;

            var associatedJob = JobBag.First(x => x.Instance == instance);
            associatedJob.SetJobComplete();
            if (OnJobStateChange != null)
            {
                OnJobStateChange(this, new JobStatusChangedEventArgs { Job = associatedJob });
            }
        }

        public ConcurrentBag<VlcJob> JobBag
        {
            get { return bag ?? (bag = new ConcurrentBag<VlcJob>()); }
        }
        private ConcurrentBag<VlcJob> bag;

        public delegate void VlcDriverEventHandler(object source,JobStatusChangedEventArgs e);
        public event VlcDriverEventHandler OnJobStateChange;
    }
}
