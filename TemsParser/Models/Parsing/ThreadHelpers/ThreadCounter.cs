using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.Parsing.ThreadHelpers
{
    public class ThreadCounter
    {
        private readonly Object LockObj = new Object();
        private int _treadsInWork;
        private bool _isInitialized;
        public event EventHandler AllThreadsIsFinished;


        public ThreadCounter()
        {
            _isInitialized = false;
        }

        public void DecreaseTreadsInWork()
        {
            lock (LockObj)
            {
                _treadsInWork--;

                if ((_treadsInWork == 0) && _isInitialized)
                {
                    OnAllThreadsIsFinished();
                }
            }
        }

        public void InitializeTreadsInWork(int initialValue)
        {
            lock (LockObj)
            {
                _treadsInWork = initialValue;
                _isInitialized = true;
            }
        }

        private void OnAllThreadsIsFinished()
        {
            var handler = AllThreadsIsFinished;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
    }
}
