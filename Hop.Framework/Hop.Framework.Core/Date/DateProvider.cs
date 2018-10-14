using System;

namespace Hop.Framework.Core.Date
{
    public class DateProvider : IDateProvider
    {
        private int _minutes = 0;

        public void SetTimeZone(int minutes)
        {
            _minutes = minutes;
        }
        public DateTime Now()
        {
            return DateTime.UtcNow.AddMinutes(_minutes);
        }
        public DateTime ToLocal(DateTime date)
        {
            return date.AddMinutes(_minutes);
        }

        public DateTime ToUTC(DateTime date)
        {
            return date.ToUniversalTime();
        }

        public DateTime UTC()
        {
            return DateTime.UtcNow;
        }
    }
}
