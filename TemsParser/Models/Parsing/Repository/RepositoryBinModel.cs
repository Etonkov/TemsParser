using Ninject;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TemsParser.Common;
using TemsParser.IO;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing.Area;
using TemsParser.Models.Parsing.Point;
using TemsParser.Models.Settings;
using TemsParser.Processing;

namespace TemsParser.Models.Parsing.Repository
{
    public class RepositoryBinModel<T> : RepositoryBase
        where T: struct, IPoint
    {
        private readonly int BinningSize;
        private double _areaLatitudeSizeAngle;
        private double _areaLongitudeSizeAngle;
        protected readonly object ThisLockObj = new Object();
        private CoordinateModel _basePoint;
        private bool _isInitialized;
        private Calculator _calculator;

        private readonly Dictionary<TechnologyListItemModel,
            Dictionary<OperatorListItemModel, List<AreaBinModel<T>>>> Areas;


        public RepositoryBinModel(IEnumerable<TechnologyListItemModel> techList,
                                  IEnumerable<OperatorListItemModel> operList,
                                  SettingsModel settings,
                                  string directoryBase): base(directoryBase)
        {
            Areas = new Dictionary<TechnologyListItemModel,
                       Dictionary<OperatorListItemModel, List<AreaBinModel<T>>>>();

            bool withFreq = typeof(T) == typeof(PointFreqModel);

            foreach (var techItem in techList)
            {
                var operDict = new Dictionary<OperatorListItemModel, List<AreaBinModel<T>>>();

                foreach (var operItem in operList)
                {
                    operDict.Add(operItem, new List<AreaBinModel<T>>());
                }

                Areas.Add(techItem, operDict);
            }

            BinningSize = settings.BinningSize;
            _isInitialized = false;
            //_baseLatitude = _baseLongitude = Double.NaN;
        }


        private bool TryFindArea(List<AreaBinModel<T>> areaList, CoordinateModel coordinate, out AreaBinModel<T> area)
        {
            area = default(AreaBinModel<T>);

            for (int i = 0; i < areaList.Count; i++)
            {
                var areaItem = areaList[i];

                bool inRange = (coordinate.Latitude >= areaItem.MinLatitude) &&
                               (coordinate.Latitude < areaItem.MaxLatitude) &&
                               (coordinate.Longitude >= areaItem.MinLongitude) &&
                               (coordinate.Longitude < areaItem.MaxLongitude);

                if (inRange)
                {
                    area = areaItem;
                    return true;
                }
            }

            return false;
        }

        public override void AddValue(Object s, BestLevelFoundEventArgs ea)
        {
            if (_isInitialized == false)
            {
                lock (ThisLockObj)
                {
                    if (_isInitialized == false)
                    {
                        _calculator = new Calculator(ea.Coordinate.Latitude);
                        _areaLatitudeSizeAngle = _calculator.GetAngle(BinningSize * AreaBase.Dimentions, false);
                        _areaLongitudeSizeAngle = _calculator.GetAngle(BinningSize * AreaBase.Dimentions, true);
                        _basePoint = _calculator.GetFirstBasePoint(ea.Coordinate, AreaBase.Dimentions, BinningSize);
                        _isInitialized = true;
                    }
                }
            }


            var operArea = Areas[ea.Technology];
            var areaList = operArea[ea.Operator];
            AreaBinModel<T> area;

            if (TryFindArea(areaList, ea.Coordinate, out area) == false)
            {
                lock (areaList)
                {
                    if (TryFindArea(areaList, ea.Coordinate, out area) == false)
                    {
                        var minPoint = Calculator.GetAreaBasePoint(_basePoint,
                                                                   ea.Coordinate,
                                                                   _areaLatitudeSizeAngle,
                                                                   _areaLongitudeSizeAngle);

                        var maxPoint = new CoordinateModel(minPoint.Latitude + _areaLatitudeSizeAngle,
                                                           minPoint.Longitude + _areaLongitudeSizeAngle);

                        area = new AreaBinModel<T>(ea.Technology,
                                                   ea.Operator,
                                                   minPoint,
                                                   maxPoint,
                                                   BinningSize,
                                                   _calculator);

                        areaList.Add(area);
                    }
                }
            }

            area.AddValue(ea.Coordinate, ea.FreqLevelPair);
        }

        public override int Save()
        {
            var lockObj = new Object();
            Exception outerException = default(Exception);
            int fileCount = 0;

            string header;
            bool withFreq;

            if (typeof(T) == typeof(PointFreqModel))
            {
                withFreq = true;
            }
            else
            {
                withFreq = false;
            }

            Parallel.ForEach(Areas, (techArea) =>
            {
                Parallel.ForEach(techArea.Value, (operArea) =>
                {
                    string fileName = DirectoryBase + String.Format("_{0}_{1}.txt", techArea.Key, operArea.Key);

                    try
                    {
                        header = GetHeader(withFreq, operArea.Key);

                        using (var writer = new ParsedFileWriter(fileName, header))
                        {
                            Parallel.ForEach(operArea.Value, (areaItem) =>
                            {
                                var lines = areaItem.GetResult();

                                try
                                {
                                    writer.AddLines(lines);
                                }
                                catch (Exception e)
                                {
                                    outerException = e;
                                }
                            });

                            if (writer.IsInitialized)
                            {
                                lock (lockObj)
                                {
                                    fileCount++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        outerException = e;
                    }
                });
            });

            if (outerException != null)
            {
                throw outerException;
            }

            return fileCount;
        }
    }
}
