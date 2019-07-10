using System;
using System.ComponentModel;

namespace VerticalSliceArchitecture.Infrastructure.Framework.Helpers
{
    public static class EnumHelper
    {
        public static string ToDescriptionString(this Enum val)
        {
            string defaultValue = val.ToString();
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : defaultValue;
        }
        
    }
}
