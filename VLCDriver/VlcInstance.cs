using System;
using System.Diagnostics;
using NLog;

namespace VLCDriver
{
    public interface IVlcInstance
    {
        event VlcInstance.VlcEventHandler OnExited;
        Process Process { get; }
        DateTime StartTime { get; }
        void Kill();
    }

    public class VlcInstance : IVlcInstance
    {
        private readonly ILogger logger;

        public delegate void VlcEventHandler(object source, EventArgs e);
        public event VlcEventHandler OnExited;

        public VlcInstance(Process process, ILogger logger)
        {
            this.logger = logger;
            Process = process;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Process.Exited += Process_Exited;
            Process.OutputDataReceived += OnOutputDataReceived;
            Process.ErrorDataReceived += OnErrorDataReceived;
        }

        private void UnsubscribeToEvents()
        {
            Process.Exited -= Process_Exited;
            Process.OutputDataReceived -= OnOutputDataReceived;
            Process.ErrorDataReceived -= OnErrorDataReceived;
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            logger.Debug("Process id {0} finished.", Process.Id);
            UnsubscribeToEvents();
            if (OnExited != null)
            {
                OnExited(this, EventArgs.Empty);
            }
        }
        public Process Process { get; private set; }

        public void Kill()
        {
            if (Process != null)
            {
                Process.Kill();
            }
        }

        public DateTime StartTime 
        {
            get { return Process.StartTime; }
        }
    }
}
