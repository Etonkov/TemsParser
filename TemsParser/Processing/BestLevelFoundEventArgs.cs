using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing;

namespace TemsParser.Processing
{
    public struct BestLevelFoundEventArgs
    {
        public BestLevelFoundEventArgs(CoordinateModel coordinate,
                                       TechnologyListItemModel technology,
                                       OperatorListItemModel oper,
                                       FreqLevelPairModel freqLevelPair)
        {
            Coordinate = coordinate;
            Technology = technology;
            Operator = oper;
            FreqLevelPair = freqLevelPair;
        }

        public CoordinateModel Coordinate { get; private set; }

        public TechnologyListItemModel Technology { get; private set; }

        public OperatorListItemModel Operator { get; private set; }

        public FreqLevelPairModel FreqLevelPair { get; private set; }
    }
}
