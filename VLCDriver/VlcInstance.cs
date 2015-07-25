using System;
using System.Diagnostics;

namespace VLCDriver
{
    public interface IVlcInstance
    {
        event VlcInstance.VlcEventHandler OnExited;
        Process Process { get; set; }
    }

    public class VlcInstance : IVlcInstance
    {
        public delegate void VlcEventHandler(object source, EventArgs e);
        public event VlcEventHandler OnExited;

        public VlcInstance(Process process)
        {
            Process = process;
            Process.Exited += Process_Exited;
            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.ErrorDataReceived += Process_ErrorDataReceived;
        }

        void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
        }

        void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Process.Exited -= Process_Exited;
            if (OnExited != null)
            {
                OnExited(this, new EventArgs());
            }
        }
        public Process Process { get; set; }
    }
}
