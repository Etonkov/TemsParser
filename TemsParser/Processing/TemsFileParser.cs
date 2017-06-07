using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.IO;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing;
using TemsParser.Models.TemsFileInfo;
using TemsParser.Models.Settings;
using System.ComponentModel;
using TemsParser.Extentions.Model.TemsFileInfo;
using TemsParser.Models.Parsing.ThreadHelpers;
using System.Threading;

namespace TemsParser.Processing
{
    public class TemsFileParser: IDisposable
    {
        private readonly object LockObj = new Object();
        private static readonly int ProcessorCount = Environment.ProcessorCount;
        private readonly bool BinningEnabled;
        private readonly bool CompareOperatorsEnabled;
        private readonly int BinningSize;
        private readonly List<Task> Tasks;
        private readonly TemsFileReader Reader;
        private readonly IDictionary<TechnologyListItemModel, IDictionary<OperatorListItemModel, IEnumerable<int>>> FreqPool;
        public event EventHandler Finished;
        public event EventHandler RowParsed;
        public event BestLevelFoundEventHandler BestLevelFoundEvent;
        public delegate void BestLevelFoundEventHandler(object sender, BestLevelFoundEventArgs ea);
        public delegate void TemsFileParserFinishedEventHandler(object s, TemsFileParserFinishedEventArgs ea);


        public TemsFileParser(TemsFileReader reader, SettingsModel settings)
        {
            BinningEnabled = settings.BinningEnabled;
            BinningSize = settings.BinningSize;
            CompareOperatorsEnabled = settings.CompareOperatorsEnabled;
            Reader = reader;
            FreqPool = reader.HeaderInfo.GetFreqPool();
            Tasks = new List<Task>();
        }

        private void OnRowParsed()
        {
            var handler = RowParsed;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void OnFinished()
        {
            var handler = Finished;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void OnBestLevelFound(BestLevelFoundEventArgs ea)
        {
            var handler = BestLevelFoundEvent;

            if (handler != null)
            {
                handler(this, ea);
            }
        }

        public void Parse(CancellationToken cancelToken)
        {
            long rowCount = Reader.BodyRowCount;
            long bufferLength = TemsFileReader.LinesBufferLength;
            var initialThreadCount = (int)Math.Min(ProcessorCount, Math.Ceiling((decimal)rowCount / bufferLength));

            Exception outsideException = default(Exception);

            for (long i = 0; i < initialThreadCount; i++)
            {

                Tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        ParallelParsePart(Reader.HeaderInfo.ColumnInfoList, cancelToken);
                    }
                    catch (Exception e)
                    {
                        outsideException = e;
                    }
                }));
            }

            foreach (var taskItem in Tasks)
            {
                taskItem.Wait();
            }

            if (outsideException != null)
            {
                throw outsideException;
            }

            OnFinished();
        }

        private void ParallelParsePart(IEnumerable<ColumnInfoModel> columnInfoList, CancellationToken token)
        {
            string[] lines;

            while (Reader.TryReadLines(out lines))
            {
                Parallel.ForEach(lines, new ParallelOptions { CancellationToken = token }, (line) =>
                {
                    if (line != null)
                    {
                        IEnumerable<RowDataModel> rowDataList;

                        bool rowDataResult =
                            StringParser.TryParseStringIntoRowDataList(line, columnInfoList, out rowDataList);

                        if (rowDataResult)
                        {
                            foreach (var rowData in rowDataList)
                            {
                                foreach (var freqPoolKvp in FreqPool[rowData.TechnologyItem])
                                {
                                    var filteredFreqLevel = from f in rowData.FreqLevelValues
                                                            where freqPoolKvp.Value.Contains(f.Freq)
                                                            select f;

                                    var filteredFreqLevelList = filteredFreqLevel.ToList();

                                    if (filteredFreqLevelList.Count > 0)
                                    {
                                        var bestFreqLevel = Calculator.GetBestFreqLevel(filteredFreqLevel);

                                        var ea = new BestLevelFoundEventArgs(new CoordinateModel(rowData.Latitude, rowData.Longitude),
                                                                             rowData.TechnologyItem,
                                                                             freqPoolKvp.Key,
                                                                             bestFreqLevel);

                                        OnBestLevelFound(ea);
                                    }
                                }
                            }
                        }

                        OnRowParsed();
                    }
                });
            }
        }

        public void Dispose()
        {
            Reader.Dispose();

            //foreach (var item in Tasks)
            //{
            //    item.
            //}
        }
    }
}
