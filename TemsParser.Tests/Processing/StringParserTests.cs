using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemsParser.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Parsing;
using TemsParser.Models.TemsFileInfo;
using TemsParser.Models.Config;

namespace TemsParser.Processing.Tests
{
    [TestClass()]
    public class StringParserTests
    {
        [TestMethod()]
        public void TryParseStringIntoRowValuesListTest()
        {
            string s = "11:17:48.70	MS2	0		MPH State Report			55.75937833	42.763585	1016	-112	1017	-106			55.38362	43.79558833	10563	-89.99	10836	-82.5";

            var gg = new TechnologyListItemModel();
            gg.Name = "2G";
            var ci1 = new ColumnInfoModel(gg, 7, 8, new Dictionary<int, int>() { { 9, 10 }, { 11, 12 } });

            var ggg = new TechnologyListItemModel();
            gg.Name = "3G";
            var ci2 = new ColumnInfoModel(ggg, 15, 16, new Dictionary<int, int>() { { 17, 18 }, { 19, 20 } });

            IEnumerable<RowDataModel> result;

            var isValid = StringParser.TryParseStringIntoRowDataList(s, new List<ColumnInfoModel>() { ci1, ci2 }, out result);

            Assert.IsTrue(isValid);

            var rowData1 = result.ToList()[0];
            Assert.IsTrue(rowData1.Latitude == 55.75937833);
            Assert.IsTrue(rowData1.Longitude == 42.763585);
            Assert.IsTrue(rowData1.FreqLevelValues.ToArray()[0].Freq == 1016);
            Assert.IsTrue(rowData1.FreqLevelValues.ToArray()[1].Freq == 1017);
            Assert.IsTrue(rowData1.FreqLevelValues.ToArray()[0].Level == -112);
            Assert.IsTrue(rowData1.FreqLevelValues.ToArray()[1].Level == -106);
            Assert.IsTrue(rowData1.FreqLevelValues.Count() == 2);

            var rowData2 = result.ToList()[1];
            Assert.IsTrue(rowData2.Latitude == 55.38362);
            Assert.IsTrue(rowData2.Longitude == 43.79558833);
            Assert.IsTrue(rowData2.FreqLevelValues.ToArray()[0].Freq == 10563);
            Assert.IsTrue(rowData2.FreqLevelValues.ToArray()[1].Freq == 10836);
            Assert.IsTrue(rowData2.FreqLevelValues.ToArray()[0].Level == -89.99);
            Assert.IsTrue(rowData2.FreqLevelValues.ToArray()[1].Level == -82.5);
            Assert.IsTrue(rowData2.FreqLevelValues.Count() == 2);
        }
    }
}