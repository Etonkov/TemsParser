using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using TemsParser.Common;
using TemsParser.Models.Config;

namespace TemsParser.Models.TemsFileInfo
{
    public class ColumnInfoModel
    {
        public ColumnInfoModel(TechnologyListItemModel technology,
                               int latitudeIndex,
                               int longitudeIndex,
                               Dictionary<int, int> freqLevelIndexes)
        {
            this.TechnologyItem = technology;
            this.LatitudeIndex = latitudeIndex;
            this.LongitudeIndex = longitudeIndex;
            this.FreqLevelIndexes = freqLevelIndexes;
        }

        public TechnologyListItemModel TechnologyItem { get; private set; }

        public int LatitudeIndex { get; private set; }

        public int LongitudeIndex { get; private set; }

        public Dictionary<int, int> FreqLevelIndexes { get; private set; }
    }
}
