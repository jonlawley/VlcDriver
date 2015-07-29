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
            SetConsoleCtrlHandler(handler, true);
            #endregion

            var input = new FileInfo(@"c:\Temp\inputVideo.avi");
            var output = new FileInfo(@"c:\Temp\outputVideo.mpg");

            Job = VlcDriver.CreateVideoJob();
            Job.InputFile = input;
            Job.OutputFile = output;
            Job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.Mpeg2;
            Job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;

            var driver = VlcDriver.CreateVlcDriver();
            driver.StartJob(Job);

            while (Job.State != VlcJob.JobState.Finished)
            {
                Job.UpdateProgress();
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("{0}% Complete", Job.PercentComplete);
                Thread.Sleep(2000);
            }
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
        // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        #endregion
    }
}
