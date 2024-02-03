using System.ComponentModel.DataAnnotations;

namespace MoneyBudgets.Validations
{
    public class FirstCapitalLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null | string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var firstLetter = value.ToString()?[0].ToString();

            if(firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("The first letter must be capital.");
            }

            return ValidationResult.Success;            
        }
    }
}
