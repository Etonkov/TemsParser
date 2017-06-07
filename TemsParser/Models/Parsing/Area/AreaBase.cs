using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.IO;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing.Point;
using TemsParser.Processing;

namespace TemsParser.Models.Parsing.Area
{
    public abstract class AreaBase
    {
        public const int Dimentions = 100;

        private readonly TechnologyListItemModel TechnologyItem;

        private readonly OperatorListItemModel OperatorItem;

        //public event EventHandler MemoryAllocated;


        public AreaBase(TechnologyListItemModel technology,
                        OperatorListItemModel oper)
        {
            OperatorItem = oper;
            TechnologyItem = technology;
        }


        //public DateTime CallTime { get; private set; }

        public bool InSavingMode { get; protected set; }

        public double MinLatitude { get; protected set; }

        public double MinLongitude { get; protected set; }

        public double MaxLatitude { get; protected set; }

        public double MaxLongitude { get; protected set; }

        protected string Path
        {
            get
            {
                return String.Format(@"\Area\{0}\{1}\{2}_{3}.dat",
                                     TechnologyItem.Name,
                                     OperatorItem.Name,
                                     MinLatitude.ToString(CultureInfo.InvariantCulture.NumberFormat).Replace('.','x'),
                                     MinLongitude);
            }
        }


        //protected void UpdateCallTime()
        //{
        //    CallTime = DateTime.Now;
        //}

        //protected abstract void SavingModeOff();

        //protected void OnMemoryAllocated()
        //{
        //    var handler = MemoryAllocated;

        //    if (handler != null)
        //    {
        //        handler(this, new EventArgs());
        //    }
        //}

        //public abstract void SavingModeOn();

        public abstract void AddValue(CoordinateModel coordinate, FreqLevelPairModel freqLevelPair);

        public abstract List<string> GetResult();
    }
}
