using System;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace TemsParser.CustomAttributes
{
    public enum CoordinateType { Latitude, Longitude };

    /// <summary>
    /// Specifies that a data field value must be a coordinate type
    /// </summary>
    public class CoordinateAttribute : ValidationAttribute
    {
        private CoordinateType _coordinateType;

        public CoordinateAttribute(CoordinateType coordinateType)
        {
            _coordinateType = coordinateType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            double coordinate;
            bool isValid = false;
            string message = String.Empty;

            bool parseResult = Double.TryParse(value.ToString().Replace(",", "."),
                                               NumberStyles.Number,
                                               CultureInfo.InvariantCulture.NumberFormat,
                                               out coordinate);

            if (parseResult == true)
            {
                if (_coordinateType == CoordinateType.Latitude)
                {
                    if ((coordinate > 90) || (coordinate < -90))
                    {
                        message = "Введите широту от -90 до 90, например: 51.56938.";
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                else if (_coordinateType == CoordinateType.Longitude)
                {
                    if ((coordinate > 180) || (coordinate < -180))
                    {
                        message = "Введите долготу от -180 до 180, например: 36.07397.";
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }
            else
            {
                message = String.Format("Значение '{0}' не может быть преобразовано в координату.", value);
            }

            if (isValid)
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

                return new ValidationResult(rmErrorMessage ?? ErrorMessage ?? message);
            }
        }
    }
}
