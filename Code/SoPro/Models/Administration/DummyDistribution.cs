using Sopro.Interfaces;
using System;
using System.Collections.Generic;

namespace Sopro.Models.Administration
{
    /// <summary>
    /// Verteilalgorithmus zu Testzwecken. 
    /// Verteilt einfach nur wahllos Buchungen in die Schedule.
    /// </summary>
    public class DummyDistribution : IDistributionStrategy
    {
        public bool distribute(List<Booking> bookings, Schedule schedule, int puffer)
        {
            foreach (Booking item in bookings)
            {
                schedule.addBooking(item);
            }
            return true;
        }
    }
}
