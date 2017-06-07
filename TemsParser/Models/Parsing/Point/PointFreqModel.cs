using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Models.Parsing.Point
{
    [Serializable]
    public struct PointFreqModel: IPoint
    {
        private Object _lockObj;

        private double _sumLevel;

        private double _bestLevel;

        private int _bestFreq;

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
                    _bestLevel = value.Level;
                    _bestFreq = value.Freq;
                }
                else
                {
                    _sumLevel += value.Level;

                    if (value.Level > _bestLevel)
                    {
                        _bestLevel = value.Level;
                        _bestFreq = value.Freq;
                    }
                }

                _amount++;
            }
        }


        public bool GetValue(out FreqLevelPairModel result)
        {
            if (_amount == 0)
            {
                result = new FreqLevelPairModel(Int32.MinValue, Double.NaN);
                return false;
            }
            else
            {
                result = new FreqLevelPairModel(_bestFreq, _sumLevel / _amount);
                return true;
            }
        }
    }
}
