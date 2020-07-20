using System;
using System.Collections.Generic;

namespace Sopro.Interfaces
{
    public interface IFunctionStrategy
    {
        List<DateTime> generateDateTimeValues(DateTime start, DateTime end, int bookings);
    }
}
