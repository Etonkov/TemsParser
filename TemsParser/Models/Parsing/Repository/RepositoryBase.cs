using Ninject;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TemsParser.Common;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing.Area;
using TemsParser.Models.Parsing.Point;
using TemsParser.Processing;

namespace TemsParser.Models.Parsing.Repository
{
    public abstract class RepositoryBase
    {
        protected readonly string DirectoryBase;

        public RepositoryBase(string directoryBase)
        {
            DirectoryBase = directoryBase;
        }

        public abstract void AddValue(Object s, BestLevelFoundEventArgs ea);

        public abstract int Save();

        public static string GetHeader(bool withFreq,OperatorListItemModel oper)
        {
            var localThis = new AreaItemModel();
            var columnNames = new List<string>();
            columnNames.Add(NotifyPropertyChanged.GetPropertyName(() => localThis.Latitude));
            columnNames.Add(NotifyPropertyChanged.GetPropertyName(() => localThis.Longitude));
            columnNames.Add(oper.ToString());

            if (withFreq)
            {
                columnNames.Add(NotifyPropertyChanged.GetPropertyName(() => localThis.Freq));
            }

            return String.Join("\t", columnNames);
        }
    }
}
