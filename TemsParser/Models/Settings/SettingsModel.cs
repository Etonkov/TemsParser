using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using TemsParser.Common;
using TemsParser.Models.Config;

namespace TemsParser.Models.Settings
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SettingsModel : IValidationItem
    {
        [field: NonSerialized()]
        public event EventHandler OpenFilesChanged;

        public const int BinningSizeMinValue = 10;
        public const int BinningSizeMaxValue = 500;
        private IEnumerable<string> _openFiles;


        public SettingsModel()
        {
            BinningEnabled = true;
            CompareOperatorsEnabled = true;
            DefineBestFreqEnabled = false;
            OpenFiles = new List<string>();
            SelectedTechnologies = new HashSet<string>();
            SelectedOperators = new HashSet<string>();
            BinningSize = 50;
        }

        [Range(BinningSizeMinValue, BinningSizeMaxValue)]
        public int BinningSize { get; set; }

        public bool BinningEnabled { get; set; }

        public bool CompareOperatorsEnabled { get; set; }

        public bool DefineBestFreqEnabled { get; set; }

        [Required]
        public IEnumerable<string> OpenFiles
        {
            get { return _openFiles; }
            set
            {
                if (_openFiles != value)
                {
                    _openFiles = value;
                    var handler = OpenFilesChanged;

                    if (handler != null)
                    {
                        handler(this, new EventArgs());
                    }
                }
            }
        }

        public string SelectedRegion { get; set; }

        public HashSet<string> SelectedTechnologies { get; set; }

        public HashSet<string> SelectedOperators { get; set; }
    }
}
