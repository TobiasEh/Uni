using Sopro.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Infrastructure
{
    /// <summary>
    /// Die Klasse der Ladestation. Ladestationen sind in Ladezonen enthalten.
    /// Ladestationen enhalten selbst verschiedene Stecker.
    /// </summary>
    public class Station
    {
        [ListMinLength(1)]
        public List<Plug> plugs { get; set; } = new List<Plug>() { new Plug() { power = 0, type = PlugType.CCS} };
        public string manufacturer { get; set; }
        [Range(0, int.MaxValue)]
        public int maxPower { get; set; }
        [Range(0, int.MaxValue)]
        public int maxParallelUseable { get; set; }
        public int id { get; set; }

        /// <summary>
        /// Füge der Station einen Stecker hinzu.
        /// </summary>
        /// <param name="plug">Der hinzuzufügende Stecker.</param>
        /// <returns>Wahrheitswert enstprechend ob das Hinzufügen erfolgreich war oder nicht.</returns>
        public bool addPlug(Plug plug)
        {
            if (plugs.Contains(plug))
                return false;

            plugs.Add(plug);
            return plugs.Contains(plug);
        }

        /// <summary>
        /// Entferne einen Stecker aus der Station.
        /// </summary>
        /// <param name="plug">Der zu entfernende Stecker.</param>
        /// <returns>Wahrheitswert entsprechend ob das Entfernen erfolgreich war oder nicht.</returns>
        public bool deletePlug(Plug plug)
        {
            if (plugs.Contains(plug))
            {
                plugs.Remove(plug);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
