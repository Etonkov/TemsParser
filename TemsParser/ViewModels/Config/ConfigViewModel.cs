using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.ComponentModel;

using TemsParser.Models.Config;
using TemsParser.Behaviors;
using TemsParser.Extentions;
using TemsParser.IO;
using TemsParser.Messages;
using TemsParser.Common;
using System.Threading.Tasks;
using TemsParser.Extentions.Model.Config;
using TemsParser.Extentions.ViewModel;

namespace TemsParser.ViewModels.Config
{
    public class ConfigViewModel : ViewModelBase
    {
        #region Fields

        // For on/off edit buttons.
        private bool _editEnabled;

        // For enabled/disabled all comands.
        private bool _isEnabled = true;


        // Dictionary for name button.
        private static readonly Dictionary<ObjectTypes, string> AddChildrenCommandNames = new Dictionary<ObjectTypes, string>()
        {
            {ObjectTypes.Region, "Добавить технологию"},
            {ObjectTypes.Technology, "Добавить оператора"},
            {ObjectTypes.Operator, "Добавить частоты"},
            {ObjectTypes.Freq, "Добавить частоты"}
        };

        // Dictionary for name button.
        private static readonly Dictionary<ObjectTypes, ObjectTypes> AddChildrenTypes =
            new Dictionary<ObjectTypes, ObjectTypes>()
            {
                {ObjectTypes.Region, ObjectTypes.Technology},
                {ObjectTypes.Technology, ObjectTypes.Operator},
                {ObjectTypes.Operator, ObjectTypes.Freq},
                {ObjectTypes.Freq, ObjectTypes.Freq}
            };

        // Backing feelds of properties.
        private ObservableCollection<TreeViewItemViewModel> _treeViewTopItems;
        private ConfigModel _config;

        // Support PropertyChanged("isSelected") for TreeViewSelectedItem property.
        private PropertyChangedEventHandler _propertyChangedHandler;
        private NotifyCollectionChangedEventHandler _collectionChangedHandler;

        #endregion


        #region Constructors

        public ConfigViewModel(ConfigModel config)
        {
            // Ok/Cancel Commands.
            CancelCommand = new Command(ex => Close());

            OkCommand = new Command(ex =>
            {
                DialogResult = true;
                Close();
            },
            ce => _isEnabled);

            // Expand all and collapse all treeview buttons commands.
            ExpandAllCommand = new Command(ex => ExpandAll(), ce => IsEnabled);
            CollapseAllCommand = new Command(ex => CollapseAll(), ce => IsEnabled);

            // Import and export config buttons commands.
            ExportConfigCommand = new AsyncCommand(ex => ExportConfigAsync(), ce => IsEnabled);
            ImportConfigCommand = new AsyncCommand(ex => ImportConfigAsync(), ce => IsEnabled);

            // Edit config buttons commands.
            AddRegionCommand = new Command(ex => AddRegion(), ce => IsEnabled);
            AddChildrenCommand = new Command(ex => AddChildren(), ce => (EditEnabled && IsEnabled));
            ModifySelectedCommand = new Command(ex => ModifySelected(), ce => (EditEnabled && IsEnabled));
            DeleteSelectedCommand = new Command(ex => DeleteSelected(), ce => (EditEnabled && IsEnabled));
            
            // Support PropertyChanged("isSelected") for TreeViewSelectedItem property.
            _propertyChangedHandler = new PropertyChangedEventHandler(ItemPropertyChanged);
            _collectionChangedHandler = new NotifyCollectionChangedEventHandler(ItemsCollectionChanged);

            // Copy config.
            Config = new ConfigModel(config);
        }

        #endregion


        #region Properties

        // Expand all and collapse all treeview buttons commands.
        public ICommand ExpandAllCommand { get; private set; }
        public ICommand CollapseAllCommand { get; private set; }

        // Import and export config buttons commands.
        public ICommand ExportConfigCommand { get; private set; }
        public ICommand ImportConfigCommand { get; private set; }

        // Edit config buttons commands.
        public ICommand AddRegionCommand { get; private set; }
        public ICommand AddChildrenCommand { get; private set; }
        public ICommand ModifySelectedCommand { get; private set; }
        public ICommand DeleteSelectedCommand { get; private set; }

        // Config data
        public ConfigModel Config
        {
            get { return _config; }
            private set
            {
                if (value != _config)
                {
                    _config = value;

                    // TreeViewTopItems update.
                    TreeViewTopItems = _config.Regions.ToTreeViewTopItems();
                }
            }
        }

        // TreeView view model data
        public ObservableCollection<TreeViewItemViewModel> TreeViewTopItems
        {
            get { return _treeViewTopItems; }
            private set
            {
                if (value != _treeViewTopItems)
                {
                    _treeViewTopItems = value;

                    // Support PropertyChanged("isSelected") for TreeViewSelectedItem property
                    TreeViewTopItems.CollectionChanged += _collectionChangedHandler;

                    foreach (var treeViewitem in TreeViewTopItems.Traverse(o => o.Children))
                    {
                        treeViewitem.PropertyChanged += _propertyChangedHandler;
                        treeViewitem.Children.CollectionChanged += _collectionChangedHandler;
                    }

                    OnPropertyChanged();
                }
            }
        }

        // TreeView selected item
        public TreeViewItemViewModel TreeViewSelectedItem
        {
            get
            {
                return TreeViewTopItems
                           .Traverse(o => o.Children)
                           .FirstOrDefault(o => o.IsSelected);
            }
        }

        // Buttons name
        public string AddButtonTitle
        {
            get
            {
                if (TreeViewSelectedItem == null)
                {
                    EditEnabled = false;
                    return "Добавить...";
                }
                else
                {
                    EditEnabled = true;
                    return AddChildrenCommandNames[TreeViewSelectedItem.ObjectType];
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool EditEnabled
        {
            get { return _editEnabled; }
            private set
            {
                if (_editEnabled != value)
                {
                    _editEnabled = value;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        #endregion


        #region Methods

        // Commands methods
        private void ExpandAll()
        {
            var coll = TreeViewTopItems.Traverse(o => o.Children);

            foreach (var treeViewItem in coll)
	        {
                treeViewItem.IsExpanded = true;
	        }
        }

        private void CollapseAll()
        {
            foreach (var treeViewItem in TreeViewTopItems.Traverse(o => o.Children))
            {
                treeViewItem.IsExpanded = false;
            }
        }

        private async Task ExportConfigAsync()
        {
            IsEnabled = false;
            await FileDialog.ExportConfigXmlAsync(Config);
            IsEnabled = true;
        }

        private async Task ImportConfigAsync()
        {
            IsEnabled = false;

            var importConfigData = await FileDialog.ImportConfigXmlAsync();

            if (importConfigData.Result)
            {
                Config = importConfigData.Config;
            };

            IsEnabled = true;
        }

        private void AddRegion()
        {
            var vm = new INameItemViewModel(Config, ObjectTypes.Region);

            vm.Title = "Добавить регион";
            Show(vm);

            if (vm.DialogResult == true)
            {
                TreeViewTopItems.FocusOn(((ConfigItemBase)(vm.INameItem)).Id);
            }
        }

        private void AddChildren()
        {

            var childrenType = AddChildrenTypes[TreeViewSelectedItem.ObjectType];
            if (childrenType == ObjectTypes.Freq)
            {
                FreqViewModel vm;

                if (TreeViewSelectedItem.ObjectType == ObjectTypes.Freq)
                {
                    vm = new FreqViewModel(Config, TreeViewSelectedItem.Parent.Id);
                }
                else
                {
                    vm = new FreqViewModel(Config, TreeViewSelectedItem.Id);
                }

                vm.Title = AddChildrenCommandNames[TreeViewSelectedItem.ObjectType];
                Show(vm);

                if (vm.DialogResult == true)
                {
                    TreeViewTopItems.FocusOn(vm.Freq.Id);
                }
            }
            else
            {
                var vm = new INameItemViewModel(Config, childrenType, TreeViewSelectedItem.Id);

                vm.Title = AddChildrenCommandNames[TreeViewSelectedItem.ObjectType];
                Show(vm);

                if (vm.DialogResult == true)
                {
                    TreeViewTopItems.FocusOn(vm.ItemId);
                }
            }

        }

        private void DeleteSelected()
        {
            var message = String.Format("Следующий элемент будет удален из конфигурации:\n{0} \"{1}\"\n\nПродолжить?",
                                        TreeViewSelectedItem.TypeToDisplay,
                                        TreeViewSelectedItem.Name);

            if (Alarms.ShowQuestion("Удаление элемента", message))
            {
                var elementId = TreeViewSelectedItem.Id;
                int parentId;

                if (TreeViewSelectedItem.Parent != null)
                {
                    parentId = TreeViewSelectedItem.Parent.Id;
                }
                else
                {
                    parentId = 0;
                }

                switch (TreeViewSelectedItem.ObjectType)
                {
                    case ObjectTypes.Region:
                        {
                            Config.Regions.Remove(Config.Regions.First(o => o.Id == elementId));
                        }
                        break;
                    case ObjectTypes.Operator:
                        {
                            var parent = Config.Regions.
                                           SelectMany(o => o.Technologies).
                                           First(o => o.Id == parentId);

                            var element = Config.Regions.
                                              SelectMany(o => o.Technologies).
                                              SelectMany(o => o.Operators).
                                              First(o => o.Id == elementId);

                            parent.Operators.Remove(element);
                        }
                        break;
                    case ObjectTypes.Technology:
                        {
                            var parent = Config.Regions.First(o => o.Id == parentId);

                            var element = Config.Regions.
                                              SelectMany(o => o.Technologies).
                                              First(o => o.Id == elementId);

                            parent.Technologies.Remove(element);
                        }
                        break;
                    case ObjectTypes.Freq:
                        {
                            var parent = Config.Regions.
                                             SelectMany(o => o.Technologies).
                                             SelectMany(o => o.Operators).
                                             First(o => o.Id == parentId);

                            var element = Config.Regions.
                                             SelectMany(o => o.Technologies).
                                             SelectMany(o => o.Operators).
                                             SelectMany(o => o.Freqs).
                                             First(o => o.Id == elementId);

                            parent.Freqs.Remove(element);
                        }
                        break;
                }
            }
        }

        private void ModifySelected()
        {
            if (TreeViewSelectedItem.ObjectType == ObjectTypes.Freq)
            {
                var vm = new FreqViewModel(Config, TreeViewSelectedItem.Parent.Id, TreeViewSelectedItem.Id);
                vm.Title = AddChildrenCommandNames[TreeViewSelectedItem.ObjectType];
                Show(vm);

                if (vm.DialogResult)
                {
                    TreeViewTopItems.FocusOn(vm.Freq.Id);
                }
            }
            else
            {
                int parentId = 0;

                if (TreeViewSelectedItem.Parent != null)
                {
                    parentId = TreeViewSelectedItem.Parent.Id;
                }

                var vm = new INameItemViewModel(Config,
                                                TreeViewSelectedItem.ObjectType,
                                                parentId,
                                                TreeViewSelectedItem.Id);

                vm.Title = TreeViewSelectedItem.TypeToDisplay;
                Show(vm);

                if (vm.DialogResult)
                {
                    TreeViewTopItems.FocusOn(vm.ItemId);
                }
            }
        }

        // add PropertyChanged("isSelected") logic for TreeViewSelectedItem property
        private void SubscribePropertyChanged(TreeViewItemViewModel item)
        {
            item.PropertyChanged += _propertyChangedHandler;
            item.Children.CollectionChanged += _collectionChangedHandler;

            foreach (var subItem in item.Children)
            {
                SubscribePropertyChanged(subItem);
            }
        }

        private void UnsubscribePropertyChanged(TreeViewItemViewModel item)
        {
            item.Children.CollectionChanged -= _collectionChangedHandler;
            item.PropertyChanged -= _propertyChangedHandler;

            foreach (var subItem in item.Children)
            {
                UnsubscribePropertyChanged(subItem);
            }
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TreeViewItemViewModel oldItem in e.OldItems)
                {
                    UnsubscribePropertyChanged(oldItem);
                }
            }

            if (e.NewItems != null)
            {
                foreach (TreeViewItemViewModel newItem in e.NewItems)
                {
                    SubscribePropertyChanged(newItem);
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                OnPropertyChanged(() => TreeViewSelectedItem);
                OnPropertyChanged(() => AddButtonTitle);
            }
        }

        #endregion
    }
}
