using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Sopro.ValidationAttributes
{
    public class BookingPlugsValidation : ValidationAttribute
    {
        /// <summary>
        /// Überprüft ob es Duplikate in der plugliste einer Buchung gibt.
        /// </summary>
        /// <param name="value">Liste der Plugs.</param>
        /// <returns>
        /// validiert oder falsifiziert den Variableninhalt.
        /// </returns>
        public override bool IsValid(object value)
        {
            List<PlugType> plugarray = value as List<PlugType>;
            if (plugarray != null) { 
                
                if (plugarray.Count >= 1 && plugarray.Count() == plugarray.Distinct().Count())
                {
                    return true;
                } 
            }
            return false;
        }
    }
}
