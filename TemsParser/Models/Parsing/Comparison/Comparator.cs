using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing.Point;
using TemsParser.Processing;

namespace TemsParser.Models.Parsing.Comparison
{
    public class Comparator : ComparatorBase
    {
        private readonly object ThisLockObj = new Object();

        private readonly Dictionary<TechnologyListItemModel,
            Dictionary<OperatorListItemModel, Dictionary<Thread, ComparatorItem>>> Storage;


        public Comparator(IEnumerable<TechnologyListItemModel> techList,
                          IEnumerable<OperatorListItemModel> operList,
                          string directoryBase) : base(directoryBase)
        {
            Storage = new Dictionary<TechnologyListItemModel,
                Dictionary<OperatorListItemModel, Dictionary<Thread, ComparatorItem>>>();

            foreach (var techItem in techList)
            {
                var storageValue = new Dictionary<OperatorListItemModel, Dictionary<Thread, ComparatorItem>>();

                foreach (var operItem in operList)
                {
                    storageValue.Add(operItem, new Dictionary<Thread, ComparatorItem>());
                }

                Storage.Add(techItem, storageValue);
            }
        }

        public override void AddValue(object s, BestLevelFoundEventArgs ea)
        {
            var storageValue = Storage[ea.Technology];
            var operPartValue = storageValue[ea.Operator];
            var currentThread = Thread.CurrentThread;
            ComparatorItem comparator;

            if (operPartValue.TryGetValue(currentThread, out comparator) == false)
            {
                lock (ThisLockObj)
                {
                    if (operPartValue.TryGetValue(currentThread, out comparator) == false)
                    {
                        comparator = new ComparatorItem();
                    }

                    operPartValue.Add(currentThread, comparator);
                }
            };

            comparator.AddValue(ea.FreqLevelPair);
        }

        public override int Save()
        {
            int result = 0;

            foreach (var storageKvp in Storage)
            {
                if (storageKvp.Value.Count == 0)
                {
                    continue;
                }


                var valuesCount = storageKvp.Value.SelectMany(o => o.Value.Values).Where(o => (o.Amount > 0)).Count();

                if (valuesCount > 0)
                {
                    string filePath = DirectoryBase + "_" + storageKvp.Key.Name + "_COMPARISON.txt";

                    using (var writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("Operator\tValue");

                        foreach (var operItem in storageKvp.Value)
                        {
                            double sumLevel = 0;
                            long amount = 0;

                            foreach (var comparatorItem in operItem.Value)
                            {
                                sumLevel += comparatorItem.Value.SumLevel;
                                amount += comparatorItem.Value.Amount;
                            }

                            double averageLevel = sumLevel / amount;
                            var sLevel = Math.Round(averageLevel, 2).ToString(CultureInfo.InvariantCulture.NumberFormat);
                            writer.WriteLine(operItem.Key.Name + "\t" + sLevel);
                        }

                        result++;
                    }
                }
            }

            return result;
        }
    }
}
