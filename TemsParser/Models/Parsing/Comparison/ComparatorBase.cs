using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Processing;

namespace TemsParser.Models.Parsing.Comparison
{
    public abstract class ComparatorBase
    {
        protected readonly string DirectoryBase;

        public ComparatorBase(string directoryBase)
        {
            DirectoryBase = directoryBase;
        }

        public abstract void AddValue(object s, BestLevelFoundEventArgs ea);

        public abstract int Save();
    }
}
