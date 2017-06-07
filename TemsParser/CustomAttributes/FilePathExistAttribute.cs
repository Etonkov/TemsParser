using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.IO;

namespace TemsParser.CustomAttributes
{
    /// <summary>
    /// Specifies that a file path is exits.
    /// </summary>
    public class FilePathExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string filePath = value.ToString();

            if (File.Exists(filePath))
	        {
                return ValidationResult.Success;
	        }
            else
            {
                ResourceManager rm;
                string rmErrorMessage = null;

                if (ErrorMessageResourceType != null)
                {
                    rm = new ResourceManager(ErrorMessageResourceType);
                    rmErrorMessage = rm.GetString(ErrorMessageResourceName, Thread.CurrentThread.CurrentCulture);
                }

                var message =
                    String.Format(("Файл \"{0}\" не найден."), value.ToString());

                return new ValidationResult(rmErrorMessage ?? ErrorMessage ?? message);
            }
        }
    }
}
