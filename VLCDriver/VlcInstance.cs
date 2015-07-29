using System;
using System.Diagnostics;

namespace VLCDriver
{
    public interface IVlcInstance
    {
        event VlcInstance.VlcEventHandler OnExited;
        Process Process { get; }
    }

    public class VlcInstance : IVlcInstance
    {
        public delegate void VlcEventHandler(object source, EventArgs e);
        public event VlcEventHandler OnExited;

        public VlcInstance(Process process)
        {
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
            UnsubscribeToEvents();
            if (OnExited != null)
            {
                OnExited(this, EventArgs.Empty);
            }
        }
        public Process Process { get; private set; }
    }
}
