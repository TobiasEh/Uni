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
