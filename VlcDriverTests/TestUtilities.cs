using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VLCDriver;

namespace VlcDriverTests
{
    public static class TestUtilities
    {
        const string TestFilesDirName = "TestFiles";
        const string TestConversionOutput = "Output";
        public static FileInfo GetTestFile(string testFileNameWithExtension)
        {
            var testFileDir = GetTestDir();
            var expectedTestFile = Path.Combine(testFileDir, testFileNameWithExtension);
            var info = new FileInfo(expectedTestFile);
            if(info.Exists)
            {
                return info;
            }
            throw new FileNotFoundException("Test File not found",testFileNameWithExtension);
        }

        public static string GetTestOutputDir()
        {
            var assemblyFolder = GetProjectDir();
            return Path.Combine(assemblyFolder, TestConversionOutput);
        }

        private static string GetProjectDir()
        {
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            if (projectDirectory != null)
            {
                var projDir = projectDirectory.FullName;
                return projDir;
            }
            throw new InvalidOperationException();
        }

        public static string GetTestDir()
        {
            var assemblyFolder = GetProjectDir();
            return Path.Combine(assemblyFolder, TestFilesDirName);
        }

        public static void CleanUpTestArea()
        {
            var outputdir = GetTestOutputDir();
            if (!Directory.Exists(outputdir))
            {
                Directory.CreateDirectory(outputdir);
            }
            var dirInfo = new DirectoryInfo(outputdir);
            foreach (var file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }

        internal static int CloseAllVlcs()
        {
            var vlcProcesses = Process.GetProcessesByName("vlc");

            var numberClosed = vlcProcesses.Count();

            foreach(var process in vlcProcesses)
            {
                process.Kill();
            }

            return numberClosed;
        }

        public static void SetVlcExeLocationOnNonStandardWindowsEnvironments(VlcDriver driver)
        {
            var osver = Environment.OSVersion;
            switch (osver.Platform)
            {
                case PlatformID.Win32NT:
                    break;
                case PlatformID.Unix:
                    driver.VlcExePath = new FileInfo("/usr/bin/vlc");
                    break;
                default:
                    throw new InvalidOperationException("I've not thought this through yet");
            }
        }
    }
}
