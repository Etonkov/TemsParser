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
    public class INameListItemViewModel : ViewModelBase
    {
        //private readonly ObjectTypes ItemType;
        //private readonly int Index;
        private ObservableCollection<IName> _iNameList;

        /// <summary>
        /// Common constructor.
        /// </summary>
        private INameListItemViewModel()
        {
            OkCommand = new Command(
                ex =>
                {
                    DialogResult = true;
                    Close();
                },
                ce => ValidateObject(this));

            CancelCommand = new Command(ex => Close());
        }

        /// <summary>
        /// Initializes a new instance of the TechnologiesListItemViewModel class.
        /// Used to modify TechnologiesListItem in the TechnologiesList.
        /// </summary>
        /// <param name="iNameList"></param>
        /// <param name="iNameListItem"></param>
        /// <param name="itemType"></param>
        public INameListItemViewModel(ObservableCollection<IName> iNameList, IName iNameListItem, ObjectTypes itemType)
            : this()
        {
            var index = iNameList.IndexOf(iNameListItem);

            switch (itemType)
            {
                case ObjectTypes.RegionListItem:
                    {
                        INameList = new ObservableCollection<IName>();

                        foreach (var item in iNameList)
                        {
                            INameList.Add(new RegionListItemModel((RegionListItemModel)item,
                                                                   shallowCopyRegions: true));
                        }
                    }
                    break;
                case ObjectTypes.OperatorListItem:
                    {
                        INameList = new ObservableCollection<IName>();

                        foreach (var item in iNameList)
                        {
                            INameList.Add(new OperatorListItemModel((OperatorListItemModel)item,
                                                                     shallowCopyOperators: true));
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

            INameListItem = INameList[index];
        }


        /// <summary>
        /// Initializes a new instance of the TechnologiesListItemViewModel class.
        /// Used to add new TechnologiesListItem in the TechnologiesList.
        /// </summary>
        /// <param name="iNameList"></param>
        /// <param name="itemType"></param>
        public INameListItemViewModel(ObservableCollection<IName> iNameList, ObjectTypes itemType)
            : this()
        {
            switch (itemType)
            {
                case ObjectTypes.RegionListItem:
                    {
                        INameList = new ObservableCollection<IName>();

                        foreach (var item in iNameList)
                        {
                            INameList.Add(new RegionListItemModel((RegionListItemModel)item,
                                                                   shallowCopyRegions: true));
                        }

                        INameListItem = new RegionListItemModel();
                        INameList.Add(INameListItem);
                    }
                    break;
                case ObjectTypes.OperatorListItem:
                    {
                        INameList = new ObservableCollection<IName>();

                        foreach (var item in iNameList)
                        {
                            INameList.Add(new OperatorListItemModel((OperatorListItemModel)item,
                                                                     shallowCopyOperators: true));
                        }

                        INameListItem = new OperatorListItemModel();
                        INameList.Add(INameListItem);
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

        [UniqueValuesCollection(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "ElementAlreadyContained")]
        public ObservableCollection<IName> INameList
        {
            get { return _iNameList; }
            private set
            {
                if (_iNameList != value)
                {
                    _iNameList = value;
                    OnPropertyChanged();
                }
            }
        }

        public IName INameListItem { get; private set; }

        [Names(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Names")]
        [MaxLengthTrimmed(16, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "MaxLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string Name
        {
            get { return INameListItem.Name; }
            private set
            {
                if (INameListItem.Name != value)
                {
                    INameListItem.Name = value;
                    OnPropertyChanged();
                    OnPropertyChanged(() => INameList);
                }
            }
        }
    }
}