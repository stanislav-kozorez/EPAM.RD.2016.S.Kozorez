using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sequence;

namespace Sequence.Tests
{
    [TestClass]
    public class SequenceTest
    {
        [TestMethod]
        public void GetSequence_LimitIsZero_ReturnAnEmptyArray()
        {
            //Arrange
            //Act
            var result = Sequence.GetSequence(0);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSequence_LimitIsMinusOne_ThrowAnException()
        {
            var result = Sequence.GetSequence(-1);
        }

        [TestMethod]
        public void GetSequence_LimitIsOne_ReturnsAnArrayWithOneElement()
        {
            var result = Sequence.GetSequence(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0]);
        }
    }
}
