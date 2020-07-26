using System;
using System.ComponentModel.DataAnnotations;


namespace Sopro.ValidationAttributes
{
    public class BookingStartTimeValidation : ValidationAttribute
    {
        /// <summary>
        /// Überprüft ob der Nutzer einen sinnvollen Startzeitpunkt gewählt hat.
        /// </summary>
        /// <param name="value">gewählter Startzeitpunkt.</param>
        /// <returns>
        /// validiert oder falsifiziert den Variableninhalt.
        /// </returns>
        public override bool IsValid(object value)
        {
            DateTime time = Convert.ToDateTime(value);
            if (time > DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }     
        }
    }
}
