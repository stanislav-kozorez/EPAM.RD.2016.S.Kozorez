using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IteratorExample;
using System.Diagnostics;
using System.Collections;

namespace SimpleIteratorTests
{
    [TestClass]
    public class IteratorExampleTest
    {
        [TestMethod]
        public void MoveNext_AtFirstUsage_ReturnsTrue()
        {
            var iterator = IteratorGetter.GetIterator().GetEnumerator();
            

            Assert.IsNotNull(iterator);
            Assert.AreEqual(iterator.MoveNext(), true);   
        }

        [TestMethod]
        public void Current_ReturnsTwo()
        {
            var iterator = (IEnumerator)IteratorGetter.GetIterator().GetEnumerator();

            Assert.IsNotNull(iterator);
            Assert.AreEqual(iterator.MoveNext(), true);
            Assert.AreEqual(iterator.Current, 2);
        }
    }
}
