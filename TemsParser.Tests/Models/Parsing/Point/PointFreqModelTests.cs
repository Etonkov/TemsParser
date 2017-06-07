using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Parsing;
using TemsParser.Models.Parsing.Point;



namespace TemsParser.Tests.Models.Parsing.Point
{
    [TestClass]
    public class PointFreqModelTests
    {
        [TestMethod]
        public void AddValueTest()
        {
            // Arrange.
            var testingData = new List<List<double>>()
            {
                new List<double>() { 100, 10, 100, 10 },
                new List<double>() { 100 },
                new List<double>() { 0 },
                new List<double>() { -100 },
                new List<double>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                new List<double>() { 0.00001, 1.2316745645, -12315.9578652 },
                new List<double>() { 0.000232401, 1.223167445, -12315.9578652 },
                new List<double>() { 0.00001, 200.231656466, -125.957865135 }

            };

            Random random = new Random();

            for (int i = 10; i <= 1000; i += 10)
            {
                List<double> item = new List<double>();

                for (int j = 0; j < i; j++)
                {
                    item.Add(random.Next());
                }
            }

            double maxDiviation = 0;

            foreach (var testingItem in testingData)
            {
                var pointFreqModel = new PointFreqModel();
                pointFreqModel.Initialize();
                double summ = 0;
                int amount = 0;

                foreach (var valueItem in testingItem)
                {
                    amount++;
                    summ += valueItem;
                    pointFreqModel.AddValue(new FreqLevelPairModel(valueItem));
                }

                var average = summ / amount;
                FreqLevelPairModel actual;
                pointFreqModel.GetValue(out actual);
                Assert.AreEqual(Math.Round(average, 10), Math.Round(actual.Level, 10));
                var diviation = Math.Abs(actual.Level - average);

                if (diviation > maxDiviation)
                {
                    maxDiviation = diviation;
                }
            }
        }
    }
}
