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
    /// This class represents a mobile operator.
    /// </summary>
    [Serializable]
    public class OperatorModel : ConfigItemBase, IName, IRemoveSpaces
    {
        /// <summary>
        /// Data store for Freqs property.
        /// </summary>
        [XmlIgnore]
        private ObservableCollection<FreqModel> _freqs;

        /// <summary>
        /// Data store for Name property.
        /// Used to store the invalid values (that are not found in the Config.OperatorsList).
        /// </summary>
        [XmlIgnore]
        private string _name;

        /// <summary>
        /// Data store for Name property.
        /// </summary>
        [XmlIgnore]
        private OperatorListItemModel _operatorListItem;


        /// <summary>
        /// Initializes a new instance of the OperatorModel class.
        /// Used for XML deserializations.
        /// </summary>
        public OperatorModel()
        {
            Freqs = new ObservableCollection<FreqModel>();
            Name = String.Empty;

            ConfigPropertyChanged += SubscribeToConfigChanged;
        }


        /// <summary>
        /// Initializes a new instance of the OperatorModel class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="operatorModel">An instanse of the OperatorModel class for copying.</param>
        public OperatorModel(OperatorModel operatorModel): this()
        {
            Config = operatorModel.Config;
            Name = operatorModel.Name;
            Freqs = operatorModel.Freqs.Clone();
            //Id = operatorModel.Id;
        }


        /// <summary>
        /// This is a name of operator.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNameRequired")]
        [Names(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNames")]
        [MaxLengthTrimmed(16, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNameMaxLength")]
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                if (OperatorListItem != null)
                {
                    return OperatorListItem.Name;
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
                        OperatorListItem = Config.OperatorsList.FirstOrDefault(o => (o.ToString() == value.Trim()));
                    }

                    //if (OperatorListItem == null)
                    //{
                        _name = value;
                    //}

                    OnToStringValueChanged();
                }
            }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNameNotListed")]
        [IgnoreGetAllChildren]
        [XmlIgnore]
        public OperatorListItemModel OperatorListItem
        {
            get { return _operatorListItem; }
            set
            {
                if (_operatorListItem != value)
                {
                    if (_operatorListItem != null)
                    {
                        _operatorListItem.Operators.Remove(this);
                    }

                    _operatorListItem = value;

                    if ((_operatorListItem != null) && (_operatorListItem.Operators.Contains(this) == false))
                    {
                        _operatorListItem.Operators.Add(this);
                    }

                    OnToStringValueChanged();
                }
            }
        }

        /// <summary>
        /// A list of frequencies.
        /// </summary>
        [XmlElement("freqList")]
        public ObservableCollection<FreqModel> Freqs
        {
            get { return _freqs; }
            set
            {
                if (_freqs != value)
                {
                    var oldValue = _freqs;
                    var newValue = value;
                    _freqs = value;
                    var e = new CollectionPropertyChangedEventArgs(oldValue, newValue);
                    OnCollectionPropertyChanged(e);
                }
            }
        }


        void SubscribeToConfigChanged(object sender, ConfigPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                OperatorListItem = Config.OperatorsList.FirstOrDefault(o => (o.ToString() == this.ToString()));
            }
            else
            {
                //ConfigPropertyChanged -= SubscribeToConfigChanged;
                OperatorListItem = null;
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
        /// Overrides ToString() method.
        /// </summary>
        public override string ToString()
        {
            return Name.Trim();
        }
    }
}
