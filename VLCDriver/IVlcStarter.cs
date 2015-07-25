using System.IO;

namespace VLCDriver
{
    public interface IVlcStarter
    {
        IVlcInstance Start(string parameters, FileInfo vlcExePath);
    }
}
