﻿using System;
using System.Collections.Concurrent;
using NLog;

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
        private readonly ILogger logger;

        /// <summary>
        /// Concurrency Dictionary Value indicates if it's still being Used
        /// </summary>

        public PortAllocator(ILogger logger)
        {
            this.logger = logger;
        }

        private ConcurrentDictionary<int,bool> UsedPorts
        {
            get { return usedPorts ?? (usedPorts = new ConcurrentDictionary<int, bool>()); }
        }
        private ConcurrentDictionary<int,bool> usedPorts;

        public int StartPort { get; set; }

        public int NewPort()
        {
            var port = StartPort;
            while (true)
            {
                if (!UsedPorts.ContainsKey(port) || !UsedPorts[port])
                {
                    UsedPorts[port] = true;
                    return port;
                }
                port++;
            }
        }

        public void ReleasePort(int port)
        {
            if (UsedPorts.ContainsKey(port))
            {
                UsedPorts[port] = false;
            }
            else
            {
                var invalidOperationException = new InvalidOperationException(string.Format("Port {0} is not in use or already released",port));
                logger.Error(invalidOperationException);
                throw invalidOperationException;
            }
        }
    }
}