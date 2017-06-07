using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing;
using TemsParser.Models.TemsFileInfo;
using TemsParser.Processing;

namespace TemsParser.Extentions.Model.TemsFileInfo
{
    public static class HeaderInfoModelExtentions
    {
        public static IDictionary<TechnologyListItemModel, IDictionary<OperatorListItemModel,IEnumerable<int>>> GetFreqPool(this HeaderInfoModel headerInfo)
        {
            var returns = new Dictionary<TechnologyListItemModel, IDictionary<OperatorListItemModel, IEnumerable<int>>>();

            var technologyList = headerInfo.ColumnInfoList
                                     .Select(c => c.TechnologyItem)
                                     .SelectMany(t => t.Technologies);

            foreach (var technologyItem in technologyList)
            {
                var operList = new Dictionary<OperatorListItemModel, IEnumerable<int>>();
                foreach (var operatorItem in technologyItem.Operators)
                {
                    var totalSpectrum = String.Join(",", operatorItem.Freqs.Select(f => f.Spectrum));
                    var freqs = StringParser.ParseSpectrumIntoRfcnList(totalSpectrum);

                    //var freqPool = new FreqPoolModel(technologyItem.TechnologyListItem,
                    //                                 operatorItem.OperatorListItem,
                    //                                 freqs);

                    operList.Add(operatorItem.OperatorListItem, freqs);
                }

                returns.Add(technologyItem.TechnologyListItem, operList);
            }

            return returns;
        }
    }
}
