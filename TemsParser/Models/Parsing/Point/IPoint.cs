using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Models.Parsing.Point
{
    public interface IPoint
    {
        void AddValue(FreqLevelPairModel value);

        void Initialize();

        bool GetValue(out FreqLevelPairModel freqLevelPair);
    }
}
