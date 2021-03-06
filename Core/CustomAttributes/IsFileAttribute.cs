using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.CustomAttributes
{
    internal class IsFileAttribute : ValidationAttribute
    {
        private string Extensions { get; set; } = "png,jpg,jpeg,pdf,docx,xlsx";

        public IsFileAttribute (string errorMessage = "")
        {
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid (object value, ValidationContext validationContext)
        {
            try
            {
                IFormFile? file = value as IFormFile;

                bool isValid = true;

                List<string> allowedExtensions = Extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                // Verification.  
                if (value != null)
                {
                    // Initialization.  
                    var fileName = file!.FileName;

                    // Settings.  
                    isValid = allowedExtensions.Any(y => fileName.EndsWith(y));

                    if (isValid)
                    {
                        return ValidationResult.Success;
                    }
                }

                if (value == null)
                {
                    return ValidationResult.Success;
                }
            }
            catch (Exception)
            { }

            return new ValidationResult(ErrorMessage);
        }
    }
}
