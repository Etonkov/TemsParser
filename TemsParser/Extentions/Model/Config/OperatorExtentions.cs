using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.ViewModels.Config;

namespace TemsParser.Extentions.Model.Config
{
    public static class OperatorExtentions
    {
        /// <summary>
        /// Creates a copy of the current ObservableCollection.
        /// </summary>
        public static ObservableCollection<OperatorModel> Clone(this ObservableCollection<OperatorModel> operators)
        {
            var result = new ObservableCollection<OperatorModel>();

            foreach (var operItem in operators)
            {
                result.Add(new OperatorModel(operItem));
            }

            return result;
        }

        public static TreeViewItemViewModel ToTreeViewItemViewModel(this OperatorModel oper, TreeViewItemViewModel parent)
        {
            var result = new TreeViewItemViewModel(ObjectTypes.Operator)
            {
                Name = oper.ToString(),
                Id = oper.Id,
                Parent = parent,
                Children = new ObservableCollection<TreeViewItemViewModel>()
            };

            foreach (var freqItem in oper.Freqs)
            {
                result.Children.Add(freqItem.ToTreeViewItemViewModel(result));
            }

            oper.ToStringValueChanged += (s, e) =>
            {
                if (result.Name != oper.ToString())
                {
                    result.Name = oper.ToString();
                }
            };

            oper.Freqs.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var newFreq = e.NewItems[0] as FreqModel;
                            result.Children.Add(newFreq.ToTreeViewItemViewModel(result));
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        {
                            result.Children.Move(e.OldStartingIndex, e.NewStartingIndex);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        {
                            result.Children.RemoveAt(e.OldStartingIndex);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        {
                            var newFreq = e.NewItems[0] as FreqModel;
                            result.Children[e.OldStartingIndex] = newFreq.ToTreeViewItemViewModel(result);
                        }
                        break;
                }
            };

            return result;
        }
    }
}
