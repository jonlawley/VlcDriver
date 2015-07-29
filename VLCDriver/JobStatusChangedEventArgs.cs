using System;

namespace VLCDriver
{
    public class JobStatusChangedEventArgs : EventArgs
    {
        public VlcJob Job { get; set; }
    }
}