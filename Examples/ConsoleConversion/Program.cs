using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using VLCDriver;

namespace ConsoleConversion
{
    class Program
    {
        private static VlcVideoJob Job;
        static void Main()
        {
            #region This region is only for use in this console example as ending the console will leave VLC running
            handler = ConsoleEventCallback;
            var osver = Environment.OSVersion;
            switch (osver.Platform)
            {
                case PlatformID.Win32NT:
                    SetConsoleCtrlHandler(handler, true);
                    break;
            }
            
            #endregion

            var input = new FileInfo(@"c:\Temp\inputVideo.avi");
            var output = new FileInfo(@"c:\Temp\outputVideo.mpg");

            if (!input.Exists)
            {
                throw new FileNotFoundException("Example app needs a file to convert", input.FullName);
            }

            var driver = new VlcDriver();
            //driver.VlcExePath = new FileInfo("/usr/bin/vlc"); - Only on Non Windows environments
            Job = driver.CreateVideoJob();
            Job.InputFile = input;
            Job.OutputFile = output;
            Job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.Mpeg2;
            Job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;

            driver.StartJob(Job);

            while (Job.State != VlcJob.JobState.Finished)
            {
                Job.UpdateProgress();
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("{0}% Complete. Remaining {1}", string.Format("{0:0.0#}", Job.PercentComplete * 100), Job.EstimatedTimeToCompletion.ToString(@"h\h\:m\m\:s\s", System.Globalization.CultureInfo.InvariantCulture));
                Thread.Sleep(1000);
            }

            #region This region is only for use in this console example as ending the console will leave VLC running
            if (SignalThread != null)
            {
                SignalThread.Abort();
            }
            #endregion
        }

        #region This region is only for use in this console example as ending the console will leave VLC running
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Console.WriteLine("Console window closing");
                Job.Instance.Kill();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
        private static Thread SignalThread;
        // PInvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        /// <summary>
        /// Windows specific code for dealing with when the console is closed
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        #endregion
    }
}
