using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
