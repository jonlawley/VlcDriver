using System;
using System.IO;
using System.Threading;
using VLCDriver;

namespace ConsoleConversion
{
    class Program
    {
        static void Main()
        {
            var input = new FileInfo(@"c:\Temp\inputVideo.avi");
            var output = new FileInfo(@"c:\Temp\outputVideo.mpg");

            var job = VlcDriver.CreateVideoJob();
            job.InputFile = input;
            job.OutputFile = output;
            job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.Mpeg2;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;

            var driver = VlcDriver.CreateVlcDriver();
            driver.StartJob(job);

            while (job.State != VlcJob.JobState.Finished)
            {
                job.UpdateProgress();
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("{0}% Complete", job.PercentComplete);
                Thread.Sleep(2000);
            }
        }
    }
}
