using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestClass]
    public class ReservationTests
    {
        //method_scenario_expectedBehavior
        //AAA = arrange, act and assert

        [TestMethod]
        public void CanBeCancelledBy_UserIsAdmin_ReturnsTrue()
        {
            //arrange
            var reservation = new Reservation();

            //act
            var result = reservation.CanBeCancelledBy(new User { IsAdmin = true });

            //assert
            Assert.IsTrue(result);
        }        
        
        [TestMethod]
        public void CanBeCancelledBy_UserIsMadeBy_ReturnsTrue()
        {
            //arrange
            var reservation = new Reservation();
            var user = new User();

            reservation.MadeBy = user;

            //act
            var result = reservation.CanBeCancelledBy(user);

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanBeCancelledBy_UserIsMadeBy_ReturnsFalse()
        {
            //arrange
            var reservation = new Reservation();
            var user = new User();

            //act
            var result = reservation.CanBeCancelledBy(user);

            //assert
            Assert.IsFalse(result);
        }

    }
}