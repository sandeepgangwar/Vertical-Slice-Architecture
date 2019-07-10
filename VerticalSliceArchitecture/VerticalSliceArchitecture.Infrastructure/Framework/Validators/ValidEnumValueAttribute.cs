using System;
using System.ComponentModel.DataAnnotations;

namespace VerticalSliceArchitecture.Infrastructure.Framework.Validators
{
    public class ValidEnumValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type enumType = value.GetType();
            bool valid = Enum.IsDefined(enumType, value);

            if (!valid)
            {
                return new ValidationResult($"{value} is not a valid value for type {enumType.Name}");
            }

            return ValidationResult.Success;
        }
    }
}