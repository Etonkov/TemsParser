using Ninject;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
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
    public class RepositoryModel : RepositoryBase
    {
        private readonly bool WithFreq;

        private readonly Dictionary<TechnologyListItemModel,
            Dictionary<OperatorListItemModel, ParsedFileWriter>> Writers;

        protected readonly object ThisLockObj = new Object();




        public RepositoryModel(IEnumerable<TechnologyListItemModel> techList,
                               IEnumerable<OperatorListItemModel> operList,
                               SettingsModel settings,
                               string directoryBase,
                               bool withFreq) : base(directoryBase)
        {
            Writers = new Dictionary<TechnologyListItemModel,
                       Dictionary<OperatorListItemModel, ParsedFileWriter>>();

            WithFreq = withFreq;

            foreach (var techItem in techList)
            {
                var operDict = new Dictionary<OperatorListItemModel, ParsedFileWriter>();

                foreach (var operItem in operList)
                {
                    var filePath = DirectoryBase + String.Format("_{0}_{1}.txt", techItem, operItem);
                    operDict.Add(operItem, new ParsedFileWriter(filePath, GetHeader(WithFreq, operItem)));
                }

                Writers.Add(techItem, operDict);
            }
        }


        public override void AddValue(Object s, BestLevelFoundEventArgs ea)
        {
            var operArea = Writers[ea.Technology];
            var fileWriter = operArea[ea.Operator];

            string line = ea.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\t"
                + ea.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\t"
                + ea.FreqLevelPair.ToString(WithFreq);

            fileWriter.AddLine(line);
        }

        public override int Save()
        {
            int fileCount = 0;

            foreach (var techWriterKvp in Writers)
            {
                foreach (var operWriterKvp in techWriterKvp.Value)
                {
                    var writer = operWriterKvp.Value;

                    if (writer.IsInitialized)
                    {
                        fileCount++;
                    }

                    writer.Dispose();
                }
            }

            return fileCount;
        }
    }
}
