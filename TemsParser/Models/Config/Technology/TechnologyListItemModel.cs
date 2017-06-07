using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TemsParser.CustomAttributes;
using TemsParser.Resources;

namespace TemsParser.Models.Config
{
    /// <summary>
    /// This class represents an item of technology list.
    /// </summary>
    [Serializable]
    public class TechnologyListItemModel : IName, IRemoveSpaces, IValidationItem
    {
        /// <summary>
        /// Initializes a new instance of the TechnologyListItemModel class.
        /// Used for XML deserializations.
        /// </summary>
        public TechnologyListItemModel()
        {
            Name = String.Empty;
            LatitudeColumnName = String.Empty;
            LongitudeColumnName = String.Empty;
            FreqColumnNamePart = String.Empty;
            LevelColumnNamePart = String.Empty;
            Technologies = new List<TechnologyModel>();
        }

        /// <summary>
        /// Initializes a new instance of the TechnologyListItemModel class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="technologyListItem">An instanse of the TechnologyListItemModel class for copying.</param>
        public TechnologyListItemModel(TechnologyListItemModel technologyListItem, bool shallowCopyTechnologies = false)
            : this()
        {
            Name = technologyListItem.Name;
            LatitudeColumnName = technologyListItem.LatitudeColumnName;
            LongitudeColumnName = technologyListItem.LongitudeColumnName;
            FreqColumnNamePart = technologyListItem.FreqColumnNamePart;
            LevelColumnNamePart = technologyListItem.LevelColumnNamePart;

            if (shallowCopyTechnologies)
            {
                Technologies = new List<TechnologyModel>(technologyListItem.Technologies);
            }
        }


        /// <summary>
        /// This is a name of technology list item.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechnologyNameRequired")]
        [Names(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechNames")]
        [MaxLengthTrimmed(16, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechNameMaxLength")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// This is a name of latitude column.
        /// Used to parse a latitude from the file for the specified technology.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "LatitudeColumnNameRequired")]
        [XmlElement("latitudeColumnName")]
        public string LatitudeColumnName { get; set; }

        /// <summary>
        /// This is a name of longitude column.
        /// Used to parse a longitude from the file for the specified technology.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "LongitudeColumnNameRequired")]
        [XmlElement("longitudeColumnName")]
        public string LongitudeColumnName { get; set; }

        /// <summary>
        /// This is a part of the name of the frequency column.
        /// Used to parse a frequency values from the file for the specified technology.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "FreqColumnNamePartRequired")]
        [XmlElement("freqColumnNamePart")]
        public string FreqColumnNamePart { get; set; }

        /// <summary>
        /// This is a part of the name of the level column data.
        /// Used to parse a level values from the file for the specified technology.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "LevelColumnNamePartRequired")]
        [XmlElement("levelColumnNamePart")]
        public string LevelColumnNamePart { get; set; }

        [IgnoreGetAllChildren]
        [XmlIgnore]
        public List<TechnologyModel> Technologies { get; set; }


        /// <summary>
        /// Removes white spases in Name property.
        /// This is implementation of IRemoveSpaces interface.
        /// </summary>
        public void RemoveSpaces()
        {
            Name = Name.Trim();
            LatitudeColumnName = LatitudeColumnName.Trim();
            LongitudeColumnName = LongitudeColumnName.Trim();
            FreqColumnNamePart = FreqColumnNamePart.Trim();
            LevelColumnNamePart = LevelColumnNamePart.Trim();
        }

        /// <summary>
        /// Overrides ToString() method.
        /// </summary>
        public override string ToString()
        {
            return Name.Trim();
        }
    }
}
