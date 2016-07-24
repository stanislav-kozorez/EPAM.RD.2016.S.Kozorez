using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;

namespace UserStorageSystemTests
{
    [TestClass]
    public class DefaultGeneratorTest
    {
        [TestMethod]
        public void RestoreGeneratorState_WithParamTen_ReturnsTenInCurrent()
        {
            var generator = new DefaultGenerator();
            generator.RestoreGeneratorState("11");
            Assert.AreEqual(11, generator.Current);
        }

        [TestMethod]
        public void Current_AfterGeneratorInitialization_ReturnsOne()
        {
            var generator = new DefaultGenerator();
            Assert.AreEqual(1, generator.Current);
        }

        [TestMethod]
        public void MoveNext_AfterGeneratorInitialization_ReturnsTwo()
        {
            var generator = new DefaultGenerator();
            generator.MoveNext();          
            Assert.AreEqual(2, generator.Current);
        }

        [TestMethod]
        public void Reset_CurrentReturnsOne()
        {
            var generator = new DefaultGenerator();
            generator.MoveNext();
            generator.MoveNext();
            generator.MoveNext();
            generator.MoveNext();
            generator.Reset();
            Assert.AreEqual(1, generator.Current);
        }
    }
}
