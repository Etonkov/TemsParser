using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Linq;

namespace TemsParser.CustomAttributes
{
    /// <summary>
    /// Specifies that a values in collection must be unique.
    /// Uses ToString() value for comparison.
    /// </summary>
    public class UniqueValuesCollectionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var objCollection = value as IEnumerable<object>;

            if (objCollection == null)
            {
                string errorMessage = String.Format("Поле {0} не является типом перечисления.", value.GetType());
                throw new ApplicationException(errorMessage);
            }

            var stringCollection = objCollection
                                       .Where(o => (o.ToString().Trim() != String.Empty))
                                       .Select(o => o.ToString().Trim());

            var list = new List<dynamic>();
            var notUniqueValues = new List<string>();

            foreach (var item in stringCollection)
            {
                if (list.Contains(item))
                {
                    notUniqueValues.Add(item);
                }
                else
                {
                    list.Add(item);
                }
            }

            if (notUniqueValues.Count > 0)
            {
                ResourceManager rm;
                string rmErrorMessage = null;

                if (ErrorMessageResourceType != null)
                {
                    rm = new ResourceManager(ErrorMessageResourceType);
                    rmErrorMessage = rm.GetString(ErrorMessageResourceName, Thread.CurrentThread.CurrentCulture);
                }

                var message =
                    String.Format("Следующие элементы повторяются в списке: {0}.", String.Join(", ", notUniqueValues));

                return new ValidationResult(rmErrorMessage ?? ErrorMessage ?? message);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
