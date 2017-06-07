using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.IO;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing.Point;
using TemsParser.Processing;

namespace TemsParser.Models.Parsing.Area
{
    public class AreaBinModel<T> : AreaBase
                where T : struct, IPoint
    {
        private readonly Object LockObj = new Object();
        private readonly int BinningSize;
        private readonly Calculator Calc;
        private T[,] _points;


        public AreaBinModel(TechnologyListItemModel technology,
                            OperatorListItemModel oper,
                            CoordinateModel minPoint,
                            CoordinateModel maxPoint,
                            int binningSize,
                            Calculator calc)
            : base(technology, oper)
        {
            Calc = calc;
            InSavingMode = false;
            BinningSize = binningSize;
            _points = new T[Dimentions, Dimentions];

            for (int i = 0; i < Dimentions; i++)
            {
                for (int j = 0; j < Dimentions; j++)
                {
                    _points[i, j].Initialize();
                }
            }

            MinLatitude = minPoint.Latitude;
            MinLongitude = minPoint.Longitude;
            MaxLatitude = maxPoint.Latitude;
            MaxLongitude = maxPoint.Longitude;
        }

        private void DeserialisePoints()
        {
            lock (LockObj)
            {
                _points = FileReader.DeserialisePointArray<T>(Path);
                InSavingMode = false;
            }
        }

        public override void AddValue(CoordinateModel coordinate, FreqLevelPairModel freqLevelPair)
        {
            int xIndex = (int)Calc.GetLength(coordinate.Latitude - MinLatitude, false) / BinningSize;
            int yIndex = (int)Calc.GetLength(coordinate.Longitude - MinLongitude, true) / BinningSize;

            _points[xIndex, yIndex].AddValue(freqLevelPair);
        }

        public override List<string> GetResult()
        {
            List<string> result = new List<string>();
            double latitudeStep = Calc.GetAngle(BinningSize, false);
            double longitudeStep = Calc.GetAngle(BinningSize, true);


            for (int i = 0; i < AreaBase.Dimentions; i++)
            {
                for (int j = 0; j < AreaBase.Dimentions; j++)
                {
                    FreqLevelPairModel freqLevelPair;

                    if (_points[i, j].GetValue(out freqLevelPair))
                    {
                        var latitude = MinLatitude + latitudeStep * i + latitudeStep / 2;
                        var longitude = MinLongitude + longitudeStep * j + longitudeStep / 2;

                        string line = latitude.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\t"
                            + longitude.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\t" + freqLevelPair;

                        result.Add(line);
                    }
                }
            }

            return result;
        }
    }
}
