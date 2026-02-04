using System.ComponentModel.DataAnnotations;

namespace SpaceFlow.Core.CustomValidation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value is DateTime dateTimeValue)
                return dateTimeValue >= DateTime.Now;

            return true;
        }
    }
}
