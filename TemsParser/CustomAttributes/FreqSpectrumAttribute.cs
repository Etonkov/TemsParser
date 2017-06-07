using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace TemsParser.CustomAttributes
{
    /// <summary>
    /// Specifies that a data field value must be a string representation frequency spectrum like "62,113-124;1022".
    /// Uses comma ',' and semicolon ';' chars for split frequencies and frequency ranges.
    /// Uses dash '-' char for split low and higth frequencies in frequency ranges.
    /// </summary>
    public class FreqSpectrumAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Represent value that indicated whether validation successful or failed.
            // Default - successful.
            bool isValid = true;

            // Split a string representation of spectrum to a string array of freq ranges.
            string[] ranges = value.ToString().Split(new char[] { ';', ',' });


            foreach (var rangeItem in ranges)
            {
                // Split a string representation of spectrum to a string array of freq ranges.
                string[] freqs = rangeItem.Split('-');
                var freqsLength = freqs.Length;

                switch (freqsLength)
                {
                    // If frequency range contains one frequency and its parsing was failed
                    // or frequency values is negative then validation is failed.
                    case 1:
                        {
                            int freq;
                            bool parseResult = Int32.TryParse(freqs[0], out freq);

                            if ((parseResult != true) || (freq < 0))
                            {
                                isValid = false;
                            }
                        }
                    break;
                    // If frequency range contains two frequency(lower and higher) and its parsing was failed
                    // or its values is negative or lower freq greater than higher freq - validation is failed.
                    case 2:
                        {
                            int freqLow;
                            int freqHi;

                            bool freqLowRarseResult = Int32.TryParse(freqs[0], out freqLow);
                            bool freqHiRarseResult = Int32.TryParse(freqs[1], out freqHi);

                            if (!freqLowRarseResult ||
                                !freqHiRarseResult ||
                                (freqLow < 0) ||
                                (freqHi < 0) ||
                                (freqLow > freqHi))
                            {
                                isValid = false;
                            }
                        }
                        break;
                    // Otherwise(frequency range contains more than two frequencies), validation is failed.
                    default:
                        isValid = false;
                        break;
                }
            }

            // Return values and support ErrorMessage and ResourceManager.
            if (isValid == true)
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
                    String.Format(("Значение \"{0}\" нельзя преобразовать в частотный спектр."), value.ToString());

                return new ValidationResult(rmErrorMessage ?? ErrorMessage ?? message);
            }
        }
    }
}
