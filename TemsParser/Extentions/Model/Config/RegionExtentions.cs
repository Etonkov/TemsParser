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
    public static class RegionExtentions
    {
        /// <summary>
        /// Creates a copy of the current ObservableCollection.
        /// </summary>
        public static ObservableCollection<RegionModel> Clone(this ObservableCollection<RegionModel> regions)
        {
            var result = new ObservableCollection<RegionModel>();

            foreach (var regionsItem in regions)
            {
                result.Add(new RegionModel(regionsItem));
            }

            return result;
        }

        public static ObservableCollection<TreeViewItemViewModel> ToTreeViewTopItems(this ObservableCollection<RegionModel> regions)
        {
            var result = new ObservableCollection<TreeViewItemViewModel>();

            foreach (var regionItem in regions)
            {
                result.Add(regionItem.ToTreeViewItemViewModel());
            }

            regions.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var newRegion = e.NewItems[0] as RegionModel;
                            result.Add(newRegion.ToTreeViewItemViewModel());
                            break;
                        }
                    case NotifyCollectionChangedAction.Move:
                        {
                            result.Move(e.OldStartingIndex, e.NewStartingIndex);
                            break;
                        }
                    case NotifyCollectionChangedAction.Remove:
                        {
                            result.RemoveAt(e.OldStartingIndex);
                            break;
                        }
                    case NotifyCollectionChangedAction.Replace:
                        {
                            var newRegion = e.NewItems[0] as RegionModel;
                            result[e.OldStartingIndex] = newRegion.ToTreeViewItemViewModel();
                            break;
                        }
                }
            };

            return result;
        }

        public static TreeViewItemViewModel ToTreeViewItemViewModel(this RegionModel region)
        {
            var result = new TreeViewItemViewModel(ObjectTypes.Region)
            {
                Name = region.ToString(),
                Id = region.Id,
                Children = new ObservableCollection<TreeViewItemViewModel>()
            };

            foreach (var technologyItem in region.Technologies)
            {
                result.Children.Add(technologyItem.ToTreeViewItemViewModel(result));
            }

            region.ToStringValueChanged += (s, e) =>
            {
                if (result.Name != region.ToString())
                {
                    result.Name = region.ToString();
                }
            };

            region.Technologies.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var newTechnology = e.NewItems[0] as TechnologyModel;
                            result.Children.Add(newTechnology.ToTreeViewItemViewModel(result));
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
                            var newTechnology = e.NewItems[0] as TechnologyModel;
                            result.Children[e.OldStartingIndex] = newTechnology.ToTreeViewItemViewModel(result);
                        }
                        break;
                }
            };

            return result;
        }
    }
}
