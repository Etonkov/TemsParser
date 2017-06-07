using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.ViewModels.Config;

namespace TemsParser.Extentions.Model.Config
{
    [Serializable]
    public static class FreqExtentions
    {
        /// <summary>
        /// Creates a copy of the current ObservableCollection.
        /// </summary>
        public static ObservableCollection<FreqModel> Clone(this ObservableCollection<FreqModel> freqs)
        {
            var result = new ObservableCollection<FreqModel>();

            foreach (var freqItem in freqs)
            {
                result.Add(new FreqModel(freqItem));
            }

            return result;
        }

        public static TreeViewItemViewModel ToTreeViewItemViewModel(this FreqModel freq, TreeViewItemViewModel parent)
        {
            var result = new TreeViewItemViewModel(ObjectTypes.Freq)
            {
                Name = freq.ToString(),
                Id = freq.Id,
                Parent = parent
            };

            freq.ToStringValueChanged += (s, e) =>
            {
                if (result.Name != freq.ToString())
                {
                    result.Name = freq.ToString();
                }
            };

            return result;
        }
    }
}
