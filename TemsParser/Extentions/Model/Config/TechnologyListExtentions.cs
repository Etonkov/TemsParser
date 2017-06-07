using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Extentions.Model.Config
{
    public static class TechnologyListExtentions
    {
        /// <summary>
        /// Creates a new ObservableCollection that is a deep copy of the current ObservableCollection.
        /// </summary>
        /// <param name="technologiesList"></param>
        /// <returns></returns>
        public static ObservableCollection<TechnologyListItemModel> Clone(
            this ObservableCollection<TechnologyListItemModel> technologiesList,
            bool shallowCopyTechnologies = false)
        {
            var result = new ObservableCollection<TechnologyListItemModel>();

            foreach (var technologyListItem in technologiesList)
            {
                result.Add(new TechnologyListItemModel(technologyListItem,
                                                       shallowCopyTechnologies: shallowCopyTechnologies));
            }

            return result;
        }
    }
}
