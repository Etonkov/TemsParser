using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemsParser.Models.Config;
using TemsParser.ViewModels.Config;
using System.Collections.Specialized;


namespace TemsParser.Extentions.Model.Config
{
    public static class TechnologyExtentions
    {
        /// <summary>
        /// Creates a copy of the current ObservableCollection.
        /// </summary>
        public static ObservableCollection<TechnologyModel> Clone(this ObservableCollection<TechnologyModel> technologies)
        {
            var result = new ObservableCollection<TechnologyModel>();

            foreach (var technologyItem in technologies)
            {
                result.Add(new TechnologyModel(technologyItem));
            }

            return result;
        }

        public static TreeViewItemViewModel ToTreeViewItemViewModel(this TechnologyModel technology, TreeViewItemViewModel parent)
        {
            var result = new TreeViewItemViewModel(ObjectTypes.Technology)
            {
                Name = technology.ToString(),
                Id = technology.Id,
                Parent = parent,
                Children = new ObservableCollection<TreeViewItemViewModel>()
            };

            foreach (var operatorItem in technology.Operators)
            {
                result.Children.Add(operatorItem.ToTreeViewItemViewModel(result));
            }

            technology.ToStringValueChanged += (s, e) =>
            {
                if (result.Name != technology.ToString())
                {
                    result.Name = technology.ToString();
                }
            };

            technology.Operators.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var newOperator = e.NewItems[0] as OperatorModel;
                            result.Children.Add(newOperator.ToTreeViewItemViewModel(result));
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
                            var newOperator = e.NewItems[0] as OperatorModel;
                            result.Children[e.OldStartingIndex] = newOperator.ToTreeViewItemViewModel(result);
                        }
                        break;
                }
            };

            return result;
        }
    }
}
