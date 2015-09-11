using System;
using System.IO;

namespace VLCDriver
{
    public class VlcLocator : IVlcLocator
    {
        private string DefaultVlcPathBasedOnProcessor()
        {
            var programFilesLocation = ProgramFilesx86();

            if (programFilesLocation == null)
            {
                return null;
            }

            var expectedVlcLocation = Path.Combine(programFilesLocation, "VideoLAN\\VLC\\vlc.exe");

            return File.Exists(expectedVlcLocation) ? expectedVlcLocation : null;
        }

        public string Location
        {
            get { return location ?? (location = DefaultVlcPathBasedOnProcessor()); }
            set { location = value; }
        }
        private string location;

        private string ProgramFilesx86()
        {
            if (Is64BitOs())
            {
                return GetEnviromentVariable("ProgramFiles(x86)");
            }

            return GetEnviromentVariable("ProgramFiles");
        }

        private bool Is64BitOs()
        {
            return 8 == IntPtr.Size
                            || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")));
        }

        private string GetEnviromentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }
    }
}
