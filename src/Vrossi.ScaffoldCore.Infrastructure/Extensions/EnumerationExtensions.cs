using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Extensions
{
    public static class EnumerationExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var enumAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (enumAttributes == null) return string.Empty;

            return enumAttributes.Length > 0 ? enumAttributes[0].Name : value.ToString();
        }
    }
}
