using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLeh.Util
{
    /// <summary>
    /// This class will return the first parameter the first time it's ran, and after flipTime time has passed. otherwise, it will return the second parameter.
    /// </summary>
    public class TimeSensitiveLatch
    {
        public TimeSensitiveLatch(bool theDefault, bool triggeredValue, TimeSpan flipTime = default(TimeSpan), ICurrentDateTimeProvider provider = null)
        {
            Default = theDefault;
            Triggered = triggeredValue;
            if (flipTime == default(TimeSpan))
                flipTime = TimeSpan.FromHours(6);
            FlipTime = flipTime;
            if (provider == null)
                provider = new CurrentDateTimeProvider();
            DateTimeProvider = provider;
        }

        public void SetTimeProvider(ICurrentDateTimeProvider provider)
        {
            DateTimeProvider = provider;
        }

        bool Default;
        bool Triggered;
        ICurrentDateTimeProvider DateTimeProvider { get; set; }
        TimeSpan FlipTime;
        DateTime LastFlip { get; set; }
        DateTime NextFlip { get { return LastFlip.Add(FlipTime); } }
        bool ever = false;

        public bool Value
        {
            get
            {
                var now = DateTimeProvider.Now;
                if (!ever)
                {
                    ever = true;
                    LastFlip = now;
                    return Default;
                }
                if (NextFlip < now)
                {
                    LastFlip = now.Add(FlipTime);
                    return Default;
                }
                return Triggered;
            }
        }
    }
}
