using Sopro.CustomValidationAttributes;
using Sopro.Interfaces;
using Sopro.Interfaces.ControllerSimulation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sopro.Models.Simulation
{
    /// <summary>
    /// Die Klasse die ein Szenario beschreibt. Ein Szenario ist eine Template
    /// für eine zu simulierende Verteilung einer Buchungsabfolge.
    /// </summary>
    public class Scenario : IScenario
    {
        public string id { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int duration { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int bookingCountPerDay { get; set; }
        [Required]
        [ListMinLength(1)]
        public List<Vehicle> vehicles { get; set; }
        [Required]
        public List<Rushhour> rushhours { get; set; }
        public DateTime start { get; set; }
        [Required]
        public ILocation location { get; set; }

        /// <summary>
        /// Konstruktor des Szenarios.
        /// </summary>
        public Scenario()
        {
            id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Füge dem Szenario ein Fahrzeug hinzu.
        /// </summary>
        /// <param name="vehicle">Das hinzuzufügende Fahrzeug.</param>
        /// <returns>Wahrheitswert entsprechend ob das Hinzufügen erfolgreich war oder nicht.</returns>
        public bool addVehicle(Vehicle vehicle)
        {
            if (vehicles.Contains(vehicle))
                return false;

            vehicles.Add(vehicle);
            return vehicles.Contains(vehicle);
        }

        /// <summary>
        /// Entferne ein Fahrzeug aus dem Szenario.
        /// </summary>
        /// <param name="vehicle">Das zu entfernende Fahrzeug.</param>
        /// <returns>Wahrheitswert entsprechend ob das Entfernen erfolgreich war oder nicht.</returns>
        public bool deleteVehicle(Vehicle vehicle)
        {
            if (!vehicles.Contains(vehicle))
                return false;

            vehicles.Remove(vehicle);
            return !vehicles.Contains(vehicle);
        }

        /// <summary>
        /// Füge dem Szenario eine Stoßzeit hinzu.
        /// </summary>
        /// <param name="rushhour">Die hinzuzufügende Stoßzeit.</param>
        /// <returns>Wahrheitswert entsprechend ob das Hinzufügen erfolgreich war oder nicht.</returns>
        public bool addRushhour(Rushhour rushhour)
        {
            if (rushhours.Contains(rushhour))
                return false;

            rushhours.Add(rushhour);
            return rushhours.Contains(rushhour);
        }

        /// <summary>
        /// Entferne eine Stoßzeit aus dem Szenario.
        /// </summary>
        /// <param name="rushhour">Die zu entfernende Stoßzeit.</param>
        /// <returns>Wahrheitswert entsprechend ob das Entfernen erfolgreich war oder nicht.</returns>
        public bool deleteRushhour(Rushhour rushhour)
        {
            if (!rushhours.Contains(rushhour))
                return false;

            rushhours.Remove(rushhour);
            return !rushhours.Contains(rushhour);
        }
    }
}
