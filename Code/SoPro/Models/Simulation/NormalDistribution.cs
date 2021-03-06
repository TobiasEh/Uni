﻿using Sopro.Interfaces;
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
        /// Generiert die Zeitpunkte, innerhalb einer Stoßzeit, zu denen eine Buchung erstellt werden sollte.
        /// Die stoßzeit wird dafür in gleich weit entfernte Zeitpunkte geteilt.
        /// Jeder Zeitpunkt wird mit einer Wahrscheinlichkeit abhängig von der Normalverteilung der rückgabe Liste hinzugefügt.
        /// </summary>
        /// <param name="start">Startzeitpunkt der Stoßzeit</param>
        /// <param name="end">Endzeitpunkt der Stoßzeit</param>
        /// <param name="maxProbability">Die maximal erreichbare Wahrscheinlichkeit.</param>
        /// <param name="minProbability">Die Minimal erreichbare Wahrscheinlichkeit.</param>
        /// <param name="length">Der Abstand zwischen den Zeitpunkten</param>
        /// <param name="bookings">Maximal Zahl der noch erzeugbaren Buchungen.</param>
        /// <param name="spread">Spread der Normalverteilung</param>
        /// <returns>Liste an Zeitpunkten zu denen eine Buchung erzeugt werden soll.</returns>
        public List<DateTime> generateDateTimeValues(DateTime start, DateTime end,double maxProbability, double minProbability, TimeSpan length, int bookings, double spread)
        {
            Random r = new Random();

            DateTime currently = start;
            List<DateTime> generatedTimeSteps = new List<DateTime>();

            // Zeitspanne der Buchung.
            TimeSpan timeSpan = end - start;

            // Mittelpunkt. Gibt den Punkt an an dem die höchste Wahrscheinlichkeit errreicht werden soll.
            int peak = (int) (timeSpan.TotalMinutes) / 2;

            
            double diff = maxProbability - minProbability;

            // Berechnet den multiplier mit dem man sicherstellen kann, dass das Minimum nie unterschritten und das Maximum nie überschritten wird.
            double multiplier = (diff) / ((1 / spread * Math.Sqrt(Math.PI * 2)) * Math.Log((-0.5) * Math.Pow((peak-peak)/spread,2)));

            // Läuft durch alle Möglichen Zeitschritte.
            while(currently <= end)
            {
                // Berechnet die Wahrscheinlichkeit mit der zu diesem Zeitpunkt eine Buchung erstellt werden soll.
                double prob = ((1 / spread * Math.Sqrt(Math.PI * 2)) * Math.Log((-0.5) * Math.Pow(((end-currently).TotalMinutes - peak) / spread, 2))) * multiplier + minProbability;

                if(prob <= r.NextDouble())
                {
                    // Zeitpunkt wird der Liste hinzugefügt.
                    generatedTimeSteps.Add(currently);
                    bookings--;
                    if(bookings == 0)
                    {
                        // Keine Buchungen mehr übrig.
                        return generatedTimeSteps;
                    }
                }
                // Weiterzählen um length.
                currently = currently.Add(length);
            }
            return generatedTimeSteps;
        }


        /*
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
        } */
        
    }
}
