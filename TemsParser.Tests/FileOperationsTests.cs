
using TemsParser.Models.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using TemsParser.Processing;
using System.Collections.Generic;
using TemsParser.Models.TemsFileInfo;
using System.IO;
using TemsParser.IO;


namespace TemsParser.Tests
{
    [TestClass]
    public class FileOperationsTests
    {
        [TestMethod]
        public void FileOperations_GetTechnologyDataList()
        {
            string header = @"Time	MS	Frame Number	Direction	Message Type	Event	EventInfo	All-Latitude"
                + "	All-Longitude	All-Scanned ARFCN[0]	All-Scanned RxLev (dBm)[0]	"
                + "All-Scanned ARFCN[1]	All-Scanned RxLev (dBm)[1]";

            string[] lines = { String.Empty, String.Empty, String.Empty, header, "Line1", "Line2", "etc." };
            string testFileName = "TemsParser_FileOperationsTests_GetTechnologyDataList.txt";

            var tech = new TechnologyListItemModel()
            {
                Name = "2G",
                LatitudeColumnName = "All-Latitude",
                LongitudeColumnName = "All-Longitude",
                FreqColumnNamePart = "All-Scanned ARFCN",
                LevelColumnNamePart = "All-Scanned RxLev (dBm)"
            };


            // Act.
            if (File.Exists(testFileName))
	        {
		        File.Delete(testFileName);
	        }

            File.WriteAllLines(testFileName, lines);
            var technologies = new List<TechnologyListItemModel>() { tech };
            //TemsFileInfoModel temsFileData;
            //FileStream fstream = File.OpenRead(testFileName);
            //bool isValid;
            HeaderInfoModel headerInfo;
            using (TemsFileReader reader = new TemsFileReader(testFileName, testFileName, technologies))
            {
                //isValid = reader.ReadFileInfo();
                headerInfo = reader.HeaderInfo;
            };

            if (File.Exists(testFileName))
            {
                File.Delete(testFileName);
            }

            // Assert.
            //Assert.IsTrue(isValid);
            Assert.AreEqual(4, headerInfo.HeaderRowIndex);

            foreach (var columnInfo in headerInfo.ColumnInfoList)
            {
                Assert.AreEqual(7, columnInfo.LatitudeIndex);
                Assert.AreEqual(8, columnInfo.LongitudeIndex);

                var freqLevel = columnInfo.FreqLevelIndexes.ToList();

                Assert.AreEqual(9, freqLevel[0].Key);
                Assert.AreEqual(10, freqLevel[0].Value);
                Assert.AreEqual(11, freqLevel[1].Key);
                Assert.AreEqual(12, freqLevel[1].Value);
            }
        }
    }
}
