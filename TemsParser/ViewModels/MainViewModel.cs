using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.IO;
using System.Threading.Tasks;

using TemsParser.Models.Settings;
using TemsParser.Models.Config;
using TemsParser.Behaviors;
using TemsParser.Extentions;
using TemsParser.CustomAttributes;
using TemsParser.Common;
using TemsParser.IO;
using System.Collections.ObjectModel;
using TemsParser.Resources;
using TemsParser.ViewModels.Config;
using TemsParser.Messages;
using System.Threading;

namespace TemsParser.ViewModels
{
    public class MainViewModel : ViewModelBase, IValidationItem
    {
        #region Fields

        public event BooleanPropertyChangedEventHandler IsEnabledChanged;

        private ConfigModel _config;

        private bool _isEnabled;

        private bool _openIsEnabled;

        private IEnumerable<CheckboxViewModel> _operators = new List<CheckboxViewModel>();

        private IEnumerable<CheckboxViewModel> _technologies = new List<CheckboxViewModel>();

        private IEnumerable<FilePathViewModel> _openFiles;

        private int? _binningSizeTextBox;


        #endregion


        #region Constructor

        public MainViewModel(ConfigModel config, SettingsModel settings)
        {
            //OpenCommand = new Command(ex => Open(), ce => IsEnabled);
            OpenCommand = new Command(ex => Open(), ce => IsEnabled);
            EditConfigCommand = new AsyncCommand(ex => EditConfigAsync(), ce => IsEnabled);
            CalculateDistanceCommand = new Command(ex => CalculateDistance());
            ExitCommand = new Command(ex => App.Current.Shutdown());
            ParseCommand = new AsyncCommand(ex => ParseAsync(), ce => ValidateThis());
            Settings = settings;
            Config = config;
            IsEnabled = true;
            OpenIsEnabled = true;
            BinningSizeTextBox = BinningSizeSlider.ToString();

            OpenFiles = Settings.OpenFiles.ToFilePathViewModelCollection();

            Settings.OpenFilesChanged += (s, e) =>
                {
                    OpenFiles = Settings.OpenFiles.ToFilePathViewModelCollection();
                };
        }



        #endregion


        #region Properties

        [IgnoreGetAllChildren]
        public SettingsModel Settings { get; private set; }

        [IgnoreGetAllChildren]
        public ConfigModel Config
        {
            get { return _config; }
            private set
            {
                if (_config != value)
                {
                    _config = value;
                    OnPropertyChanged(() => RegionNames);
                    OnPropertyChanged(() => SelectedRegionName);
                    OnPropertyChanged(() => Technologies);
                    OnPropertyChanged(() => Operators);
                    OnPropertyChanged(() => IsTechnologySelected);
                    OnPropertyChanged(() => IsOperatorSelected);
                    OnPropertyChanged(() => CompareOperatorsCheckBoxVisibility);
                    ClearSettings();
                }
            }
        }

        // Commands
        [IgnoreGetAllChildren]
        public ICommand OpenCommand { get; private set; }

        [IgnoreGetAllChildren]
        public ICommand EditConfigCommand { get; private set; }

        [IgnoreGetAllChildren]
        public ICommand CalculateDistanceCommand { get; private set; }

        [IgnoreGetAllChildren]
        public IAsyncCommand ParseCommand { get; private set; }

        [IgnoreGetAllChildren]
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// List of all files to be parsed.
        /// </summary>
        public IEnumerable<FilePathViewModel> OpenFiles
        {
            get { return _openFiles; }
            private set
            {
                if (_openFiles != value)
                {
                    _openFiles = value;
                    OnPropertyChanged();
                }
            }
        }

        [EqualsTrue(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "NoSelectedFiles")]
        public bool IsOpenedFiles
        {
            get
            {
                return (OpenFiles.Count() > 0);
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                if (_isEnabled != value)
                {
                    var handler = IsEnabledChanged;

                    if (handler != null)
	                {
                        handler(this, new BooleanPropertyChangedEventArgs(_isEnabled, value));
	                }

                    _isEnabled = value;
                    CommandManager.InvalidateRequerySuggested();
                    OnPropertyChanged();
                    OnPropertyChanged(() => CompareOperatorsCheckBoxVisibility);
                }
            }
        }

        public bool OpenIsEnabled
        {
            get { return _openIsEnabled; }
            private set
            {
                if (_openIsEnabled != value)
                {
                    _openIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// List of the all regions name in configuraton.
        /// </summary>
        public IEnumerable<string> RegionNames
        {
            get
            {
                return Config.Regions.Select(o => o.Name);
            }
        }

        /// <summary>
        /// This is a name of the selected region.
        /// </summary>
        [Required]
        public string SelectedRegionName
        {
            get
            {
                return RegionNames.FirstOrDefault(o => (o == Settings.SelectedRegion));
            }
            private set
            {
                if (Settings.SelectedRegion != value)
                {
                    Settings.SelectedRegion = value;
                    OnPropertyChanged();
                    OnPropertyChanged(() => Technologies);
                    OnPropertyChanged(() => Operators);
                    OnPropertyChanged(() => IsTechnologySelected);
                }
            }
        }


        /// <summary>
        /// Selected region.
        /// </summary>
        [IgnoreGetAllChildren]
        public RegionModel SelectedRegion
        {
            get
            {
                var result = Config.Regions.FirstOrDefault(o => (o.Name == Settings.SelectedRegion));

                if (result == null)
                {
                    result = new RegionModel();
                }

                return result;
            }
        }

        /// <summary>
        /// Indicates whether the technology is selected.
        /// </summary>
        [EqualsTrue(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "TechnologyNotSelected")]
        public bool IsTechnologySelected
        {
            get
            {
                if ((SelectedRegionName == null) || (SelectedRegionName == String.Empty))
                {
                    return true;
                }
                else
                {
                    return Technologies.Where(o => (o.IsChecked == true)).Count() != 0;
                }
            }
        }

        /// <summary>
        /// Indicates whether the operator is selected.
        /// </summary>
        [EqualsTrue(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "OperatorNotSelected")]
        public bool IsOperatorSelected
        {
            get
            {
                if ((SelectedRegionName == null) || (SelectedRegionName == String.Empty))
                {
                    return true;
                }
                else
                {
                    return Operators.Where(o => (o.IsChecked == true)).Count() != 0;
                }
            }
        }

        /// <summary>
        /// List of technologies checkboxes.
        /// </summary>
        [IgnoreGetAllChildren]
        public IEnumerable<CheckboxViewModel> Technologies
        {
            get
            {
                foreach (var item in _technologies)
                {
                    IsEnabledChanged -= item.IsEnabledUpdate;
                }

                var result = new List<CheckboxViewModel>();

                var techs = SelectedRegion.Technologies
                    .Where(t => (t.Operators.SelectMany(o => o.Freqs).SelectMany(f => f.Spectrum).Count() != 0))
                    .Select(o => o.Name)
                    .Distinct()
                    .OrderBy(o => o);

                foreach (var techItem in techs)
                {
                    var isChecked = Settings.SelectedTechnologies.Contains(techItem);
                    var checkbox = new CheckboxViewModel(techItem, isChecked);
                    result.Add(checkbox);
                    checkbox.IsCheckedChanged += SelectedTechnologiesUpdate;
                    IsEnabledChanged += checkbox.IsEnabledUpdate;
                }

                return result;
            }
        }

        /// <summary>
        /// List of operators checkboxes.
        /// </summary>
        [IgnoreGetAllChildren]
        public IEnumerable<CheckboxViewModel> Operators
        {
            get
            {
                foreach (var item in _operators)
                {
                    IsEnabledChanged -= item.IsEnabledUpdate;
                }

                var result = new List<CheckboxViewModel>();

                var opers = SelectedRegion.Technologies
                    .SelectMany(o => o.Operators)
                    .Where(o => (o.Freqs.SelectMany(f => f.Spectrum).Count() != 0))
                    .Select(o => o.Name)
                    .Distinct()
                    .OrderBy(o => o);

                foreach (var operItem in opers)
                {
                    var isChecked = Settings.SelectedOperators.Contains(operItem);
                    var checkbox = new CheckboxViewModel(operItem, isChecked);
                    result.Add(checkbox);
                    checkbox.IsCheckedChanged += SelectedOperatorsUpdate;
                    IsEnabledChanged += checkbox.IsEnabledUpdate;
                }

                return result;
            }
        }

        /// <summary>
        /// Binning enabled.
        /// </summary>
        public bool BinningEnabled
        {
            get { return Settings.BinningEnabled; }
            private set
            {
                if (Settings.BinningEnabled != value)
                {
                    Settings.BinningEnabled = value;
                    OnPropertyChanged(); 
                }
            }
        }

        public int BinningSizeMinValue
        {
            get { return SettingsModel.BinningSizeMinValue; }
        }

        public int BinningSizeMaxValue
        {
            get { return SettingsModel.BinningSizeMaxValue; }
        }


        /// <summary>
        /// Binning size slider value.
        /// </summary>
        public int BinningSizeSlider
        {
            get { return Settings.BinningSize; }
            private set
            {
                if (Settings.BinningSize != value)
                {
                    Settings.BinningSize = value;
                    BinningSizeTextBox = value.ToString();
                    OnPropertyChanged(() => BinningSizeTextBox);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Binning size textbox value.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        [Range(SettingsModel.BinningSizeMinValue, SettingsModel.BinningSizeMaxValue,
            ErrorMessage = "Значение должно быть от {1} до {2}.")]
        public string BinningSizeTextBox
        {
            get { return _binningSizeTextBox.ToString(); }
            private set
            {
                if (value == String.Empty)
                {
                    _binningSizeTextBox = null;
                    //Settings.BinningSize = null;
                    OnPropertyChanged();
                    //OnPropertyChanged(() => BinningSizeSlider);
                }
                else
                {
                    int parsedValue;

                    if (Int32.TryParse(value, out parsedValue))
                    {
                        _binningSizeTextBox = parsedValue;
                        
                        if ((parsedValue >= BinningSizeMinValue) && (parsedValue <= BinningSizeMaxValue))
                        {
                            Settings.BinningSize = parsedValue;
                            OnPropertyChanged(() => BinningSizeSlider);
                        }

                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Compare operators checkbox value.
        /// </summary>
        public bool CompareOperatorsEnabled
        {
            get
            {
                return Settings.CompareOperatorsEnabled;
            }
            private set
            {
                if (Settings.CompareOperatorsEnabled != value)
                {
                    Settings.CompareOperatorsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }



        public bool CompareOperatorsCheckBoxVisibility
        {
            get
            {
                var checkedOpers = from oper in Operators
                                   where oper.IsChecked == true
                                   select oper;

                if (checkedOpers.Count() > 1)
                {
                    return (true && IsEnabled);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Define frequency checkbox value.
        /// </summary>
        public bool DefineFreqEnabled
        {
            get
            {
                return Settings.DefineBestFreqEnabled;
            }
            private set
            {
                if (Settings.DefineBestFreqEnabled != value)
                {
                    Settings.DefineBestFreqEnabled = value;
                    OnPropertyChanged();
                }
            }
        }


        #endregion


        #region Methods

        //private void Open()
        //{
        //    IEnumerable<string> openFiles = new List<string>();
        //    bool isOpened = false;

        //    isOpened = FileOperations.OpenFilesTxt(out openFiles);

        //    if (isOpened)
        //    {
        //        Settings.OpenFiles = openFiles;
        //        OnPropertyChanged(() => OpenFiles);
        //        OnPropertyChanged(() => IsOpenedFiles);
        //    }
        //}

        private void Open()
        {
            OpenIsEnabled = false;
            var files = FileDialog.SelectTxtFiles();

            if(files.Count() > 0)
            {
                Settings.OpenFiles = files;
                OnPropertyChanged(() => OpenFiles);
                OnPropertyChanged(() => IsOpenedFiles);
            }

            OpenIsEnabled = true;
        }


        private async Task EditConfigAsync()
        {
            var configViewModel = new ConfigViewModel(Config);
            configViewModel.Title = "Конфигурация";
            Show(configViewModel);

            if (configViewModel.DialogResult == true)
            {
                Config = new ConfigModel(configViewModel.Config);

                // Save config.
                await FileWriter.SerializeConfigAsync(this.Config);
            }
        }

        private void CalculateDistance()
        {
            var vm = new CalcDistanceViewModel();
            vm.Title = "Вычисление расстояния";
            Show(vm, isModal: false);
        }

        private async Task ParseAsync()
        {
            IsEnabled = false;
            var notExistFiles = new List<string>();

            await Task.Run(() =>
                {
                    foreach (var item in OpenFiles)
                    {
                        if (File.Exists(item.Path))
                        {
                            item.IsExist = true;
                        }
                        else
                        {
                            notExistFiles.Add(item.Path);
                            item.IsExist = false;
                        }
                    }
                });

            if (notExistFiles.Count == 0)
            {
                var vm = new ParsingViewModel(Config, Settings);
                vm.Title = "Парсинг";
                Show(vm);
            }
            else
            {
                var message = String.Format("Файл(ы) не найдены:\n{0}", String.Join("\n", notExistFiles));
                Alarms.ShowError("Ошибка чтения файлов", message);
            }

            IsEnabled = true;
        }

        private void SelectedTechnologiesUpdate(object sender, EventArgs e)
        {
            var techCheckbox = sender as CheckboxViewModel;
            
            if (techCheckbox != null)
            {
                if (techCheckbox.IsChecked == true)
                {
                    Settings.SelectedTechnologies.Add(techCheckbox.Name);
                }
                else if (Settings.SelectedTechnologies.Contains(techCheckbox.Name))
                {
                    Settings.SelectedTechnologies.Remove(techCheckbox.Name);
                }
            }
            
            OnPropertyChanged(() => IsTechnologySelected);
        }

        private void SelectedOperatorsUpdate(object sender, EventArgs e)
        {
            var techCheckbox = sender as CheckboxViewModel;

            if (techCheckbox != null)
            {
                if (techCheckbox.IsChecked == true)
                {
                    Settings.SelectedOperators.Add(techCheckbox.Name);
                }
                else if (Settings.SelectedOperators.Contains(techCheckbox.Name))
                {
                    Settings.SelectedOperators.Remove(techCheckbox.Name);
                }
            }

            OnPropertyChanged(() => IsOperatorSelected);
            OnPropertyChanged(() => CompareOperatorsCheckBoxVisibility);
        }


        private void ClearSettings()
        {
            var selectedOperators = Settings.SelectedOperators;
            var clearSelectedOperators = new HashSet<string>();

            foreach (var item in selectedOperators)
            {
                if (Config.OperatorsList.FirstOrDefault(o => (o.Name == item)) != null)
                {
                    clearSelectedOperators.Add(item);
                }
            }

            Settings.SelectedOperators = clearSelectedOperators;
            var selectedTechnologies = Settings.SelectedTechnologies;
            var clearSelectedTechnologies = new HashSet<string>();

            foreach (var item in selectedTechnologies)
            {
                if (Config.TechnologiesList.FirstOrDefault(o => (o.Name == item)) != null)
                {
                    clearSelectedTechnologies.Add(item);
                }
            }

            Settings.SelectedTechnologies = clearSelectedTechnologies;
        }

        private bool ValidateThis()
        {
            bool returns = true;
            var validationItems = this.GetAllChildren<IValidationItem>();

            foreach (var validationItem in validationItems)
            {
                returns = returns && ValidateObject(validationItem); 
            }

            return returns;
        }

        #endregion
    }
}
