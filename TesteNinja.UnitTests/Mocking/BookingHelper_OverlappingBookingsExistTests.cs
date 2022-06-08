using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelper_OverlappingBookingsExistTests
    {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _repository;

        private DateTime ArriveOn(int year, int month, int day) => new DateTime(year, month, day, 14, 0, 0);
        private DateTime DepartOn(int year, int month, int day) => new DateTime(year, month, day, 10, 0, 0);
        private DateTime Before(DateTime dateTime, int days = 1) => dateTime.AddDays(-days);
        private DateTime After(DateTime dateTime, int days = 1) => dateTime.AddDays(days);

        [SetUp]
        public void SetUp()
        {
            _existingBooking = new Booking
            {
                Id = 2,
                Reference = "b",
                ArrivalDate = ArriveOn(2017, 1, 15),
                DepartureDate = DepartOn(2017, 1, 20)
            };

            _repository = new Mock<IBookingRepository>();

            _repository.Setup<IQueryable<Booking>>(r => r.GetActiveBookings(1))
                .Returns(new List<Booking> { _existingBooking }.AsQueryable());
        }

        [Test]
        public void BookingStartsFinishesBeforeExistingBooking_ReturnsEmptyString()
        {
            var booking = new Booking
            {
                Id = 1,
                Reference = "a",
                ArrivalDate = Before(_existingBooking.ArrivalDate, days: 2),
                DepartureDate = Before(_existingBooking.ArrivalDate)
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);

            Assert.That(result, Is.EqualTo(String.Empty));

        }        
        
        [Test]
        public void BookingStartsFinishesInTheMiddleExistingBooking_ReturnsBookingsReference()
        {
            var booking = new Booking
            {
                Id = 1,
                Reference = "a",
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.ArrivalDate)
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));

        }

        [Test]
        public void BookingStartsBeforeFinishesAfterExistingBooking_ReturnsBookingsReference()
        {
            var booking = new Booking
            {
                Id = 1,
                Reference = "a",
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate)
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));

        }        
        
        [Test]
        public void BookingStartsMiddleFinishesMiddleExistingBooking_ReturnsBookingsReference()
        {
            var booking = new Booking
            {
                Id = 1,
                Reference = "a",
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate)
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));

        }        
        
        [Test]
        public void BookingStartsMiddleFinishesAfterExistingBooking_ReturnsBookingsReference()
        {
            var booking = new Booking
            {
                Id = 1,
                Reference = "a",
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate)
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));

        }        
        
        [Test]
        public void BookingStartAfterFinishesAfterExistingBooking_ReturnsEmpty()
        {
            var booking = new Booking
            {
                Id = 1,
                Reference = "a",
                ArrivalDate = After(_existingBooking.DepartureDate),
                DepartureDate = After(_existingBooking.DepartureDate, days: 2)
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);

            Assert.That(result, Is.Empty);

        }

        [Test]
        public void BookingsOverLapButNewBookingCancelled_ReturnsEmpty()
        {
            var booking = new Booking
            {
                Id = 1,
                Reference = "a",
                Status = "Cancelled",
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate)
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);

            Assert.That(result, Is.EqualTo(String.Empty));

        }

    }
}
