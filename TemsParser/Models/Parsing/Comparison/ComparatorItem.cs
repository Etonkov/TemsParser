using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.Parsing.Comparison
{
    public class ComparatorItem
    {
        public double SumLevel { get; private set; }

        public long Amount { get; private set; }

        public void AddValue(FreqLevelPairModel value)
        {
            if (Amount == 0)
            {
                SumLevel = value.Level;
            }
            else
            {
                SumLevel += value.Level;
            }

            Amount++;
        }
    }
}
