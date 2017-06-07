using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Extentions.Model.Config
{
    public static class RegionListExtentions
    {
        /// <summary>
        /// Creates a new ObservableCollection that is a deep copy of the current ObservableCollection.
        /// </summary>
        /// <param name="technologiesList"></param>
        /// <returns></returns>
        public static ObservableCollection<RegionListItemModel> Clone(
            this ObservableCollection<RegionListItemModel> regionsList)
        {
            var result = new ObservableCollection<RegionListItemModel>();

            foreach (var technologyListItem in regionsList)
            {
                result.Add(new RegionListItemModel(technologyListItem));
            }

            return result;
        }
    }
}
