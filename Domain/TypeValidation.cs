using System.ComponentModel.DataAnnotations;
namespace Titanium_MVC.Models
{
    public class TypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                if (stringValue.Contains("file") || stringValue.Contains("plot")|| stringValue.Contains("Plot") || stringValue.Contains("File") )
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("The Area must contain 'file' or 'plot'.");
                }
            }
            return new ValidationResult("Invalid input type.");
        }
    }
}
