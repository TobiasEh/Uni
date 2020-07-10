using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces
{
    public interface IFunctionStrategy
    {
        List<DateTime> generateDateTimeValues(DateTime start, DateTime end, int bookings);
    }
}
