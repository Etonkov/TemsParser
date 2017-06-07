using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Globalization;
using System.Threading;


namespace TemsParser.CustomAttributes
{
    /// <summary>
    /// Specifies that a boolean value must be equals true.
    /// </summary>
    public class EqualsTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool unboxValue;
            string displayName;

            if (validationContext != null)
            {
                displayName = validationContext.DisplayName;
            }
            else
            {
                displayName = String.Empty;
            }

            try
            {
                unboxValue = (bool)value;
            }
            catch (InvalidCastException)
            {
                var message =
                    String.Format("{0}:{1} is invalid type. This attribute may only be used with the Boolean type",
                                  displayName,
                                  value.GetType());

                throw new ApplicationException(message);
            }


            if (unboxValue == true)
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

                var message = String.Format("Значение поля \"{0}\" не верное(false).",
                                            displayName);

                return new ValidationResult(rmErrorMessage ?? ErrorMessage ?? message);
            }
        }
    }
}
