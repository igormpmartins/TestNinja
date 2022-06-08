using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class DemeritPointsCalculatorTests
    {
        private DemeritPointsCalculator _calculator;

        [SetUp]
        public void SetUp() => _calculator = new DemeritPointsCalculator();

        [Test]
        [TestCase(-1)]
        [TestCase(500)]
        public void CalculateDemeritPoints_InvalidSpeed_ThrowException(int invalidSpeed)
        {
            Assert.That(() => _calculator.CalculateDemeritPoints(invalidSpeed), 
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(64, 0)]
        [TestCase(65, 0)]
        [TestCase(66, 0)]
        [TestCase(70, 1)]
        [TestCase(72, 1)]
        [TestCase(75, 2)]
        public void CalculateDemeritPoints_WhenCalled_ReturnPoints(int speed, int expectedPoints)
        {
            var result = _calculator.CalculateDemeritPoints(speed);
            Assert.That(result, Is.EqualTo(expectedPoints));
        }
    }
}
