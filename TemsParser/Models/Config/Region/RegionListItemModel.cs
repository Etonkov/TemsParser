using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Linq;

using TemsParser.CustomAttributes;
using TemsParser.Resources;
using TemsParser.Extentions;


namespace TemsParser.Models.Config
{
    /// <summary>
    /// This class represents an item of region list.
    /// </summary>
    [Serializable]
    public class RegionListItemModel : IName, IRemoveSpaces, IValidationItem
    {
        /// <summary>
        /// Initializes a new instance of the RegionListItemModel class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="regionListItem">An instanse of the RegionListItemModel class for copying.</param>

        /// <summary>
        /// Initializes a new instance of the RegionListItemModel class.
        /// Used for XML deserializations.
        /// </summary>
        public RegionListItemModel()
        {
            Name = String.Empty;
            Regions = new List<RegionModel>();
        }




        /// <summary>
        /// Initializes a new instance of the RegionListItemModel class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="regionListItem">An instanse of the RegionListItemModel class for copying.</param>
        /// <param name="shallowCopyRegions">Determinds create shallow copy of regions or not.</param>
        public RegionListItemModel(RegionListItemModel regionListItem, bool shallowCopyRegions = false)
            : this()
        {
            Name = regionListItem.Name;

            if (shallowCopyRegions)
            {
                Regions = new List<RegionModel>(regionListItem.Regions);
            }
        }


        /// <summary>
        /// This is a name of region list item.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "RegionNameRequired")]
        [Names(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "RegionNames")]
        [MaxLengthTrimmed(16, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "RegionNameMaxLength")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [IgnoreGetAllChildren]
        [XmlIgnore]
        public List<RegionModel> Regions { get; set; }

        /// <summary>
        /// Removes white spases in Name property.
        /// This is implementation of IRemoveSpaces interface.
        /// </summary>
        public void RemoveSpaces()
        {
            Name = Name.Trim();
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
