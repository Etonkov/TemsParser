using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Common;

namespace TemsParser.Models.Parsing
{
    public struct AreaItemModel
    {
        public AreaItemModel(double latitude, double longitude, int freq, double level)
        {
            Latitude = latitude;
            Longitude = longitude;
            Freq = freq;
            Level = level;
        }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public int Freq { get; private set; }

        public double Level { get; private set; }
    }
}
