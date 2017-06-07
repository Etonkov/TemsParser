using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Specialized;
using System.Reflection;

using TemsParser.CustomAttributes;
using TemsParser.Extentions;
using TemsParser.Common;
using System.Diagnostics;
using TemsParser.Extentions.Model.Config;

namespace TemsParser.Models.Config
{
    /// <summary>
    /// This class represents a configuration.
    /// Configuration is used to define parameters of parsing.
    /// Contains a hierarchical structure: ConfigModel - RegionModel - TechnologyModel - OperatorModel - FreqModel.
    /// </summary>
    [Serializable]
    [XmlRoot("config")]
    public class ConfigModel : ISortByName, IValidationItem
    {
        #region Fields

        /// <summary>
        /// Data store for CurrentId property.
        /// </summary>
        private int _currentId;

        /// <summary>
        /// Data store for RegionsList property.
        /// </summary>
        private ObservableCollection<RegionListItemModel> _regionsList;

        /// <summary>
        /// Data store for TechnologiesList property.
        /// </summary>
        private ObservableCollection<TechnologyListItemModel> _technologiesList;

        /// <summary>
        /// Data store for OperatorsList property.
        /// </summary>
        private ObservableCollection<OperatorListItemModel> _operatorsList;

        /// <summary>
        /// Data store for Regions property.
        /// </summary>
        private ObservableCollection<RegionModel> _regions;

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ConfigModel class.
        /// Used for XML deserializations.
        /// </summary>
        public ConfigModel()
        {
            _currentId = 0;
            RegionsList = new ObservableCollection<RegionListItemModel>() { };
            TechnologiesList = new ObservableCollection<TechnologyListItemModel>() { };
            OperatorsList = new ObservableCollection<OperatorListItemModel>() { };
            Regions = new ObservableCollection<RegionModel>();
        }

        /// <summary>
        /// Initializes a new instance of the ConfigModel class with default values.
        /// Use if deserializations failed.
        /// </summary>
        /// <param name="useDefaults">Determines whether to use the default values.</param>
        public ConfigModel(bool useDefaults = false) : this()
        {
            if (useDefaults == true)
            {
                RegionsList = new ObservableCollection<RegionListItemModel>()
                {
                    new RegionListItemModel() { Name = "Брянск" },
                    new RegionListItemModel() { Name = "Владимир" },
                    new RegionListItemModel() { Name = "Калуга" },
                    new RegionListItemModel() { Name = "Курск" },
                    new RegionListItemModel() { Name = "НН" },
                    new RegionListItemModel() { Name = "Орёл" },
                    new RegionListItemModel() { Name = "Рязань" },
                    new RegionListItemModel() { Name = "Тула" }
                };

                TechnologiesList = new ObservableCollection<TechnologyListItemModel>()
                {
                    new TechnologyListItemModel()
                    {
                        Name = "2G",
                        LatitudeColumnName = "All-Latitude",
                        LongitudeColumnName = "All-Longitude",
                        FreqColumnNamePart = "All-Scanned ARFCN",
                        LevelColumnNamePart = "All-Scanned RxLev (dBm)"
                    },
                    new TechnologyListItemModel()
                    {
                        Name = "3G",
                        LatitudeColumnName = "All-Latitude",
                        LongitudeColumnName = "All-Longitude",
                        FreqColumnNamePart = "All-Sc Best UARFCN",
                        LevelColumnNamePart = "All-Sc Best Aggr Ec (dBm)"
                    },
                    new TechnologyListItemModel()
                    {
                        Name = "4G",
                        LatitudeColumnName = "All-Latitude",
                        LongitudeColumnName = "All-Longitude",
                        FreqColumnNamePart = "All-Sc Best EARFCN",
                        LevelColumnNamePart = "All-Sc Best RSRP (dBm)"
                    }
                };

                OperatorsList = new ObservableCollection<OperatorListItemModel>()
                { 
                    new OperatorListItemModel() { Name = "МегаФон" },
                    new OperatorListItemModel() { Name = "БиЛайн" },
                    new OperatorListItemModel() { Name = "МТС" },
                    new OperatorListItemModel() { Name = "Tele2"}
                };

                Regions = new ObservableCollection<RegionModel>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the ConfigModel class.
        /// New instance is copied from the input argument.
        /// Using a deep copy.
        /// </summary>
        /// <param name="config">Input argument which to be copied.</param>
        public ConfigModel(ConfigModel config) : this()
        {
            RegionsList = config.RegionsList.Clone();
            TechnologiesList = config.TechnologiesList.Clone();
            OperatorsList = config.OperatorsList.Clone();
            Regions = config.Regions.Clone();
        }

        #endregion


        #region Properties

        /// <summary>
        /// Last and largest identification number of config item.
        /// </summary>
        [XmlIgnore]
        public int CurrentId
        {
            get { return _currentId; }
        }

        /// <summary>
        /// List of all availible regions.
        /// </summary>
        [UniqueValuesCollection]
        [XmlArray(ElementName = "regionsList", Order = 0)]
        [XmlArrayItem("item")]
        public ObservableCollection<RegionListItemModel> RegionsList
        {
            get { return _regionsList; }
            set
            {
                if (_regionsList != value)
                {
                    if (_regionsList != null)
                    {
                        var resetRegions = from r in _regionsList.SelectMany(o => o.Regions)
                                           where _regionsList.Contains(r.RegionListItem)
                                           select r;

                        resetRegions = resetRegions.ToList();

                        foreach (var resetRegion in resetRegions)
                        {
                            resetRegion.RegionListItem = null;
                        }
                    }

                    _regionsList = value;

                    if ((_regionsList != null) && (Regions != null))
                    {
                        foreach (var regionItem in Regions)
                        {
                            bool regionNotListed = (_regionsList.Contains(regionItem.RegionListItem) == false);

                            if ((regionItem.RegionListItem == null) || regionNotListed)
                            {
                                regionItem.RegionListItem =
                                    _regionsList.FirstOrDefault(o => (o.ToString() == regionItem.ToString()));
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// List of all availible mobile technologies.
        /// </summary>
        [UniqueValuesCollection]
        [XmlArray(ElementName = "technologiesList", Order = 1)]
        [XmlArrayItem("item")]
        public ObservableCollection<TechnologyListItemModel> TechnologiesList
        {
            get { return _technologiesList; }
            set
            {
                if (_technologiesList != value)
                {
                    if (_technologiesList != null)
                    {
                        var resetTechnology = from t in _technologiesList.SelectMany(o => o.Technologies)
                                              where _technologiesList.Contains(t.TechnologyListItem)
                                              select t;

                        resetTechnology = resetTechnology.ToList();

                        foreach (var resetRegion in resetTechnology)
                        {
                            resetRegion.TechnologyListItem = null;
                        }
                    }

                    _technologiesList = value;

                    if ((_technologiesList != null) && (Regions != null))
                    {
                        foreach (var technologyItem in Regions.SelectMany(o => o.Technologies))
                        {
                            bool technologyNotListed = !_technologiesList.Contains(technologyItem.TechnologyListItem);

                            if ((technologyItem.TechnologyListItem == null) || technologyNotListed)
                            {
                                technologyItem.TechnologyListItem =
                                    _technologiesList.FirstOrDefault(o => (o.ToString() == technologyItem.ToString()));
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// List of all availible mobile operators.
        /// </summary>
        [UniqueValuesCollection]
        [XmlArray(ElementName = "operatorsList", Order = 2)]
        [XmlArrayItem("item")]
        public ObservableCollection<OperatorListItemModel> OperatorsList
        {
            get { return _operatorsList; }
            set
            {
                if (_operatorsList != value)
                {
                    if (_operatorsList != null)
                    {
                        var resetOperators = from o in _operatorsList.SelectMany(o => o.Operators)
                                             where _operatorsList.Contains(o.OperatorListItem)
                                             select o;

                        resetOperators = resetOperators.ToList();

                        foreach (var resetOperator in resetOperators)
                        {
                            resetOperator.OperatorListItem = null;
                        }
                    }

                    _operatorsList = value;

                    if ((_operatorsList != null) && (Regions != null))
                    {
                        var operators = Regions
                                            .SelectMany(o => o.Technologies)
                                            .SelectMany(o => o.Operators);

                        foreach (var operatorItem in operators)
                        {
                            bool operatorNotListed = !_operatorsList.Contains(operatorItem.OperatorListItem);

                            if ((operatorItem.OperatorListItem == null) || operatorNotListed)
                            {
                                operatorItem.OperatorListItem =
                                    _operatorsList.FirstOrDefault(o => (o.ToString() == operatorItem.ToString()));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// List of regions.
        /// </summary>
        [UniqueValuesCollection]
        [XmlElement(ElementName = "region", Order = 3)]
        public ObservableCollection<RegionModel> Regions
        {
            get { return _regions; }
            set
            {
                if (_regions != value)
                {
                    var oldValue = _regions;
                    var newValue = value;
                    _regions = value;
                    var ea = new CollectionPropertyChangedEventArgs(oldValue, newValue);
                    SubscribeAndUnsubscribeOnPropertyChanged(this, ea);
                }
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Running this method occurs after deserialization.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnXmlDeserialized();
        }

        /// <summary>
        /// Subscribe and unsubscribe to the CollectionChanged event when the ObservableCollection property is changed.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Information about the event.</param>
        private void SubscribeAndUnsubscribeOnPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                UnsubscribeObservableCollectionChanged(e.OldValue);
                ResetConfigAndUnubscribeAllConfigItemBase(e.OldValue);
            }

            if (e.NewValue != null)
            {
                SubscribeObservableCollectionChanged(e.NewValue);
                SetConfigAndSubscribeAllConfigItemBase(e.NewValue);
            }
        }

        /// <summary>
        /// Subscribe to the CollectionChanged event for all INotifyCollectionChanged objects contained in item.
        /// </summary>
        /// <param name="item">An object for subscribe</param>
        private void SubscribeObservableCollectionChanged(object item)
        {
            var observableCollections = item.GetAllChildren<INotifyCollectionChanged>();

            foreach (var observableCollectionItem in observableCollections)
            {
                observableCollectionItem.CollectionChanged += ObservableCollectionChanged;
            }
        }

        /// <summary>
        /// Unsubscribe from the CollectionChanged event for all INotifyCollectionChanged objects contained in item.
        /// </summary>
        /// <param name="item">An object for unsubscribe</param>
        private void UnsubscribeObservableCollectionChanged(object item)
        {
            var observableCollections = item.GetAllChildren<INotifyCollectionChanged>();

            foreach (var observableCollectionItem in observableCollections)
            {
                observableCollectionItem.CollectionChanged -= ObservableCollectionChanged;
            }
        }

        /// <summary>
        /// Handler CollectionChanged event of ObservableCollection. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var newItem in e.NewItems)
                        {
                            SubscribeObservableCollectionChanged(newItem);
                            SetConfigAndSubscribeAllConfigItemBase(newItem);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var oldItem in e.OldItems)
                        {
                            UnsubscribeObservableCollectionChanged(oldItem);
                            ResetConfigAndUnubscribeAllConfigItemBase(oldItem);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        foreach (var oldItem in e.OldItems)
                        {
                            UnsubscribeObservableCollectionChanged(oldItem);
                            ResetConfigAndUnubscribeAllConfigItemBase(oldItem);
                        }

                        foreach (var newItem in e.NewItems)
                        {
                            SubscribeObservableCollectionChanged(newItem);
                            SetConfigAndSubscribeAllConfigItemBase(newItem);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Set Config property and subscribe to CollectionPropertyChanged event for
        /// all ConfigItemBase contained in object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        private void SetConfigAndSubscribeAllConfigItemBase<T>(T obj)
        {
            var configItemBaseCollection = obj.GetAllChildren<ConfigItemBase>();

            foreach (var configItemBase in configItemBaseCollection)
            {
                configItemBase.CollectionPropertyChanged += SubscribeAndUnsubscribeOnPropertyChanged;
                configItemBase.Config = this;
            }
        }

        /// <summary>
        /// Reset Config property and unsubscribe from CollectionPropertyChanged events for
        /// all ConfigItemBase contained in object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        private void ResetConfigAndUnubscribeAllConfigItemBase<T>(T obj)
        {
            var configItemBaseCollection = obj.GetAllChildren<ConfigItemBase>();

            foreach (var configItemBase in configItemBaseCollection)
            {
                configItemBase.CollectionPropertyChanged -= SubscribeAndUnsubscribeOnPropertyChanged;
                configItemBase.Config = null;
            }
        }

        /// <summary>
        /// Compresses a sequence of the ids of the elements contained in the configuration.
        /// </summary>
        /// <exception cref="TemsParser.Models.TooManyItemsInConfigurationException" />
        private void CompressIds()
        {
            var allElements = this.GetAllChildren<ConfigItemBase>();

            if (allElements.Count() <= Int32.MaxValue)
            {
                // Reset _currentId.
                _currentId = 0;

                foreach (var item in allElements)
                {
                    // Reset Config property.
                    // After this, automaticaly assigned to Id property of item next available value.
                    item.Config = null;
                    item.Config = this;
                }
            }
            else // Too many elements in configuration.
            {
                var message = String.Format("Количество объектов в конфигурации превысило {0}.", Int32.MaxValue);
                throw new TooManyItemsInConfigurationException(message);
            }
        }

        /// <summary>
        /// To run after XML-deserialization.
        /// </summary>
        public void OnXmlDeserialized()
        {
            SortAllItemsByName();
            CompressIds();
            RemoveSpaces();
        }

        /// <summary>
        /// Sort all items in configuration by name.
        /// </summary>
        public void SortAllItemsByName()
        {
            var allISortByNameChildren = this.GetAllChildren<ISortByName>();

            foreach (var item in allISortByNameChildren)
            {
                item.SortByName();
            }
        }

        /// <summary>
        /// Sorts list of regions by name.
        /// This is implementation of ISortByName interface.
        /// </summary>
        public void SortByName()
        {
            Regions = new ObservableCollection<RegionModel>(Regions.OrderBy(o => o.ToString()));
        }

        /// <summary>
        /// Assign the next available id of config to the reference result.
        /// If next value is greater than Int32.MaxValue then compress all ids and not assign value to result.
        /// </summary>
        /// <param name="result"> Reference result for assign next available id. </param>
        public void GetId(ref int result)
        {
            if (_currentId < Int32.MaxValue)
            {
                result = ++_currentId;
            }
            else
            {
                CompressIds();
            }
        }

        /// <summary>
        /// Remove spaces in all IRemoveSpaces items contained in configuration.
        /// This is implementation of IRemoveSpaces interface.
        /// </summary>
        public void RemoveSpaces()
        {
            var allIRemoveSpacesChildren = this.GetAllChildren<IRemoveSpaces>();

            foreach (var item in allIRemoveSpacesChildren)
            {
                item.RemoveSpaces();
            }
        }

        public override string ToString()
        {
            return String.Empty;
        }

        #endregion
    }
}
