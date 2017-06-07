using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using TemsParser.Common;
using TemsParser.Models.Config;

namespace TemsParser.Models.Parsing
{
    public class RowDataModel
    {
        public RowDataModel(TechnologyListItemModel technologyItem,
                            double latitude,
                            double longitude,
                            IEnumerable<FreqLevelPairModel> freqLevelValues)
        {
            this.TechnologyItem = technologyItem;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.FreqLevelValues = freqLevelValues;
        }


        public TechnologyListItemModel TechnologyItem { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public IEnumerable<FreqLevelPairModel> FreqLevelValues { get; private set; }
    }
}
