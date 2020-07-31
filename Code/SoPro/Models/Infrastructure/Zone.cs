using Sopro.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Infrastructure
{
    /// <summary>
    /// Die Klasse der Ladezone.
    /// Ladezonen sind in Standorten enthalten. Eine Ladezonen kann mehrere
    /// Ladestationen beinhalten.
    /// </summary>
    public class Zone
    {
        [ListMinLength(1)]
        public List<Station> stations { get; set; } = new List<Station>();
        public char site { get; set; }
        [Range(0,int.MaxValue)]
        public int maxPower { get; set; }

        /// <summary>
        /// Füge der Zone eine Station hinzu.
        /// </summary>
        /// <param name="station">Die hinzuzufügende Station.</param>
        /// <returns>Wahrheitswert entsprechend ob das Hinzufügen erfolgreich war oder nicht.</returns>
        public bool addStation(Station station)
        {
            if (stations.Contains(station))
                return false;

            stations.Add(station);
            return stations.Contains(station);
        }

        /// <summary>
        /// Entferne eine Station aus der Zone.
        /// </summary>
        /// <param name="station">Die zu entfernende Station.</param>
        /// <returns>Wahrheitswert entsprechend ob das Entfernen erfolgreich war oder nicht.</returns>
        public bool deleteStation(Station station)
        {
            if (!stations.Contains(station))
                return false;

            stations.Remove(station);
            return !stations.Contains(station);
        }

        /// <summary>
        /// Vergleiche zwei Zonen miteinander.
        /// </summary>
        /// <param name="zone">Die zum Vergleich verwendete Zone.</param>
        /// <returns>Wahrheitswert, ob Gleichheit besteht oder nicht.</returns>
        public bool compareTo(Zone zone)
        {
            return zone.site == this.site;
        }

        /// <summary>
        /// Erstellt eine TiefeKopie einer Zone.
        /// </summary>
        /// <returns>Eine tiefe Kopie dieser Zone.</returns>
        public Zone deepCopy()
        {
            Zone copy = new Zone();
            copy.site = site;
            copy.maxPower = maxPower;

            foreach(Station s in stations)
            {
                copy.stations.Add(s.deepCopy());
            }

            return copy;
        }
    }
}
