using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Linq;
using System.Resources;

using TemsParser.CustomAttributes;
using TemsParser.Resources;
using TemsParser.Extentions;


namespace TemsParser.Models.Config
{
    /// <summary>
    /// This class represents a radio frequency(absolute radio frequency channel numbers - ARFCN's).
    /// </summary>
    [Serializable]
    public class FreqModel : ConfigItemBase, IRemoveSpaces
    {
        /// <summary>
        /// Data store for Spectrum property.
        /// </summary>
        [XmlIgnore]
        private string _spectrum;


        /// <summary>
        /// Initializes a new instance of the FreqModel class.
        /// Used for XML deserializations.
        /// </summary>
        public FreqModel()
        {
            Spectrum = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the FreqModel class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="freqModel">An instanse of the FreqModel class for copying.</param>
        public FreqModel(FreqModel freqModel)
        {
            //Config = freqModel.Config;
            Spectrum = freqModel.Spectrum;
            //Id = freqModel.Id;
        }


        /// <summary>
        /// List of frequency values(ARFCN's).
        /// <example>1,2,5-48;1022</example>
        /// </summary>
        [FreqSpectrum]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "FreqValuesRequired")]
        [XmlText]
        public string Spectrum
        {
            get { return _spectrum; }
            set
            {
                if (_spectrum != value)
                {
                    _spectrum = value;
                    OnToStringValueChanged();
                }
            }
        }

        
        /// <summary>
        /// Removes white spases in Spectrum property.
        /// This is implementation of IRemoveSpaces interface.
        /// </summary>
        public void RemoveSpaces()
        {
            Spectrum = Spectrum.RemoveWhiteSpace();
        }

        /// <summary>
        /// Overrides ToString() method.
        /// </summary>
        public override string ToString()
        {
            return Spectrum.Trim();
        }
    }
}
