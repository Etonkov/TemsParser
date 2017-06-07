
using TemsParser.Models.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using TemsParser.Processing;
using System.Collections.Generic;
using TemsParser.Models.TemsFileInfo;
using System.IO;


namespace TemsParser.Tests
{
    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void Parsing_TryParseStringIntoTechnologyColumnIndexes()
        {
            // Arrange.
            string input = @"Time	MS	Frame Number	Direction	Message Type	Event	EventInfo	All-Latitude"
                + "	All-Longitude	All-Scanned ARFCN[0]	All-Scanned RxLev (dBm)[0]	"
                + "All-Scanned ARFCN[1]	All-Scanned RxLev (dBm)[1]";
            IEnumerable<ColumnInfoModel> result;
            bool isValid;

            var tech = new TechnologyListItemModel()
            {
                Name = "2G",
                LatitudeColumnName = "All-Latitude",
                LongitudeColumnName = "All-Longitude",
                FreqColumnNamePart = "All-Scanned ARFCN",
                LevelColumnNamePart = "All-Scanned RxLev (dBm)"
            };



            // Act.
            isValid = StringParser.TryParseStringIntoColumsInfoList(input,
                new List<TechnologyListItemModel>() { tech },
                out result);

            // Assert.
            Assert.IsTrue(isValid);

            foreach (var resultItem in result)
            {
                Assert.AreEqual(7, resultItem.LatitudeIndex);
                Assert.AreEqual(8, resultItem.LongitudeIndex);

                var freqLevel = resultItem.FreqLevelIndexes.ToList();

                Assert.AreEqual(9, freqLevel[0].Key);
                Assert.AreEqual(10, freqLevel[0].Value);
                Assert.AreEqual(11, freqLevel[1].Key);
                Assert.AreEqual(12, freqLevel[1].Value);
            }
        }
    }
}
