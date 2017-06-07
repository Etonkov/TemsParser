using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.Parsing
{
    public struct FreqLevelPairModel
    {
        public FreqLevelPairModel(int freq, double level)
        {
            Freq = freq;
            Level = level;
        }

        public FreqLevelPairModel(double level)
        {
            Freq = Int32.MinValue;
            Level = level;
        }

        public int Freq { get; private set; }

        public double Level { get; private set; }

        public override string ToString()
        {
            if (Freq == Int32.MinValue)
            {
                return Math.Round(Level, 2).ToString(CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                return Math.Round(Level, 2).ToString(CultureInfo.InvariantCulture.NumberFormat) +  "\t" + Freq;
            }
        }

        public string ToString(bool withFreq)
        {
            if (withFreq)
            {
                return Math.Round(Level, 2).ToString(CultureInfo.InvariantCulture.NumberFormat) + "\t" + Freq;
            }
            else
            {
                return Math.Round(Level, 2).ToString(CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}
