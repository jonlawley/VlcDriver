using System.Collections.Concurrent;
using System.IO;

namespace VLCDriver
{
    public interface IVlcDriver
    {
        IVlcLocator Locator { get; }
        FileInfo VlcExePath { get; set; }
        ConcurrentBag<VlcJob> JobBag { get; }
        IVlcInstance StartInstance(string parameters = "");
        VlcVideoJob CreateVideoJob();
        VlcAudioJob CreateAudioJob();
        void StartJob(VlcJob job);
        event VlcDriver.VlcDriverEventHandler OnJobStateChange;
    }
}