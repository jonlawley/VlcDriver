using System.Diagnostics;
using System.IO;

namespace VLCDriver
{
    public class VlcStarter : IVlcStarter
    {
        public IVlcInstance Start(string parameters, FileInfo vlcExePath)
        {
            if (!vlcExePath.Exists)
            {
                throw new FileNotFoundException("Vlc Exe Not found");
            }

            var info = new ProcessStartInfo(vlcExePath.FullName, parameters)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            var process = Process.Start(info);
            process.EnableRaisingEvents = true;
            return new VlcInstance(process);
        }
    }
}
