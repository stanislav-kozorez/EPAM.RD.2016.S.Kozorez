using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IteratorExample;
using System.Collections;

namespace SimpleIteratorTests
{
    [TestClass]
    public class IteratorGetterTest
    {
        [TestMethod]
        public void GetIterator_ReturnsNotNullIteratorInstance()
        {
            var iterator = IteratorGetter.GetIterator();

            Assert.IsNotNull(iterator);
        }
    }
}
