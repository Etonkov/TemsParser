using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemsParser.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Parsing;
using TemsParser.Models.Parsing.Area;
using TemsParser.Models.Parsing.Point;
using TemsParser.Models.Config;

namespace TemsParser.Processing.Tests
{
    [TestClass()]
    public class CalculatorTests
    {
        private const int EquatorLenght = 40075016;

        [TestMethod()]
        public void GetDistanceTest()
        {
            // Arrange + Act.
            var halfEquatorLenght1 = Math.Round(Calculator.GetDistance(0, 0, 0, 180), 0);
            var halfEquatorLenght2 = Math.Round(Calculator.GetDistance(-90, 0, 90, 0), 0);
            var halfEquatorLenght3 = Math.Round(Calculator.GetDistance(0, -180, 0, 0), 0);
            var halfEquatorLenght4 = Math.Round(Calculator.GetDistance(0, -90, 0, 90), 0);

            var fourthEquatorLenght1 = Math.Round(Calculator.GetDistance(0, 0, 0, 90), 0);
            var fourthEquatorLenght2 = Math.Round(Calculator.GetDistance(-45, 0, 45, 0), 0);
            var fourthEquatorLenght3 = Math.Round(Calculator.GetDistance(0, -90, 0, 0), 0);
            var fourthEquatorLenght4 = Math.Round(Calculator.GetDistance(0, -45, 0, 45), 0);


            // Assert.
            Assert.AreEqual(EquatorLenght / 2, halfEquatorLenght1);
            Assert.AreEqual(EquatorLenght / 2, halfEquatorLenght2);
            Assert.AreEqual(EquatorLenght / 2, halfEquatorLenght3);
            Assert.AreEqual(EquatorLenght / 2, halfEquatorLenght4);

            Assert.AreEqual(EquatorLenght / 4, fourthEquatorLenght1);
            Assert.AreEqual(EquatorLenght / 4, fourthEquatorLenght2);
            Assert.AreEqual(EquatorLenght / 4, fourthEquatorLenght3);
            Assert.AreEqual(EquatorLenght / 4, fourthEquatorLenght4);
        }

        [TestMethod()]
        public void GetAngleTest()
        {
            // Arrange + Act.
            Calculator calc = new Calculator(0);
            var equatorAngle = Math.Round(calc.GetAngle(EquatorLenght, true), 3);
            var halfEquatorAngle = Math.Round(calc.GetAngle(EquatorLenght / 2, true), 3);
            var decimalEquatorAngle = Math.Round(calc.GetAngle(EquatorLenght / 10, true), 3);

            // Assert.
            Assert.AreEqual(360, equatorAngle);
            Assert.AreEqual(180, halfEquatorAngle);
            Assert.AreEqual(36, decimalEquatorAngle);
        }

        [TestMethod()]
        public void GetBestFreqLevelTest()
        {
            // Arrange.
            List<FreqLevelPairModel> list1 = new List<FreqLevelPairModel>()
            {
                new FreqLevelPairModel(109,-5.5),
                new FreqLevelPairModel(1023,10.123),
                new FreqLevelPairModel(123,100.6),
            };

            List<FreqLevelPairModel> list2 = new List<FreqLevelPairModel>()
            {
                new FreqLevelPairModel(10168,200.56),
                new FreqLevelPairModel(10366,300.56),
                new FreqLevelPairModel(10123,100),
            };

            List<FreqLevelPairModel> list3 = new List<FreqLevelPairModel>()
            {
                new FreqLevelPairModel(1664,-5.14654),
                new FreqLevelPairModel(1440,-10.5),
                new FreqLevelPairModel(1300,-100),
            };

            // Act.
            var best1 = Calculator.GetBestFreqLevel(list1);
            var best2 = Calculator.GetBestFreqLevel(list2);
            var best3 = Calculator.GetBestFreqLevel(list3);

            // Assert.
            Assert.AreEqual(123, best1.Freq);
            Assert.AreEqual(100.6, best1.Level);

            Assert.AreEqual(10366, best2.Freq);
            Assert.AreEqual(300.56, best2.Level);

            Assert.AreEqual(1664, best3.Freq);
            Assert.AreEqual(-5.14654, best3.Level);
        }

        [TestMethod()]
        public void GetAreaBasePointTest()
        {
            // Arrange.
            CoordinateModel basePoint1 = new CoordinateModel(53.159, 48.208);
            CoordinateModel curPoint1 = new CoordinateModel(60.159, 50.208);
            double areaSizeAngle1 = 0.5;
            CoordinateModel areaPoint1 = new CoordinateModel(60.159, 50.208);

            CoordinateModel basePoint2 = new CoordinateModel(53.159, 48.208);
            CoordinateModel curPoint2 = new CoordinateModel(60.181, 50.238);
            double areaSizeAngle2 = 0.1;
            CoordinateModel areaPoint2 = new CoordinateModel(60.159, 50.208);

            CoordinateModel basePoint3 = new CoordinateModel(-53.159, -48.208);
            CoordinateModel curPoint3 = new CoordinateModel(-60.159, -50.208);
            double areaSizeAngle3 = 0.25;
            CoordinateModel areaPoint3 = new CoordinateModel(-60.159, -50.208);

            CoordinateModel basePoint4 = new CoordinateModel(-53.159, -48.208);
            CoordinateModel curPoint4 = new CoordinateModel(-60.198, -50.238);
            double areaSizeAngle4 = 0.25;
            CoordinateModel areaPoint4 = new CoordinateModel(-60.409, -50.458);

            CoordinateModel basePoint5 = new CoordinateModel(1.002, -2.006);
            CoordinateModel curPoint5 = new CoordinateModel(1.002, -2.006);
            double areaSizeAngle5 = 0.2;
            CoordinateModel areaPoint5 = new CoordinateModel(1.002, -2.006);

            CoordinateModel basePoint6 = new CoordinateModel(1.002, -2.006);
            CoordinateModel curPoint6 = new CoordinateModel(1.000, -2.000);
            double areaSizeAngle6 = 0.2;
            CoordinateModel areaPoint6 = new CoordinateModel(0.802, -2.006);


            // Act + Assert.
            var value1 = Calculator.GetAreaBasePoint(basePoint1, curPoint1, areaSizeAngle1, areaSizeAngle1);
            Assert.AreEqual(areaPoint1, value1);

            var value2 = Calculator.GetAreaBasePoint(basePoint2, curPoint2, areaSizeAngle2, areaSizeAngle2);
            Assert.AreEqual(areaPoint2, value2);

            var value3 = Calculator.GetAreaBasePoint(basePoint3, curPoint3, areaSizeAngle3, areaSizeAngle3);
            Assert.AreEqual(areaPoint3, value3);

            var value4 = Calculator.GetAreaBasePoint(basePoint4, curPoint4, areaSizeAngle4, areaSizeAngle4);
            Assert.AreEqual(areaPoint4, value4);

            var value5 = Calculator.GetAreaBasePoint(basePoint5, curPoint5, areaSizeAngle5, areaSizeAngle5);
            Assert.AreEqual(areaPoint5, value5);

            var value6 = Calculator.GetAreaBasePoint(basePoint6, curPoint6, areaSizeAngle6, areaSizeAngle6);
            Assert.AreEqual(areaPoint6, value6);
        }

        [TestMethod()]
        public void GetFirstBasePointTest()
        {
            // Arrange.
            List<CoordinateModel> points = new List<CoordinateModel>()
            {
                new CoordinateModel(56.306662, 44.001179),
                new CoordinateModel(76.118200, 84.134923),
                new CoordinateModel(-61.217276, -91.676901),
                new CoordinateModel(-82.532072, -39.809083),
                new CoordinateModel(82.998686, -138.158621)
            };

            List<int> dims = new List<int>()
            {
                10,
                1000,
                10000,
                351,
                654
            };

            List<int> bins = new List<int>()
            {
                1,
                2,
                5,
                10,
                123,
                500,
                15000
            };

            // Act + Assert.

            var calc = new Calculator(0);

            foreach (var point in points)
            {
                foreach (var dim in dims)
                {
                    foreach (var bin in bins)
                    {
                        var basePoint = calc.GetFirstBasePoint(point, dim, bin);
                        double latitudeDelta = calc.GetLength(basePoint.Latitude - point.Latitude, false);
                        double longitudeDelta = calc.GetLength(basePoint.Longitude - point.Longitude, false);
                        Assert.AreEqual(-(((double)dim * bin) / 2 + (double)bin / 2), Math.Round(latitudeDelta, 5));
                        Assert.AreEqual(-(((double)dim * bin) / 2 + (double)bin / 2), Math.Round(longitudeDelta, 5));
                    }
                }
            }
        }
    }
}