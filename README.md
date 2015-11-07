# VlcDriver
Create and manage VLC media conversion using .Net

VlC Driver provides a simple way to call into the media conversion from a simple .Net Library. This library uses is based around the strategy of calling VLC as a process rather than using PInvoke to call into lvc libraries (like libVLCnet http://libvlcnet.sourceforge.net/)

## Code Example

Here's a simple conversion of a video file to mpg2:

            var job = VlcDriver.CreateVideoJob();
            job.InputFile = input; // A FileInfo Object
            job.OutputFile = output; // A FileInfo Object. The file won't exist yet
            job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.Mpeg2;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;

            var driver = VlcDriver.CreateVlcDriver();
            driver.StartJob(job);
			
## Motivation

I've appreciated the simplicity of using VLC to convert between common media formats and I've written various projects to hook into it by constructing arguments.

I wanted to create a library which I can use it these various projects and thought i'd like to share it with the world.

## Installation

Solution was written in and compiles and builds in Visual Studio 2015 .Net 4.5. Nuget will handle installation of libraries.

## Logging

Logging has been implemented using NLog https://github.com/NLog .  See the console app example in this solution for a basic example.

## Mono
I've taken quite some time to ensure this project will work on Mono. This has been tested on Ubuntu 14.04, Mono JIT compiler version 4.0.3 and MonoDevelop 5.9.5.
You will need to use the "Add-in Manager" to add the mono nuget repo which is (http://mrward.github.com/monodevelop-nuget-addin-repository/4.0/main.mrep). 

On Mono, one slight difference is the requirement to specify the location of VLC install manually.  Typically this is:

			var driver = new VlcDriver();
			driver.VlcExePath = new FileInfo("/usr/bin/vlc");
			
Also remember Unix file paths and different too too!

            var job = VlcDriver.CreateVideoJob();
            job.InputFile = new FileInfo(@"/home/input.avi");
            job.OutputFile = new FileInfo(@"/home/Output.avi");

## Tests

The VLC driver has a comprehensive set of unit tests. Tests have been written using NUnit. To run them, install the "NUnit Test Adapter" extension in Visual Studio.

## Contributors

Jon Lawley, @jonlawley, jonlawley.com