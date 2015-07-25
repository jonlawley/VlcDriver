using NUnit.Framework;
using VLCDriver;

namespace VlcDriverTests
{
    [TestFixture]
    public class VlcVideoConfigurationTests
    {
        [Test]
        public void TestVideoConversion1UsingH264AndMpgAudio()
        {
            var videoConfiguration = new VideoConfiguration
            {
                Format = VideoConfiguration.VlcVideoFormat.h264,
                Scale = VideoConfiguration.VideoScale.Auto
            };

            var arguments = videoConfiguration.GetPartArguments();
            const string expectedArguments = "vcodec=h264,scale=Auto";
            Assert.AreEqual(expectedArguments, arguments);
        }


        [Test]
        public void TestVideoConversion2UsingMpg2AndMpgAudio()
        {
            var videoConfiguration = new VideoConfiguration
            {
                Format = VideoConfiguration.VlcVideoFormat.Mpeg2
            };

            var arguments = videoConfiguration.GetPartArguments();
            const string expectedArguments = "vcodec=mp2v,vb=800";
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion3UsingG264AndScaleVideoByCustomWidth()
        {
            var videoConfiguration = new VideoConfiguration
            {
                XScale = 320
            };

            var arguments = videoConfiguration.GetPartArguments();
            const string expectedArguments = "vcodec=h264,scale=Auto,width=320";
            Assert.AreEqual(expectedArguments, arguments);
        }


        [Test]
        public void TestVideoConversion4UsingG264AndScaleVideoByCustomHeight()
        {
            var videoConfiguration = new VideoConfiguration
            {
                YScale = 100
            };

            var arguments = videoConfiguration.GetPartArguments();
            const string expectedArguments = "vcodec=h264,scale=Auto,height=100";
            Assert.AreEqual(expectedArguments, arguments);
        }

        [Test]
        public void TestVideoConversion5Using264AndScaleByHalf()
        {
            var videoConfiguration = new VideoConfiguration
            {
                Scale = VideoConfiguration.VideoScale.half
            };

            var arguments = videoConfiguration.GetPartArguments();
            const string expectedArguments = "vcodec=h264,scale=0.5";
            Assert.AreEqual(expectedArguments, arguments);
        }


        [Test]
        public void TestVideoConversion6UsingMpg2AndSetCustomBitrate()
        {
            var videoConfiguration = new VideoConfiguration
            {
                Format = VideoConfiguration.VlcVideoFormat.Mpeg2,
                Mpg2Vb = 432
            };

            var arguments = videoConfiguration.GetPartArguments();
            const string expectedArguments = "vcodec=mp2v,vb=432";

            Assert.AreEqual(expectedArguments, arguments);
        }
    }
}