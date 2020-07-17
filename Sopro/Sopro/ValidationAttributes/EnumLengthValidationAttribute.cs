using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ValidationAttributes
{
    public class EnumLengthAttribute : ValidationAttribute
    {
        int enumlength, min;
        public EnumLengthAttribute(int _min,Type enumType)
        {
            enumlength = Enum.GetNames(enumType).Length;
            min = _min;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= min  && list.Count <= enumlength;
            }
            return false;
        }
    }
}
