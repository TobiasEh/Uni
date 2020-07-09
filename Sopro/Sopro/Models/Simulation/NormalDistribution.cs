using Sopro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accord.Statistics.Distributions.Univariate;

namespace Sopro.Models.Simulation
{
    public class NormalDistribution : IFunctionStrategy
    {
        public List<DateTime> generateDateTimeValues(DateTime start, DateTime end, int bookings)
        {
            List<DateTime> DTList = new List<DateTime>();
            int difH = (end - start).Hours;
            Random rand = new Random();

            // Box-Muller algorithm   https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
            for (int i = 0; i< bookings; i++) 
            {
                DateTime dt = new DateTime();
                Double u1 = 1.0 - rand.NextDouble();
                Double u2 = 1.0 - rand.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                // since 3*std includes 99.73% of results dif of Hours from start to stop (difH) /2 equals Median and max dev from middle /3 equals max dev
                //https://en.wikipedia.org/wiki/Normal_distribution 
                // and difH / 2 equals middle
                double randNormal = difH/2 + difH/6 * randStdNormal;
                DTList.Add(dt.AddHours(randNormal)); // start + randNormal
            }
            return DTList;
        }
    }
}
