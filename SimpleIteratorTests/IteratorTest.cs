using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IteratorExample;
using System.Diagnostics;

namespace SimpleIteratorTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IteratorExampleTest()
        {
            var iterator = new IteratorExample.IteratorExample();

            foreach (var number in iterator)
                Debug.Write($"{number} ");          
        }
    }
}
