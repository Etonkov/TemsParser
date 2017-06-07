using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

using TemsParser.Models.Config;
using TemsParser.Common;
using TemsParser.Extentions;
using TemsParser.Messages;
using TemsParser.CustomAttributes;
using TemsParser.Resources;


namespace TemsParser.ViewModels.Config
{
    public class INameListViewModel : ViewModelBase
    {
        private ObservableCollection<IName> _iNameList;
        private IName _selectedItem;
        private bool _enableDeleteModify;
        private readonly ObjectTypes ItemType;

        public INameListViewModel(ConfigModel config,
                                  IEnumerable<IName> iNameList,
                                  ObjectTypes itemType)
        {
            ItemType = itemType;
            //RenamingDataList = new List<RenamingData>();
            INameList = new ObservableCollection<IName>();

            // deep copieng iNameList and set INameList property.
            switch (itemType)
            {
                case ObjectTypes.RegionListItem:
                    {
                        foreach (var item in iNameList)
                        {
                            INameList.Add(new RegionListItemModel((RegionListItemModel)item,
                                                                   shallowCopyRegions:true));
                        }
                    }
                    break;
                case ObjectTypes.OperatorListItem:
                    {
                        foreach (var item in iNameList)
                        {
                            INameList.Add(new OperatorListItemModel((OperatorListItemModel)item,
                                                                     shallowCopyOperators:true));
                        }
                    }
                    break;
                case ObjectTypes.TechnologyListItem:
                    {
                        foreach (var item in iNameList)
                        {
                            INameList.Add(new TechnologyListItemModel((TechnologyListItemModel)item,
                                                                       shallowCopyTechnologies:true));
                        }
                    }
                    break;
                default:
                    {
                        string message = String.Format("Invalid itemType:{0}. {1} cannot use this itemType.",
                                                        itemType,
                                                        this.GetType().Name);

                        throw new ArgumentException(message, "itemType");
                    }
            }

            Config = config;

            AddCommand = new Command(ex => Add());
            DeleteCommand = new Command(ex => Delete(), ce => _enableDeleteModify);
            ModifyCommand = new Command(ex => Modify(), ce => _enableDeleteModify);
            CancelCommand = new Command(ex => Close());

            OkCommand = new Command(
                ex =>
                {
                    DialogResult = true;
                    Apply();
                    Close();
                },
                ce => ValidateObject(this));
        }


        public ConfigModel Config { get; private set; }
        public ObjectTypes ObjType { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ModifyCommand { get; private set; }


        [UniqueValuesCollection]
        public ObservableCollection<IName> INameList
        {
            get { return _iNameList; }
            private set { _iNameList = value; OnPropertyChanged(); }
        }

        public IName SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;

                    if (_selectedItem == null)
                    {
                        _enableDeleteModify = false;
                    }
                    else
                    {
                        _enableDeleteModify = true;
                    }

                    OnPropertyChanged();
                }
            }
        }

        //public List<RenamingData> RenamingDataList { get; private set; }


        private void Add()
        {
            switch (ItemType)
            {
                case ObjectTypes.TechnologyListItem:
                    {
                        var techsList = new ObservableCollection<TechnologyListItemModel>();

                        foreach (var item in INameList)
                        {
                            techsList.Add((TechnologyListItemModel)item);
                        }

                        TechnologyListItemViewModel vm;
                        vm = new TechnologyListItemViewModel(techsList);
                        vm.Title = "Элемент списка технологий";
                        Show(vm);

                        if (vm.DialogResult == true)
                        {
                            INameList = new ObservableCollection<IName>(vm.TechnologiesList);
                            SelectedItem = vm.TechnologyListItem;
                        }
                    }
                    break;
                default:
                    {
                        INameListItemViewModel vm;
                        vm = new INameListItemViewModel(INameList, ItemType);

                        switch (ItemType)
                        {
                            case ObjectTypes.RegionListItem:
                                {
                                    vm.Title = "Элемент списка регионов";
                                }
                                break;
                            case ObjectTypes.OperatorListItem:
                                {
                                    vm.Title = "Элемент списка операторов";
                                }
                                break;
                        }

                        Show(vm);

                        if (vm.DialogResult == true)
                        {
                            INameList = vm.INameList;
                            SelectedItem = vm.INameListItem;
                        }
                    }
                    break;
            }
        }

        private void Modify()
        {
            switch (ItemType)
            {
                case ObjectTypes.TechnologyListItem:
                    {
                        var techsList = new ObservableCollection<TechnologyListItemModel>();

                        foreach (var item in INameList)
                        {
                            techsList.Add((TechnologyListItemModel)item);
                        }

                        TechnologyListItemViewModel vm;
                        vm = new TechnologyListItemViewModel(techsList, (TechnologyListItemModel)SelectedItem);
                        vm.Title = "Элемент списка технологий";
                        Show(vm);

                        if (vm.DialogResult == true)
                        {
                            INameList = new ObservableCollection<IName>(vm.TechnologiesList);
                            SelectedItem = vm.TechnologyListItem;
                        }
                    }
                    break;
                default:
                    {
                        INameListItemViewModel vm;
                        vm = new INameListItemViewModel(INameList, SelectedItem, ItemType);

                        switch (ItemType)
                        {
                            case ObjectTypes.RegionListItem:
                                {
                                    vm.Title = "Элемент списка регионов";
                                }
                                break;
                            case ObjectTypes.OperatorListItem:
                                {
                                    vm.Title = "Элемент списка операторов";
                                }
                                break;
                        }

                        Show(vm);

                        if (vm.DialogResult == true)
                        {
                            INameList = vm.INameList;
                            SelectedItem = vm.INameListItem;
                        }
                    }
                    break;
            }
        }

        private void Delete()
        {
            bool canDelete = false;

            switch (ItemType)
            {
                case ObjectTypes.RegionListItem:
                    {
                        if (((RegionListItemModel)SelectedItem).Regions.Count() != 0)
                        {
                            string message = "Невозможно удалить регион "+ SelectedItem +
                                " из списка, так как он содержится в конфигурации.";
                            Alarms.ShowError("Ошибка удаления", message);
                        }
                        else
                        {
                            canDelete = true;
                        }
                    }
                    break;
                case ObjectTypes.TechnologyListItem:
                    {
                        if (((TechnologyListItemModel)SelectedItem).Technologies.Count() != 0)
                        {
                            string message = "Невозможно удалить технологию " + SelectedItem +
                                " из списка, так как она содержится в конфигурации.";
                            Alarms.ShowError("Ошибка удаления", message);
                        }
                        else
                        {
                            canDelete = true;
                        }
                    }
                    break;
                case ObjectTypes.OperatorListItem:
                    {
                        if (((OperatorListItemModel)SelectedItem).Operators.Count() != 0)
                        {
                            string message = "Невозможно удалить оператора " + SelectedItem +
                                " из списка, так как он содержится в конфигурации.";
                            Alarms.ShowError("Ошибка удаления", message);
                        }
                        else
                        {
                            canDelete = true;
                        }
                    }
                    break;
            }

            if (canDelete)
            {
                INameList.Remove(SelectedItem);
                OnPropertyChanged(() => INameList);
            }
        }

        private void Apply()
        {
            switch (ItemType)
            {
                case ObjectTypes.RegionListItem:
                    {
                        var regionsList = new ObservableCollection<RegionListItemModel>();

                        foreach (var iNameListItem in INameList)
	                    {
                            regionsList.Add((RegionListItemModel)iNameListItem);
                        }

                        // Update all region models.
                        foreach (var regionsListItem in regionsList)
                        {
                            foreach (var regionItem in regionsListItem.Regions)
                            {
                                regionItem.RegionListItem = regionsListItem;
                            }
                        }

                        Config.RegionsList = regionsList;
                    }
                    break;
                case ObjectTypes.OperatorListItem:
                    {
                        var operatorList = new ObservableCollection<OperatorListItemModel>();

                        foreach (var iNameListItem in INameList)
                        {
                            operatorList.Add((OperatorListItemModel)iNameListItem);
                        }

                        // Update all operator models.
                        foreach (var operatorListItem in operatorList)
                        {
                            foreach (var operatorItem in operatorListItem.Operators)
                            {
                                operatorItem.OperatorListItem = operatorListItem;
                            }
                        }

                        Config.OperatorsList = operatorList;
                    }
                    break;
                case ObjectTypes.TechnologyListItem:
                    {
                        var technologyList = new ObservableCollection<TechnologyListItemModel>();

                        foreach (var iNameListItem in INameList)
                        {
                            technologyList.Add((TechnologyListItemModel)iNameListItem);
                        }

                        // Update all technology models.
                        foreach (var technologyListItem in technologyList)
                        {
                            foreach (var technologyItem in technologyListItem.Technologies)
                            {
                                technologyItem.TechnologyListItem = technologyListItem;
                            }
                        }

                        Config.TechnologiesList = technologyList;
                    }
                    break;
            }
            
        }
    }
}