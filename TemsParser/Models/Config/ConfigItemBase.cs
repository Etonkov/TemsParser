using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Linq;
using System.Reflection;

using TemsParser.CustomAttributes;
using TemsParser.Resources;
using TemsParser.Messages;
using TemsParser.ViewModels;
using TemsParser.Common;
using TemsParser.Extentions;


namespace TemsParser.Models.Config
{
    /// <summary>
    /// Base class for all childrens configuration model classes.
    /// </summary>
    [Serializable]
    public abstract class ConfigItemBase : IValidationItem
    {
        /// <summary>
        /// Data store for Config property.
        /// </summary>
        private ConfigModel _config;

        /// <summary>
        /// Data store for Id property.
        /// </summary>
        private int _id;

        /// <summary>
        /// Represent the method that executes when the property of ObservationCollection changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// Represent a data of CollectionPropertyChanged event
        /// </param>
        public delegate void CollectionPropertyChangedEventHandler(object sender, CollectionPropertyChangedEventArgs e);

        /// <summary>
        /// Represent the method that executes when the Config property changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// Represent a data of ConfigPropertyChanged event
        /// </param>
        public delegate void ConfigPropertyChangedEventHandler(object sender, ConfigPropertyChangedEventArgs e);

        /// <summary>
        /// Occurs when the property of observation collection changes.
        /// </summary>
        public event CollectionPropertyChangedEventHandler CollectionPropertyChanged;

        /// <summary>
        /// Occurs when the Config property changes.
        /// </summary>
        public event ConfigPropertyChangedEventHandler ConfigPropertyChanged;

        /// <summary>
        /// Occurs when the ToString() value changes.
        /// </summary>
        [field:NonSerialized]
        public event EventHandler ToStringValueChanged;


        /// <summary>
        /// Reference to the configuration. If property changed then sets a new unique value to Id property.
        /// </summary>
        [IgnoreGetAllChildren]
        [XmlIgnore]
        public ConfigModel Config
        {
            get { return _config; }
            set
            {
                if (_config != value)
                {
                    var oldValue = _config;
                    var newValue = value;
                    _config = value;

                    if (_config != null)
                    {
                        _config.GetId(ref _id);
                    }
                    else
                    {
                        _id = 0;
                    }

                    var ea = new ConfigPropertyChangedEventArgs(oldValue, newValue);
                    var handler = ConfigPropertyChanged;

                    if (handler != null)
                    {
                        ConfigPropertyChanged(this, ea);
                    }
                }
            }
        }

        /// <summary>
        /// Unique id of the configuration items.
        /// Minimum value = 1. If Config property is NULL then this property = 0.
        /// </summary>
        [XmlIgnore]
        public int Id
        {
            get { return _id; }
        }


        /// <summary>
        /// Method to run after the changed collection property.
        /// Initializes CollectionPropertyChanged event for this object.
        /// </summary>
        /// <param name="e">CollectionPropertyChanged event data.</param>
        protected void OnCollectionPropertyChanged(CollectionPropertyChangedEventArgs e)
        {
            var handler = CollectionPropertyChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Method to run after ToString() value of this object changed.
        /// Initializes ToStringValueChanged event for this object.
        /// </summary>
        protected void OnToStringValueChanged()
        {
            var handler = ToStringValueChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
