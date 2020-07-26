using System.Collections;
using System.ComponentModel.DataAnnotations;


namespace Sopro.CustomValidationAttributes
{
    public class ListMinLengthAttribute : ValidationAttribute
    {
        private readonly int min;
        public ListMinLengthAttribute(int _min)
        {
            min = _min;
        }
        /// <summary>
        /// Überprüft ob die Anzahl der Elemente einer Liste das übergebene Minimum beträgt.
        /// </summary>
        /// <param name="value">übergebene Liste.</param>
        /// <returns>
        /// erzeugt eine erfolgreiche Validierung, falls die Liste genug Elemente enthält.
        /// </returns>
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= min;
            }
            return false;
        }
    }
}
