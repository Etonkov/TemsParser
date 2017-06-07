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
using TemsParser.Extentions.Model.Config;

namespace TemsParser.ViewModels.Config
{
    public class INameItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Data store for SelectedValue property.
        /// </summary>
        private string _selectedValue;

        //private readonly int ItemId;

        /// <summary>
        /// Represents the configuration item type. The field is readonly.
        /// </summary>
        private readonly ObjectTypes ItemType;

        /// <summary>
        /// Represents the parent item id. The field is readonly.
        /// </summary>
        private readonly int ParentItemId;

        /// <summary>
        /// Represents the item that is been edited in this view model. The field is readonly.
        /// </summary>
        public readonly IName INameItem;

        /// <summary>
        /// Represents the configuration. The field is readonly.
        /// </summary>
        private readonly ConfigModel Config;

        /// <summary>
        /// Represents the Boolean value that indicates whether a new item is created or not.
        /// True if creating; otherwise, false.
        /// </summary>
        private readonly bool IsNewItem;

        /// <summary>
        /// Represents the names of list for window title and button name. The field is readonly.
        /// </summary>
        private readonly Dictionary<ObjectTypes, string> ListNameTitles = new Dictionary<ObjectTypes, string>()
        {
            {ObjectTypes.Region, "Список регионов"},
            {ObjectTypes.Technology, "Список технологий"},
            {ObjectTypes.Operator, "Список операторов"}
        };

        /// <summary>
        /// Initializes a new instance of the ConfigItemViewModel class. Used to add and modify config item.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="itemId"></param>
        /// <param name="parentItemId"></param>
        /// <param name="itemType"></param>
        public INameItemViewModel(ConfigModel config, ObjectTypes itemType, int parentItemId = 0, int itemId = 0)
        {
            // Define the readonly fields.
            ParentItemId = parentItemId;
            Config = config;
            ItemType = itemType;

            // if iNameItemId == 0 then view model use for create the new item.
            if (itemId == 0)
            {
                IsNewItem = true;
            }



            // Define the INameItem.
            if (IsNewItem)
            {
                switch (ItemType)
                {

                    case ObjectTypes.Region:
                        {
                            INameItem = new RegionModel();
                        }
                        break;
                    case ObjectTypes.Technology:
                        {
                            INameItem = new TechnologyModel();
                        }
                        break;
                    case ObjectTypes.Operator:
                        {
                            INameItem = new OperatorModel();
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
            }
            else
            {
                INameItem = (IName)(Config.GetItem(itemId));
            }


            SelectedValue = INameItem.Name;

            // Commands.
            ModifyListNamesCommand = new Command(ex => ModifyListNames());
            CancelCommand = new Command(ex => Close());

            OkCommand = new Command(
                ex =>
                {
                    DialogResult = true;
                    Apply();
                    Close();
                },
                ce =>
                {
                    return ValidateObject(this);
                });
        }


        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            private set
            {
                if (_selectedValue != value)
                {
                    _selectedValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ItemId
        {
            get { return ((ConfigItemBase)INameItem).Id; }
        }

        public ObservableCollection<string> AwailableValues
        {
            get
            {
                // This values must be excluded from the list names(not displayed).
                IEnumerable<string> busyValues = null;

                // This is the all values.
                IEnumerable<string> allValues = null;

                // Define the busyValues and allValues.
                switch (ItemType)
                {
                    case ObjectTypes.Region:
                        {
                            busyValues = Config.Regions
                                             .Where(o => (o != INameItem))
                                             .Select(o => o.Name);

                            allValues = Config.RegionsList.Select(o => o.Name);
                        }
                        break;
                    case ObjectTypes.Technology:
                        {
                            busyValues = Config.Regions
                                             .First(o => (o.Id == ParentItemId))
                                             .Technologies
                                             .Where(o => (o != INameItem))
                                             .Select(o => o.Name);

                            allValues = Config.TechnologiesList.Select(o => o.Name);
                        }
                        break;
                    case ObjectTypes.Operator:
                        {
                            busyValues = Config.Regions
                                             .SelectMany(o => o.Technologies)
                                             .First(o => (o.Id == ParentItemId))
                                             .Operators
                                             .Where(o => (o != INameItem))
                                             .Select(o => o.Name);

                            allValues = Config.OperatorsList.Select(o => o.Name);
                        }
                        break;
                }

                var result = new ObservableCollection<string>();


                foreach (var allValuesItem in allValues)
                {
                    // if value from all values is not contain in busyValues then add to result.
                    if (busyValues.Contains(allValuesItem) == false)
                    {
                        result.Add(allValuesItem);
                    }
                }

                return result;
            }
        }

        public string ListNameTitle
        {
            get { return ListNameTitles[ItemType]; } 
        }

        public ICommand ModifyListNamesCommand { get; private set; }


        private void ModifyListNames()
        {

            ViewModelBase vm = null;

            switch (ItemType)
            {
                case ObjectTypes.Region:
                    vm = new INameListViewModel(Config, Config.RegionsList, ObjectTypes.RegionListItem);
                    break;
                case ObjectTypes.Technology:
                    vm = new INameListViewModel(Config, Config.TechnologiesList, ObjectTypes.TechnologyListItem);
                    break;
                case ObjectTypes.Operator:
                    vm = new INameListViewModel(Config, Config.OperatorsList, ObjectTypes.OperatorListItem);
                    break;
            }

            vm.Title = ListNameTitle;
            this.Close();
            Show(vm);

            if (vm.DialogResult == true)
            {
                OnPropertyChanged(() => AwailableValues);

                if (IsNewItem)
                {
                    SelectedValue = null;
                }
                else
                {
                    SelectedValue = INameItem.Name;
                }
            }

            this.Show(this);
        }

        private void Apply()
        {
            switch (ItemType)
            {
                case ObjectTypes.Region:
                    {
                        var reg = (RegionModel)INameItem;
                        reg.Name = SelectedValue;

                        if (IsNewItem)
                        {
                            Config.Regions.Add(reg);
                        }
                    }
                    break;
                case ObjectTypes.Technology:
                    {
                        var tech = (TechnologyModel)INameItem;
                        tech.Name = SelectedValue;

                        if (IsNewItem)
                        {
                            var parent = Config.Regions.First(o => (o.Id == ParentItemId));
                            parent.Technologies.Add(tech);
                        }
                    }
                    break;
                case ObjectTypes.Operator:
                    {
                        var oper = (OperatorModel)INameItem;
                        oper.Name = SelectedValue;

                        if (IsNewItem)
                        {
                            var parentTech = Config.Regions
                                                 .SelectMany(o => o.Technologies)
                                                 .First(o => (o.Id == ParentItemId));

                            parentTech.Operators.Add(oper);
                        }
                    }
                    break;
            }
        }
    }
}