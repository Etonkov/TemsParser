using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing;
using TemsParser.Models.TemsFileInfo;
using TemsParser.Models.Settings;
using TemsParser.Processing;

namespace TemsParser.IO
{
    public class TemsFileReader: IDisposable
    {
        #region Fields

        public const int LinesBufferLength = 10000;

        public readonly HeaderInfoModel HeaderInfo;

        private StreamReader _reader;

        private readonly long TotalRowCount;

        private long _currentBodyLineNumber;

        public readonly string FileName;

        #endregion


        #region Constructors

        public TemsFileReader(string filePath, string fileName, IEnumerable<TechnologyListItemModel> technologies)
        {
            _reader = new StreamReader(filePath);
            FileName = fileName;
            _currentBodyLineNumber = 0;
            HeaderInfo = ReadHeaderInfo(technologies);
            TotalRowCount = GetRowCount();
        }


        #endregion


        #region Properties

        public long BodyRowCount { get { return TotalRowCount - HeaderInfo.HeaderRowIndex; } }

        #endregion


        #region Methods

        public bool TryReadLines(out string[] lines)
        {
            lock (Global.LockFileOperations)
            {
                bool returns = true;
                lines = default(string[]);

                if (_currentBodyLineNumber == 0)
                {
                    InitializeStreamReader();

                    // Skeap before header.
                    for (long j = 0; j < HeaderInfo.HeaderRowIndex; j++)
                    {
                        _reader.ReadLine();
                    }
                }

                var remainingLineCount = BodyRowCount - _currentBodyLineNumber;

                if (remainingLineCount > 0)
                {
                    long arraySize;

                    if (remainingLineCount > LinesBufferLength)
                    {
                        arraySize = LinesBufferLength;
                    }
                    else
                    {
                        arraySize = remainingLineCount;
                    }

                    lines = new string[arraySize];

                    lock (Global.LockFileOperations)
                    {
                        for (int i = 0; i < arraySize; i++)
                        {
                            lines[i] = _reader.ReadLine();
                            _currentBodyLineNumber++;
                        }
                    }
                }
                else
                {
                    returns = false;
                }

                return returns;
            }
        }

        private HeaderInfoModel ReadHeaderInfo(IEnumerable<TechnologyListItemModel> technologies)
        {
            string line;
            long rowCounter = 0;
            var headerInfo = default(HeaderInfoModel);
            bool isValid = false;
            InitializeStreamReader();

            while ((line = _reader.ReadLine()) != null)
            {
                rowCounter++;

                if (line.Trim() == String.Empty)
                {
                    continue;
                }
                else /* First not empty column - header. */
                {
                    IEnumerable<ColumnInfoModel> columnInfoList;

                    isValid =
                        StringParser.TryParseStringIntoColumsInfoList(line, technologies, out columnInfoList);

                    headerInfo = new HeaderInfoModel(rowCounter, columnInfoList);
                    break;
                }
            }

            if (!isValid)
            {
                string message;

                if (technologies.Count() > 1)
                {
                    message = String.Format("Заголовок не содержит поля выбранных технологий: {0}.",
                                            String.Join(",", technologies));

                }
                else
                {
                    message = String.Format("Заголовок не содержит поля выбранной технологии: {0}.",
                                            technologies.FirstOrDefault());
                }

                throw new InvalidOperationException(message);
            }

            return headerInfo;
        }

        private long GetRowCount()
        {
            long counter = 0;
            string line;
            InitializeStreamReader();

            lock (Global.LockFileOperations)
            {
                while ((line = _reader.ReadLine()) != null)
                {
                    counter++;
                }
            }

            return counter;
        }

        private void InitializeStreamReader()
        {
            lock (Global.LockFileOperations)
            {
                _reader.BaseStream.Position = 0;
                _reader = new StreamReader(_reader.BaseStream);
            }
        }

        #endregion


        #region IDisposable

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }
        }

        #endregion
    }
}
