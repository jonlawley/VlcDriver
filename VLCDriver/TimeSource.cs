using System;

namespace VLCDriver
{
    public interface ITimeSouce
    {
        DateTime getDateTime { get; }
    }

    public class TimeSouce : ITimeSouce
    {
        public DateTime getDateTime
        {
            get
            {
                return DateTime.Now;
            }
        } 
    }
}