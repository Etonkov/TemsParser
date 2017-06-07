using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.Parsing
{
    public struct CoordinateModel
    {
        public CoordinateModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Double Latitude { get; private set; }
        public Double Longitude { get; private set; }

    }
}
