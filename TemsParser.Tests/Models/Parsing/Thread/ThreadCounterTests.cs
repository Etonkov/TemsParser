using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemsParser.Models.Parsing.ThreadHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.Parsing.ThreadHelpers.Tests
{
    [TestClass()]
    public class ThreadCounterTests
    {
        private bool _allThreadsIsFinished = false;

        //private bool AllThreadsIsFinished
        //{
        //    get
        //    {
        //        lock (this)
        //        {
        //            return _allThreadsIsFinished;
        //        }
        //    }
        //    set
        //    {
        //        lock (this)
        //        {
        //            _allThreadsIsFinished = value;
        //        }
        //    }
        //}

        [TestMethod()]
        public void ThreadCounterTest()
        {
            // Arrange.
            ThreadCounter tc = new ThreadCounter();
            tc.AllThreadsIsFinished += Tc_AllThreadsIsFinished;
            tc.InitializeTreadsInWork(1000000);

            // Act + assert;
            // Parallel test.
            Parallel.For(0, 999999, i =>
            {
                tc.DecreaseTreadsInWork();

                Assert.IsFalse(_allThreadsIsFinished);
            });

            tc.DecreaseTreadsInWork();
            Assert.IsTrue(_allThreadsIsFinished);
        }

        private void Tc_AllThreadsIsFinished(object sender, EventArgs e)
        {
            lock (this)
            {
                _allThreadsIsFinished = true;
            }
        }
    }
}