using System.ComponentModel.DataAnnotations;

namespace SpaceFlow.Core.CustomValidation
{
    public class DateGreaterAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public DateGreaterAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
            ErrorMessage = "End Date must be greater than Start Date";
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = value as DateTime?;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            
            if (property == null) throw new ArgumentException("Property with this name not found");
            
            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (currentValue.HasValue && comparisonValue.HasValue && currentValue.Value <= comparisonValue.Value)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
