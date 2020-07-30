using Sopro.CustomValidationAttributes;
using Sopro.Interfaces;
using Sopro.Models.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Infrastructure
{
    /// <summary>
    /// Die Klasse für den Standort.
    /// Ein Standort kann mehrere Zonen enthalten.
    /// </summary>
    public class Location : ILocation
    {
        public string id { get; set; }
        [ListMinLength(0)]
        public List<Zone> zones { get; set; }
        [Required]
        public string name { get; set; }
        [Range(0, 1)]
        public double emergency { get; set; }
        public Schedule schedule { get; set; }
        public Distributor distributor { get; set; }
        public DateTime normalizedDistributionTime { get; set; }

        /// <summary>
        /// Konstruktor des Standorts. Jeder Standord hat eine Schedule und einen
        /// Verteiler, sowie eine eindeutige ID.
        /// </summary>
        public Location()
        {
            schedule = new Schedule();
            distributor = new Distributor(schedule, this);
            id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Fügt dem Standort eine Zone hinzu.
        /// </summary>
        /// <param name="zone">Die hinzuzufügende Zone.</param>
        /// <returns>Wahrheitswert entsprechend ob das Hinzufügen erfolgreich war.</returns>
        public bool addZone(Zone zone)
        {
            if (zones.Contains(zone))
                return false;

            zones.Add(zone);
            return zones.Contains(zone);
        }

        /// <summary>
        /// Entfernt eine Zone aus dem Standort.
        /// </summary>
        /// <param name="zone">Die zu entfernende Zone.</param>
        /// <returns>Wahrheitswert entsprechend ob das Entfernen erfolgreich war.</returns>
        public bool deleteZone(Zone zone)
        {
            if (!zones.Contains(zone))
                return false;

            zones.Remove(zone);
            return !zones.Contains(zone);
        }

        public Location deepCopy()
        {
            Location copy = new Location();
            copy.name = name;
            copy.normalizedDistributionTime = normalizedDistributionTime;

            foreach(Zone z in zones)
            {
                copy.zones.Add(z.deepCopy());
            }

            return copy;

        }
    }
}
