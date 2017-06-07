using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing;

namespace TemsParser.Processing
{
    public class TemsFileParserFinishedEventArgs: EventArgs
    {
        public TemsFileParserFinishedEventArgs(TemsFileParser parser)
        {
            Parser = parser;
        }

        public TemsFileParser Parser { get; private set; }
    }
}
