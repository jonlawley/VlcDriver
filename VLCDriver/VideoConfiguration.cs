using System;

namespace VLCDriver
{
    public interface IVideoConfiguration
    {
        string GetPartArguments();
    }

    public class VideoConfiguration : IConfiguration, IVideoConfiguration
    {
        public enum VlcVideoFormat { h264, Mpeg2 };
        public enum VideoScale { None, Auto , half, quarter, one};

        public VlcVideoFormat Format { get; set; }
        public VideoScale Scale { get; set; }
        public int Mpg2Vb { get; set; }

        public int? XScale { get; set; }
        public int? YScale { get; set; }

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
                    throw new InvalidOperationException("Don't know how to support video type");
            }
            return string.Format("vcodec={0}{1}", vCodecName, GetScaleArguments());
        }

        private string GetScaleArguments()
        {
            if (!XScale.HasValue && !YScale.HasValue)
            {
                if (Scale == VideoScale.None)
                {
                    return string.Empty;
                }
                return string.Format(",scale={0}",ConvertScaleArguments());
            }
            var scaleArguments = string.Empty;
            scaleArguments += XScale.HasValue != YScale.HasValue ? ",scale=Auto" : ",";
            if (XScale.HasValue)
            {
                scaleArguments += string.Format(",width={0}", XScale.Value);
            }
            if (YScale.HasValue)
            {
                scaleArguments += string.Format(",height={0}", YScale.Value);
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
            throw new InvalidOperationException("Scale is not yet Supported");
        }
    }
}
