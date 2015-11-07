using System;
using NLog;

namespace VLCDriver
{
    public interface IVideoConfiguration
    {
        string GetPartArguments();
        VideoConfiguration.VlcVideoFormat Format { get; set; }
        VideoConfiguration.VideoScale Scale { get; set; }
        int Mpg2Vb { get; set; }
        int? XFrameSize { get; set; }
        int? YFrameSize { get; set; }
    }

    public class VideoConfiguration : IConfiguration, IVideoConfiguration
    {
        public enum VlcVideoFormat { h264, Mpeg2 };
        public enum VideoScale { None, Auto , half, quarter, one};

        public VlcVideoFormat Format { get; set; }
        public VideoScale Scale { get; set; }
        public int Mpg2Vb { get; set; }

        public int? XFrameSize { get; set; }
        public int? YFrameSize { get; set; }

        public VideoConfiguration()
        {
            Mpg2Vb = 800;
        }

        public string GetPartArguments()
        {
            string vCodecName;
            switch(Format)
            {
                case VlcVideoFormat.h264:
                    vCodecName = "h264";
                    break;
                case VlcVideoFormat.Mpeg2:
                    vCodecName = string.Format("mp2v,vb={0}", Mpg2Vb);
                    break;
                default:
                {
                    var invalidOperationException = new InvalidOperationException("Don't know how to support video type");
                    if (logger != null)
                    {
                        logger.Error(invalidOperationException);
                    }
                    throw invalidOperationException;
                }
            }
            return string.Format("vcodec={0}{1}", vCodecName, GetScaleArguments());
        }

        private string GetScaleArguments()
        {
            if (!XFrameSize.HasValue && !YFrameSize.HasValue)
            {
                if (Scale == VideoScale.None)
                {
                    return string.Empty;
                }
                return string.Format(",scale={0}",ConvertScaleArguments());
            }
            var scaleArguments = string.Empty;
            scaleArguments += XFrameSize.HasValue != YFrameSize.HasValue ? ",scale=Auto" : ",";
            if (XFrameSize.HasValue)
            {
                scaleArguments += string.Format(",width={0}", XFrameSize.Value);
            }
            if (YFrameSize.HasValue)
            {
                scaleArguments += string.Format(",height={0}", YFrameSize.Value);
            }
            return scaleArguments;
        }

        private string ConvertScaleArguments()
        {
            switch (Scale)
            {
                case VideoScale.Auto:
                    return "Auto";
                case VideoScale.half:
                    return "0.5";
                case VideoScale.quarter:
                    return "0.25";
            }
            var invalidOperationException = new InvalidOperationException("This Scale is not yet Supported");
            if (logger != null)
            {
                logger.Error(invalidOperationException);
            }
            throw invalidOperationException;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
