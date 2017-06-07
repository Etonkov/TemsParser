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
using TemsParser.Common;
using TemsParser.Extentions.Model.Config;

namespace TemsParser.Models.Config
{
    /// <summary>
    /// This class represents a technology of mobile radio access.
    /// </summary>
    [Serializable]
    public class TechnologyModel : ConfigItemBase, IName, IRemoveSpaces, ISortByName
    {
        /// <summary>
        /// Data store for Operators property.
        /// </summary>
        [XmlIgnore]
        private ObservableCollection<OperatorModel> _operators;

        /// <summary>
        /// Data store for Name property.
        /// Used to store the invalid values (that are not found in the Config.TechnologiesList).
        /// </summary>
        [XmlIgnore]
        private string _name;

        /// <summary>
        /// Data store for TechnologyListItem property.
        /// </summary>
        [XmlIgnore]
        private TechnologyListItemModel _technologyListItem;


        /// <summary>
        /// Initializes a new instance of the TechnologyModel class.
        /// Used for XML deserializations.
        /// </summary>
        public TechnologyModel()
        {
            Operators = new ObservableCollection<OperatorModel>();
            Name = String.Empty;

            ConfigPropertyChanged += SubscribeToConfigChanged;
        }

        /// <summary>
        /// Initializes a new instance of the TechnologyModel class.
        /// New instance contains elements copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="technology">An instanse of the TechnologyModel class for copying.</param>
        public TechnologyModel(TechnologyModel technology): this()
        {
            Config = technology.Config;
            Name = technology.Name;
            Operators = technology.Operators.Clone();
            //Id = technology.Id;
        }


        /// <summary>
        /// This is a name of technology.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechnologyNameRequired")]
        [Names(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechNames")]
        [MaxLengthTrimmed(16, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechNameMaxLength")]
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                if (TechnologyListItem != null)
                {
                    return TechnologyListItem.Name;
                }
                else if (_name != null)
                {
                    return _name;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if (Name != value)
                {
                    if (Config != null)
                    {
                        TechnologyListItem = Config.TechnologiesList
                                                 .FirstOrDefault(o => (o.ToString() == value.Trim()));
                    }

                    //if (TechnologyListItem == null)
                    //{
                        _name = value;
                    //}

                    OnToStringValueChanged();
                }
            }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechnologyNameNotListed")]
        [IgnoreGetAllChildren]
        [XmlIgnore]
        public TechnologyListItemModel TechnologyListItem
        {
            get { return _technologyListItem; }
            set
            {
                if (_technologyListItem != value)
                {
                    if (_technologyListItem != null)
                    {
                        _technologyListItem.Technologies.Remove(this);
                    }

                    _technologyListItem = value;

                    if ((_technologyListItem != null) && (_technologyListItem.Technologies.Contains(this) == false))
                    {
                        _technologyListItem.Technologies.Add(this);
                    }

                    OnToStringValueChanged();
                }
            }
        }

        /// <summary>
        /// A list of operators.
        /// </summary>
        [UniqueValuesCollection]
        [XmlElement("operator")]
        public ObservableCollection<OperatorModel> Operators
        {
            get { return _operators; }
            set
            {
                if (_operators != value)
                {
                    var oldValue = _operators;
                    var newValue = value;
                    _operators = value;
                    var e = new CollectionPropertyChangedEventArgs(oldValue, newValue);
                    OnCollectionPropertyChanged(e);
                }
            }
        }


        void SubscribeToConfigChanged(object sender, ConfigPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                TechnologyListItem = Config.TechnologiesList.FirstOrDefault(o => (o.ToString() == Name.Trim()));
            }
            else
            {
                //ConfigPropertyChanged -= SubscribeToConfigChanged;
                TechnologyListItem = null;
            }
        }

        /// <summary>
        /// Removes white spases in Name property.
        /// This is implementation of IRemoveSpaces interface.
        /// </summary>
        public void RemoveSpaces()
        {
            Name = Name.Trim();
        }

        /// <summary>
        /// Sorts list of operators by name.
        /// This is implementation of ISortByName interface.
        /// </summary>
        public void SortByName()
        {
            Operators = new ObservableCollection<OperatorModel>(Operators.OrderBy(o => o.ToString()));
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
