using System;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Infrastructure
{
    /// <summary>
    /// Die Klasse für den Ladestecker.
    /// </summary>
    public class Plug
    {
        [Range(0, int.MaxValue)]
        public int power { get; set; }
        public PlugType type { get; set; }

        /// <summary>
        /// Erstellt eine tiefe Kopie dieses Objekts
        /// </summary>
        /// <returns>Tiefe Kopie.</returns>
        public Plug deepCopy()
        {
            return new Plug() { power = power, type = type };
        }
    }
   
}
