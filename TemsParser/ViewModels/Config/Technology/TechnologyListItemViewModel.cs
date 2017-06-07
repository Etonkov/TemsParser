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
    public class TechnologyListItemViewModel : ViewModelBase
    {
        private ObservableCollection<TechnologyListItemModel> _technologiesList;

        /// <summary>
        /// Common constructor.
        /// </summary>
        private TechnologyListItemViewModel()
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
        /// <param name="config"></param>
        /// <param name="technologiesList"></param>
        /// <param name="technologyListItem"></param>
        public TechnologyListItemViewModel(ObservableCollection<TechnologyListItemModel> technologiesList,
                                           TechnologyListItemModel technologyListItem)
            : this()
        {
            var index = technologiesList.IndexOf(technologyListItem);
            TechnologiesList = technologiesList.Clone(shallowCopyTechnologies: true);
            TechnologyListItem = TechnologiesList[index];
        }


        /// <summary>
        /// Initializes a new instance of the TechnologiesListItemViewModel class.
        /// Used to add a new TechnologiesListItem to the TechnologiesList.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="technologiesList"></param>
        public TechnologyListItemViewModel(ObservableCollection<TechnologyListItemModel> technologiesList) : this()
        {
            TechnologiesList = technologiesList.Clone(shallowCopyTechnologies: true);
            TechnologyListItem = new TechnologyListItemModel();
            TechnologiesList.Add(TechnologyListItem);
        }


        [UniqueValuesCollection(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "ElementAlreadyContained")]
        public ObservableCollection<TechnologyListItemModel> TechnologiesList
        {
            get { return _technologiesList; }
            private set { _technologiesList = value; OnPropertyChanged(); }
        }


        public TechnologyListItemModel TechnologyListItem { get; private set; }


        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string Name
        {
            get { return TechnologyListItem.Name; }
            private set
            {
                if (TechnologyListItem.Name != value)
                {
                    TechnologyListItem.Name = value;
                    OnPropertyChanged();
                    OnPropertyChanged(() => TechnologiesList);
                }
            }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string LatitudeColumnName
        {
            get { return TechnologyListItem.LatitudeColumnName; }
            private set
            {
                if (TechnologyListItem.LatitudeColumnName != value)
                {
                    TechnologyListItem.LatitudeColumnName = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string LongitudeColumnName
        {
            get { return TechnologyListItem.LongitudeColumnName; }
            private set
            {
                if (TechnologyListItem.LongitudeColumnName != value)
                {
                    TechnologyListItem.LongitudeColumnName = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string FreqColumnNamePart
        {
            get { return TechnologyListItem.FreqColumnNamePart; }
            private set
            {
                if (TechnologyListItem.FreqColumnNamePart != value)
                {
                    TechnologyListItem.FreqColumnNamePart = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string LevelColumnNamePart
        {
            get { return TechnologyListItem.LevelColumnNamePart; }
            private set
            {
                if (TechnologyListItem.LevelColumnNamePart != value)
                {
                    TechnologyListItem.LevelColumnNamePart = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}