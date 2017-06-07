using System;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace TemsParser.CustomAttributes
{
    /// <summary>
    /// Specifies that a data field value must be contain only letter, digit and spases.
    /// </summary>
    public class NamesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isValid = true;

            var chars = value.ToString().ToCharArray();

            foreach (var item in chars)
            {
                if (!(Char.IsLetterOrDigit(item) || item == ' '))
                {
                    isValid = false;
                }
            }


            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                ResourceManager rm;
                string rmErrorMessage = null;
                string message = "Наименование должно состоять только из цифр, букв и пробелов.";


                if (ErrorMessageResourceType != null)
                {
                    rm = new ResourceManager(ErrorMessageResourceType);
                    rmErrorMessage = rm.GetString(ErrorMessageResourceName, Thread.CurrentThread.CurrentCulture);
                }

                return new ValidationResult(rmErrorMessage ?? ErrorMessage ?? message);
            }
        }
    }
}
