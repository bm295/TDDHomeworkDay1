using System;
using System.Collections.Generic;
using Shop;
using Xunit;

namespace UnitTest
{
    public class FnbManagementServiceTests
    {
        [Theory]
        [InlineData(79)]
        [InlineData(121)]
        public void Configuration_Outside_80_120_Seats_ShouldThrow(int totalSeats)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FnbConfiguration(totalSeats));
        }

        [Fact]
        public void ConfigureTables_WhenSeatCountExceedsCapacity_ShouldThrow()
        {
            var service = new FnbManagementService(new FnbConfiguration(100));
            var tables = new List<Table>
            {
                new Table(1, 60),
                new Table(2, 50)
            };

            Assert.Throws<InvalidOperationException>(() => service.ConfigureTables(tables));
        }

        [Fact]
        public void TryCreateReservation_ShouldTrackAvailability_For80To120SeatRestaurant()
        {
            var service = new FnbManagementService(new FnbConfiguration(100));

            Assert.True(service.TryCreateReservation(new Reservation("EGH001", 40)));
            Assert.True(service.TryCreateReservation(new Reservation("EGH002", 35)));
            Assert.False(service.TryCreateReservation(new Reservation("EGH003", 30)));

            Assert.Equal(75, service.ReservedSeats);
            Assert.Equal(25, service.AvailableSeats);
        }

        [Fact]
        public void CancelReservation_ShouldReleaseSeats()
        {
            var service = new FnbManagementService(new FnbConfiguration(80));
            service.TryCreateReservation(new Reservation("EGH001", 50));
            service.TryCreateReservation(new Reservation("EGH002", 20));

            var removed = service.CancelReservation("EGH001");

            Assert.True(removed);
            Assert.Equal(20, service.ReservedSeats);
            Assert.Equal(60, service.AvailableSeats);
        }

        [Fact]
        public void GenerateBill_ShouldCalculateSubTotal_ServiceCharge_Vat_GrandTotal()
        {
            var service = new FnbManagementService(new FnbConfiguration(100));
            var items = new List<BillItem>
            {
                new BillItem("Ribeye", 2, 950000m),
                new BillItem("Wine", 1, 1200000m)
            };

            var summary = service.GenerateBill(items);

            Assert.Equal(3100000m, summary.SubTotal);
            Assert.Equal(155000m, summary.ServiceCharge);
            Assert.Equal(325500m, summary.Vat);
            Assert.Equal(3580500m, summary.GrandTotal);
        }
    }
}
