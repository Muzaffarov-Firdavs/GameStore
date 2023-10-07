using GameStore.Service.Commons.Validators;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.Commons.Attributes
{
    public class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null) return new ValidationResult("Set an email.");
            else
            {
                string email = value.ToString()!;
                var result = EmailValidator.IsValid(email);

                if (!result) return new ValidationResult("This email is incorrect syntax.");
                return ValidationResult.Success;
            }
        }
    }
}
