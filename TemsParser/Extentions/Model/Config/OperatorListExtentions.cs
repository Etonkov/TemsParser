using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Extentions.Model.Config
{
    public static class OperatorListExtentions
    {
        /// <summary>
        /// Creates a new ObservableCollection that is a deep copy of the current ObservableCollection.
        /// </summary>
        /// <param name="technologiesList"></param>
        /// <returns></returns>
        public static ObservableCollection<OperatorListItemModel> Clone(
            this ObservableCollection<OperatorListItemModel> operatorList)
        {
            var result = new ObservableCollection<OperatorListItemModel>();

            foreach (var operatorListItem in operatorList)
            {
                result.Add(new OperatorListItemModel(operatorListItem));
            }

            return result;
        }
    }
}
