using System.Collections.Concurrent;

namespace VLCDriver
{
    public interface IPortAllocator
    {
        int StartPort { get; set; }
        int NewPort();
        void ReleasePort(int port);
    }

    /// <summary>
    /// Consider the implications of more than one of these being around at once
    /// </summary>
    public class PortAllocator : IPortAllocator
    {
        private ConcurrentDictionary<int,int> UsedPorts
        {
            get { return usedPorts ?? (usedPorts = new ConcurrentDictionary<int, int>()); }
        }
        private ConcurrentDictionary<int,int> usedPorts;

        public int StartPort { get; set; }

        public int NewPort()
        {
            var port = StartPort;
            while (true)
            {
                if (!UsedPorts.ContainsKey(port))
                {
                    UsedPorts[port] = port;
                    return port;
                }
                port++;
            }
        }

        public void ReleasePort(int port)
        {
            int outvalue;
            UsedPorts.TryRemove(port,out outvalue);
        }
    }
}