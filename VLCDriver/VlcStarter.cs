using System.Diagnostics;
using System.IO;
using NLog;

namespace VLCDriver
{
    public class VlcStarter : IVlcStarter
    {
        private readonly ILogger logger;

        public VlcStarter(ILogger logger)
        {
            this.logger = logger;
        }

        public IVlcInstance Start(string parameters, FileInfo vlcExePath)
        {

            if (!vlcExePath.Exists)
            {
                var vlcExeNotFound = string.Format("Vlc Exe Not found. Was looking in {0}", vlcExePath.FullName);
                logger.Error(vlcExeNotFound);
                throw new FileNotFoundException(vlcExeNotFound);
            }

            var info = new ProcessStartInfo(vlcExePath.FullName, parameters)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            var process = Process.Start(info);

            logger.Debug(string.Format("Jobs Arguments are\"{0}\" ProcessId: {1}", parameters, process.Id));
            process.EnableRaisingEvents = true;

            return new VlcInstance(process, logger);
        }
    }
}
