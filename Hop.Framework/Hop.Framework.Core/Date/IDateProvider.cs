using System;

namespace Hop.Framework.Core.Date
{
    public interface IDateProvider
    {
        void SetTimeZone(int minutes);
        DateTime Now();
        DateTime UTC();
        DateTime ToLocal(DateTime date);
        DateTime ToUTC(DateTime date);
    }
}
