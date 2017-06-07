using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing;
using TemsParser.Models.TemsFileInfo;


namespace TemsParser.Processing
{
    public class StringParser
    {
        private StringParser()
        { }


        private static bool GetColumnNamePairIndex(string s, out int result)
        {
            result = -1;

            int firstIndex = s.IndexOf('[');
            int secondIndex = s.IndexOf(']');

            if ((firstIndex > 0) && (secondIndex > firstIndex))
            {
                string stringPairIndex = s.Substring(firstIndex + 1, secondIndex - firstIndex - 1);

                return Int32.TryParse(stringPairIndex, out result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its double-precision floating-point number equivalent.
        /// Allows comma ',' and dot '.' chars decimal point and white-space chars in input string parameter.
        /// </summary>
        /// <param name="s">Parameter requires a string argument.</param>
        /// <returns>The method returns a double.</returns>
        public static double ParseStringIntoDouble(string s)
        {
            return Double.Parse(s.ToString().Replace(",", "."),
                                NumberStyles.Number,
                                CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Converts the string representation of a number to its double-precision floating-point number equivalent.
        /// Allows comma ',' and dot '.' chars decimal point and white-space chars in input string parameter.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">Parameter requires a string argument.</param>
        /// <param name="result">Returns a double.</param>
        /// <returns>Value indicates whether the conversion succeeded or failed.</returns>
        public static bool TryParseStringIntoDouble(string s, out double result)
        {
            return Double.TryParse(s.ToString().Replace(",", "."),
                                   NumberStyles.Number,
                                   CultureInfo.InvariantCulture.NumberFormat,
                                   out result);
        }


        /// <summary>
        /// Converts the string representation of a spectrum to a list of rfcn values.
        /// Returns a distinct and sorted list of frequencies
        /// Uses comma ',' and semicolon ';' chars for split frequencies and frequency ranges.
        /// Uses dash '-' char for split low and higth frequencies in frequency ranges.
        /// </summary>
        /// <param name="s">Parameter requires a string representation of a spectrum argument.</param>
        /// <returns>The method returns a distinct and sorted list of frequencies.></returns>
        public static IEnumerable<int> ParseSpectrumIntoRfcnList(string s)
        {
            // The returned list of frequencies.
            var result = new HashSet<int>();

            // Split a string representation of spectrum to a string array of freq ranges.
            string[] ranges = s.Split(new char[] { ';', ',' });

            foreach (var rangeItem in ranges)
            {
                // Split a string representation of range to a string array of frequency.
                string[] freqs = rangeItem.Split('-');

                // The number of frequencies in string array of frequency range.
                var freqsLength = freqs.Length;

                switch (freqsLength)
                {
                    // If frequency range contains one frequency then add this freq to freq list.
                    case 1:
                        {
                            int freq = Int32.Parse(freqs[0]);
                            result.Add(freq);
                        }
                        break;
                    // If frequency range contain two frequency(lower and higher)
                    // then add to frequency list values from lower to higher.
                    case 2:
                        {
                            int freqLow = Int32.Parse(freqs[0]);
                            int freqHi = Int32.Parse(freqs[1]);
                            
                            for (int i = freqLow; i <= freqHi; i++)
                            {
                                result.Add(i);
                            }
                        }
                        break;
                    // othewise, throw an exception.
                    default:
                        throw new ArgumentException("Cannot parse spectrum: freq range contain more than two.");
                }
            }

            // Returns sorted list.
            return new HashSet<int>(result.OrderBy(o => o));
        }

        public static bool TryParseStringIntoColumsInfoList(string s,
            IEnumerable<TechnologyListItemModel> technologiesList,
            out IEnumerable<ColumnInfoModel> result)
        {
            result = new List<ColumnInfoModel>();

            foreach (var technology in technologiesList)
            {
                string latitudeColumnName = technology.LatitudeColumnName;
                string longitudeColumnName = technology.LongitudeColumnName;
                string levelColumnNamePart = technology.LevelColumnNamePart;
                string freqColumnNamePart = technology.FreqColumnNamePart;
                int latitudeColumnIndex = -1;
                int longitudeColumnIndex = -1;
                Dictionary<int, int> freqLevelColumnIndexes = new Dictionary<int, int>();
                Dictionary<int, int> freqColumnIndexes = new Dictionary<int, int>();
                Dictionary<int, int> levelColumnIndexes = new Dictionary<int, int>();

                string[] columns = s.Split('\t');

                for (int index = 0; index < columns.Length; index++)
                {
                    string name = columns[index];

                    if (name == latitudeColumnName)
                    {
                        latitudeColumnIndex = index;
                        continue;
                    }

                    if (name == longitudeColumnName)
                    {
                        longitudeColumnIndex = index;
                        continue;
                    }

                    if (name.Contains(freqColumnNamePart))
                    {
                        int freqPairIndex;

                        if (GetColumnNamePairIndex(name, out freqPairIndex))
                        {
                            if (freqColumnIndexes.ContainsKey(freqPairIndex) == false)
                            {
                                freqColumnIndexes.Add(freqPairIndex, index);
                                continue;
                            }
                        }
                    }

                    if (name.Contains(levelColumnNamePart))
                    {
                        int levelPairIndex;

                        if (GetColumnNamePairIndex(name, out levelPairIndex))
                        {
                            if (levelColumnIndexes.ContainsKey(levelPairIndex) == false)
                            {
                                levelColumnIndexes.Add(levelPairIndex, index);
                                continue;
                            }
                        }
                    }
                }

                foreach (var freqColumnIndexPair in freqColumnIndexes)
                {
                    int levelColumnIndex;

                    if (levelColumnIndexes.TryGetValue(freqColumnIndexPair.Key, out levelColumnIndex))
                    {
                        freqLevelColumnIndexes.Add(freqColumnIndexPair.Value, levelColumnIndex);
                    }
                }

                if ((latitudeColumnIndex >= 0) && (longitudeColumnIndex >= 0) && (freqLevelColumnIndexes.Count > 0))
                {
                    var newTechnologyData = new ColumnInfoModel(technology,
                                                                latitudeColumnIndex,
                                                                longitudeColumnIndex,
                                                                freqLevelColumnIndexes);

                    ((List<ColumnInfoModel>)result).Add(newTechnologyData);
                }
            }

            return (result.Count() > 0);
        }

        public static bool TryParseStringIntoRowDataList(string s,
                                                         IEnumerable<ColumnInfoModel> columnInfoList,
                                                         out IEnumerable<RowDataModel> result)
        {
            var RowValues = new List<RowDataModel>();
            var splitedString = s.Split('\t');
            result = default(IEnumerable<RowDataModel>);
            var maxIndex = splitedString.Count() - 1;

            foreach (var columnInfo in columnInfoList)
            {
                if ((columnInfo.LatitudeIndex > maxIndex) || (columnInfo.LongitudeIndex > maxIndex))
                {
                    continue;
                }

                double latitude;
                bool latitudeResult = TryParseStringIntoDouble(splitedString[columnInfo.LatitudeIndex], out latitude);
                if (!latitudeResult) continue;

                double longitude;
                bool longitudeResult =
                    TryParseStringIntoDouble(splitedString[columnInfo.LongitudeIndex], out longitude);
                if (!longitudeResult) continue;

                if ((latitude > 90) || (latitude < -90) || (longitude > 180) || (longitude < -180))
                {
                    continue;
                }

                var freqLevelValues = new List<FreqLevelPairModel>();

                foreach (var freqLevelIndex in columnInfo.FreqLevelIndexes)
                {
                    if ((freqLevelIndex.Key > maxIndex) || (freqLevelIndex.Value > maxIndex))
                    {
                        continue;
                    }

                    int freq;
                    bool freqResult = Int32.TryParse(splitedString[freqLevelIndex.Key], out freq);

                    if (!freqResult) continue;

                    double level;
                    bool levelResult = TryParseStringIntoDouble(splitedString[freqLevelIndex.Value], out level);

                    if (levelResult && freq >= 0)
                    {
                        freqLevelValues.Add(new FreqLevelPairModel(freq, level));
                    }
                }

                if (freqLevelValues.Count > 0)
                {
                    RowValues.Add(new RowDataModel(columnInfo.TechnologyItem, latitude, longitude, freqLevelValues));
                }
            }

            if (RowValues.Count > 0)
            {
                result = RowValues;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetFileName(string filePath)
        {
            string[] pathItems = filePath.Split('\\');
            return pathItems.LastOrDefault();
        }
    }
}
