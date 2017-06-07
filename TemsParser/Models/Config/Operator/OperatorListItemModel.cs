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
    /// This class represents an item of the operator list.
    /// </summary>
    [Serializable]
    public class OperatorListItemModel : IName, IRemoveSpaces, IValidationItem
    {
        /// <summary>
        /// Initializes a new instance of the OperatorListItem class.
        /// Used for XML deserializations.
        /// </summary>
        public OperatorListItemModel()
        {
            Name = String.Empty;
            Operators = new List<OperatorModel>();
        }

        /// <summary>
        /// Initializes a new instance of the OperatorListItem class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="operatorListItem">An instanse of the OperatorListItem class for copying.</param>
        public OperatorListItemModel(OperatorListItemModel operatorListItem, bool shallowCopyOperators = false)
            : this()
        {
            Name = operatorListItem.Name;

            if (shallowCopyOperators)
            {
                Operators = new List<OperatorModel>(operatorListItem.Operators);
            }
        }


        /// <summary>
        /// This is a name of operator list item.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNameRequired")]
        [Names(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNames")]
        [MaxLengthTrimmed(16, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNameMaxLength")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [IgnoreGetAllChildren]
        [XmlIgnore]
        public List<OperatorModel> Operators { get; set; }


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
