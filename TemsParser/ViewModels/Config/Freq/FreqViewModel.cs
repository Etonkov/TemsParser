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
//using Microsoft.TeamFoundation.MVVM;


namespace TemsParser.ViewModels.Config
{
    public class FreqViewModel : ViewModelBase
    {
        /// <summary>
        /// Initialize a new instance of FreqViewModel class.
        /// Used to create new or modify existing FreqModel objects.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="parentId">Id of parent operator.</param>
        /// <param name="freqId">Id of FreqModel.</param>
        public FreqViewModel(ConfigModel config, int parentId, int freqId = 0)
        {
            // If this view model used for modify existing FreqModel object,
            // then this index used for replace original freq on OK command.
            int indexOfFreq = -1;

            // Find parent operator in config;
            ParentOperator = config.Regions
                                 .SelectMany(o => o.Technologies)
                                 .SelectMany(o => o.Operators)
                                 .First(o => (o.Id == parentId));

            // If FreqId = 0 then use view model to create a new FreqModel.
            if (freqId == 0)
            {
                Freq = new FreqModel();
            }
            else // Otherwise, use view model to modify a freq.
            {
                // Find freq in config;
                var freq = config.Regions
                               .SelectMany(o => o.Technologies)
                               .SelectMany(o => o.Operators)
                               .SelectMany(o => o.Freqs)
                               .First(o => (o.Id == freqId));

                // Deep copy FreqModel.
                Freq = new FreqModel(freq);

                // Define the index used to replase origanal freq on OK command.
                indexOfFreq = ParentOperator.Freqs.IndexOf(freq);
            }

            // Define commands.
            OkCommand = new Command(
                execute =>
                {
                    DialogResult = true;
                    Freq.Config = config;


                    if (freqId == 0)
                    {
                        ParentOperator.Freqs.Add(Freq);
                    }
                    else
                    {
                        ParentOperator.Freqs[indexOfFreq] = Freq;
                    }

                    Close();
                },
                canExecute =>
                {
                    return ValidateObject(this);
                });

            CancelCommand = new Command(ex => Close());
        }


        public FreqModel Freq { get; private set; }

        public OperatorModel ParentOperator { get; private set; }

        [FreqSpectrum]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string FreqSpectrum
        {
            get { return Freq.Spectrum; }
            set
            {
                if (Freq.Spectrum != value)
                {
                    Freq.Spectrum = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}