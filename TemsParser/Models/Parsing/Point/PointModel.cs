using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Models.Parsing.Point
{
    [Serializable]
    public struct PointModel: IPoint
    {
        private Object _lockObj;

        private double _sumLevel;

        private int _amount;


        public void Initialize()
        {
            _lockObj = new Object();
        }

        public void AddValue(FreqLevelPairModel value)
        {
            lock (_lockObj)
            {
                if (_amount == 0)
                {
                    _sumLevel = value.Level;
                }
                else
                {
                    _sumLevel += value.Level;
                }

                _amount++;

            }
        }

        public bool GetValue(out FreqLevelPairModel result)
        {
            if (_amount == 0)
            {
                result = new FreqLevelPairModel(Int32.MinValue, double.NaN);
                return false;
            }
            else
            {
                result = new FreqLevelPairModel(Int32.MinValue, _sumLevel / _amount);
                return true;
            }
        }
    }
}
