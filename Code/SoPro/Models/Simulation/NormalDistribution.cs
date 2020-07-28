using Sopro.Interfaces;
using System;
using System.Collections.Generic;

namespace Sopro.Models.Simulation
{
    /// <summary>
    /// Erstellt eine zeitliche Normalverteilung für Buchungen über einen Tag hinweg.
    /// Wird genutzt um Buchungen für die Simulation zu generieren.
    /// </summary>
    public class NormalDistribution : IFunctionStrategy
    {
        /// <summary>
        /// Erstelle die Liste der normalverteilten Zeiten.
        /// </summary>
        /// <param name="start">Anfang des Zeitraums.</param>
        /// <param name="end">Ende des Zeitraums.</param>
        /// <param name="bookings">Die Anzahl der Buchungen, die in diesem Zeitraum normalverteilt werden sollen.</param>
        /// <returns>Eine Liste an DateTimes um die normalverteilten Buchungen zu erstellen.</returns>
        public List<DateTime> generateDateTimeValues(DateTime start, DateTime end, int bookings)
        {
            List<DateTime> DTList = new List<DateTime>();
            double difH = (end - start).TotalHours;
            Random rand = new Random();

            // Box-Muller algorithm   https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
            for (int i = 0; i < bookings; i++) 
            {
                DateTime dt = start;
                DTList.Add(dt.AddHours(boxmuller(difH)));
            }
            return DTList;
        }


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
