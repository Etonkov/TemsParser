using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Models.TemsFileInfo
{
    public class FreqPoolModel
    {
        public FreqPoolModel(TechnologyListItemModel tech, OperatorListItemModel oper, IEnumerable<int> freqs)
        {
            this.Technology = tech;
            this.Operator = oper;
            this.Freqs = freqs;
        }


        public TechnologyListItemModel Technology { get; private set; }

        public OperatorListItemModel Operator { get; private set; }

        public IEnumerable<int> Freqs { get; private set; }
    }
}
