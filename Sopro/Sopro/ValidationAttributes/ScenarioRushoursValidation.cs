using Sopro.Models.Administration;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ValidationAttributes
{
    //wird aktuell nicht gebraucht, vllt später weiter bearbeitet
    public class ScenarioRushoursValidation : ValidationAttribute
    {
        private Rushhour rushhour;
        public override bool IsValid(object value)
        {
            var soc = vehicle.socStart;
            int soc2 = Convert.ToInt16(value);
            if (soc <= soc2 && soc2 <= 100)
                return true;
            else
                return false;
        }
    }
}
