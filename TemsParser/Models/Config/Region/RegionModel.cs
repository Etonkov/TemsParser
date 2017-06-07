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
    /// This class represents a region (geographically).
    /// </summary>
    [Serializable]
    public class RegionModel : ConfigItemBase, IName, IRemoveSpaces, ISortByName
    {
        /// <summary>
        /// Data store for Technologies property.
        /// </summary>
        [XmlIgnore]
        private ObservableCollection<TechnologyModel> _technologies;

        /// <summary>
        /// Data store for Name property.
        /// Used to store the invalid values (that are not found in the Config.RegionsList).
        /// </summary>
        [XmlIgnore]
        private string _name;

        /// <summary>
        /// Data store for RegionListItem property.
        /// </summary>
        [XmlIgnore]
        private RegionListItemModel _regionListItem;


        /// <summary>
        /// Initializes a new instance of the RegionModel class.
        /// Used for XML deserializations.
        /// </summary>
        public RegionModel()
        {
            Technologies = new ObservableCollection<TechnologyModel>();
            Name = String.Empty;

            ConfigPropertyChanged += SubscribeToConfigChanged;
        }

        /// <summary>
        /// Initializes a new instance of the RegionModel class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="regionModel">An instanse of the RegionModel class for copying.</param>
        public RegionModel(RegionModel regionModel) : this()
        {
            Config = regionModel.Config;
            Name = regionModel.Name;
            Technologies = regionModel.Technologies.Clone();
            //Id = regionModel.Id;
        }


        /// <summary>
        /// This is a name of region.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "RegionNameRequired")]
        [Names(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "RegionNames")]
        [MaxLengthTrimmed(16, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "RegionNameMaxLength")]
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                if (RegionListItem != null)
                {
                    return RegionListItem.Name;
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
                        RegionListItem = Config.RegionsList.FirstOrDefault(o => (o.ToString() == value.Trim()));
                    }

                    //if (RegionListItem == null)
                    //{
                        _name = value;
                    //}

                    OnToStringValueChanged();
                }
            }
        }


        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "RegionNameNotListed")]
        [IgnoreGetAllChildren]
        [XmlIgnore]
        public RegionListItemModel RegionListItem
        {
            get { return _regionListItem; }
            set
            {
                if (_regionListItem != value)
                {
                    if (_regionListItem != null)
                    {
                        _regionListItem.Regions.Remove(this);
                    }

                    _regionListItem = value;

                    if ((_regionListItem != null) && (_regionListItem.Regions.Contains(this) == false))
                    {
                        _regionListItem.Regions.Add(this);
                    }

                    OnToStringValueChanged();
                }
            }
        }

        /// <summary>
        /// A list of technologies.
        /// </summary>
        [UniqueValuesCollection]
        [XmlElement("technology")]
        public ObservableCollection<TechnologyModel> Technologies
        {
            get { return _technologies; }
            set
            {
                if (_technologies != value)
                {
                    var oldValue = _technologies;
                    var newValue = value;
                    _technologies = value;
                    var e = new CollectionPropertyChangedEventArgs(oldValue, newValue);
                    OnCollectionPropertyChanged(e);
                }
            }
        }


        void SubscribeToConfigChanged(object sender, ConfigPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                RegionListItem = Config.RegionsList.FirstOrDefault(o => (o.ToString() == this.ToString()));
            }
            else
            {
                //ConfigPropertyChanged -= SubscribeToConfigChanged;
                RegionListItem = null;
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
        /// Sorts list of technologies by name.
        /// This is implementation of ISortByName interface.
        /// </summary>
        public void SortByName()
        {
            Technologies = new ObservableCollection<TechnologyModel>(Technologies.OrderBy(o => o.ToString()));
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
