using Sopro.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using Sopro.Models.Infrastructure;
using Sopro.Interfaces.ControllerSimulation;
using System.Collections.Generic;

namespace Sopro.Models.Simulation
{
    /// <summary>
    /// Klasse die ein Fahrzeug beschreibt. Fahrzeuge sind als Buchungstemplates anzusehen.
    /// Aus Fahrzeugen werden während der Simulation Buchungen generiert.
    /// </summary>
    public class Vehicle : IVehicle
    {
        public string id { get; set; }
        [Required(ErrorMessage ="Auto ohne name is traurig")]
        public string model { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Positive Ganze Zahl eingeben!")]
        public int capacity { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Start SoC zwischen 0 und 100%")]
        public int socStart { get; set; }
        [Required]
        [BookingSocEndValidation(ErrorMessage = "End SoC sollte größer sein als start SoC!")]
        public int socEnd { get; set; }
        [Required]
        [EnumLength(1, typeof(PlugType), ErrorMessage = "Mindestens 1 Plug!")]
        public List<PlugType> plugs { get; set; } = new List<PlugType>();
        
        /// <summary>
        /// Konstruktor des Fahrzeugs.
        /// </summary>
        public Vehicle()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}
