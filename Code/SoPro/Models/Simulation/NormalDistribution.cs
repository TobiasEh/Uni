using Sopro.Interfaces;
using System;
using System.Collections.Generic;

namespace Sopro.Models.Simulation
{
    public class NormalDistribution : IFunctionStrategy
    {
        public List<DateTime> generateDateTimeValues(DateTime start, DateTime end, int bookings)
        {
            List<DateTime> DTList = new List<DateTime>();
            double differenceInHours = (end - start).TotalHours;
            for (int i = 0; i < bookings; i++) 
            {
                DateTime dt = start;
                DTList.Add(dt.AddHours(boxmuller(differenceInHours)));
            }
            return DTList;
        }

        // Generates values using Box-Muller algorithm. https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
        private double boxmuller(double interval)
        {
            Random rand = new Random();
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal1 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randStdNormal2 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
            double randNormal1 = interval / 2 + interval / 6 * randStdNormal1;
            double randNormal2 = interval / 2 + interval / 6 * randStdNormal2;
            if (randNormal1 <= interval && randNormal1 > 0)
            {
                return randNormal1;
            }
            else
            {
                if (randNormal2 <= interval && randNormal2 > 0)
                {
                    return randNormal2;
                }
            }
            return boxmuller(interval);
        }
    }
}
