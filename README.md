# VlcDriver
Create and manage VLC media conversion using .Net

VlC Driver provides a simple way to call into the media conversion from a simple .Net Library.

## Code Example

Here's a simple conversion of a video file to mg2:

            var job = VlcDriver.CreateVideoJob();
            job.InputFile = input; // A FileInfo Object
            job.OutputFile = output; // A FileInfo Object
            job.VideoConfiguration.Format = VideoConfiguration.VlcVideoFormat.Mpeg2;
            job.AudioConfiguration.Format = AudioConfiguration.ConversionFormats.Mpg;

            var driver = VlcDriver.CreateVlcDriver();
            driver.StartJob(job);
			
## Motivation

I've appreciated the simplicity of using VLC to convert between common media formats and I've written various projects to hook into it by constructing arguments.

I wanted to create a library which I can use it these various projects and thought i'd like to share it with the world.

## Installation

Solution was written in and compiles and build in Visual Studio 2013

## Tests

The VLC driver has a full set of unit tests. Tests run in Visual Studio Test Runner.

## Contributors

Jon Lawley, @jonlawley, jonlawley.com