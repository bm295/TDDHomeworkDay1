using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop
{
    public class FnbConfiguration
    {
        public const int MinimumSeats = 80;
        public const int MaximumSeats = 120;

        public FnbConfiguration(int totalSeats)
        {
            if (totalSeats < MinimumSeats || totalSeats > MaximumSeats)
            {
                throw new ArgumentOutOfRangeException(nameof(totalSeats), $"El Gaucho Hanoi supports {MinimumSeats}-{MaximumSeats} seats.");
            }

            TotalSeats = totalSeats;
        }

        public int TotalSeats { get; }
    }

    public class Table
    {
        public Table(int tableNumber, int seats)
        {
            if (tableNumber <= 0) throw new ArgumentOutOfRangeException(nameof(tableNumber));
            if (seats <= 0) throw new ArgumentOutOfRangeException(nameof(seats));

            TableNumber = tableNumber;
            Seats = seats;
        }

        public int TableNumber { get; }
        public int Seats { get; }
    }

    public class Reservation
    {
        public Reservation(string bookingCode, int partySize)
        {
            if (string.IsNullOrWhiteSpace(bookingCode)) throw new ArgumentException("Booking code is required.", nameof(bookingCode));
            if (partySize <= 0) throw new ArgumentOutOfRangeException(nameof(partySize));

            BookingCode = bookingCode.Trim();
            PartySize = partySize;
        }

        public string BookingCode { get; }
        public int PartySize { get; }
    }

    public class BillItem
    {
        public BillItem(string name, int quantity, decimal unitPrice)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (unitPrice < 0) throw new ArgumentOutOfRangeException(nameof(unitPrice));

            Name = name.Trim();
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public string Name { get; }
        public int Quantity { get; }
        public decimal UnitPrice { get; }
        public decimal Subtotal => Quantity * UnitPrice;
    }

    public class BillSummary
    {
        public BillSummary(decimal subTotal, decimal serviceChargeRate, decimal vatRate)
        {
            if (serviceChargeRate < 0) throw new ArgumentOutOfRangeException(nameof(serviceChargeRate));
            if (vatRate < 0) throw new ArgumentOutOfRangeException(nameof(vatRate));

            SubTotal = decimal.Round(subTotal, 2, MidpointRounding.AwayFromZero);
            ServiceCharge = decimal.Round(SubTotal * serviceChargeRate, 2, MidpointRounding.AwayFromZero);
            Vat = decimal.Round((SubTotal + ServiceCharge) * vatRate, 2, MidpointRounding.AwayFromZero);
            GrandTotal = SubTotal + ServiceCharge + Vat;
        }

        public decimal SubTotal { get; }
        public decimal ServiceCharge { get; }
        public decimal Vat { get; }
        public decimal GrandTotal { get; }
    }

    public class FnbManagementService
    {
        private readonly FnbConfiguration _configuration;
        private readonly List<Table> _tables = new List<Table>();
        private readonly Dictionary<string, int> _activeReservations = new Dictionary<string, int>();

        public FnbManagementService(FnbConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IReadOnlyCollection<Table> Tables => _tables.AsReadOnly();
        public int ReservedSeats => _activeReservations.Values.Sum();
        public int AvailableSeats => _configuration.TotalSeats - ReservedSeats;

        public void ConfigureTables(IEnumerable<Table> tables)
        {
            if (tables == null) throw new ArgumentNullException(nameof(tables));

            var materializedTables = tables.ToList();
            var totalSeatsFromTables = materializedTables.Sum(t => t.Seats);
            if (totalSeatsFromTables > _configuration.TotalSeats)
            {
                throw new InvalidOperationException("Total table seats exceed restaurant capacity.");
            }

            if (materializedTables.Select(t => t.TableNumber).Distinct().Count() != materializedTables.Count)
            {
                throw new InvalidOperationException("Table numbers must be unique.");
            }

            _tables.Clear();
            _tables.AddRange(materializedTables);
        }

        public bool TryCreateReservation(Reservation reservation)
        {
            if (reservation == null) throw new ArgumentNullException(nameof(reservation));
            if (_activeReservations.ContainsKey(reservation.BookingCode)) return false;
            if (reservation.PartySize > AvailableSeats) return false;

            _activeReservations[reservation.BookingCode] = reservation.PartySize;
            return true;
        }

        public bool CancelReservation(string bookingCode)
        {
            if (string.IsNullOrWhiteSpace(bookingCode)) return false;
            return _activeReservations.Remove(bookingCode.Trim());
        }

        public BillSummary GenerateBill(IEnumerable<BillItem> items, decimal serviceChargeRate = 0.05m, decimal vatRate = 0.10m)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var subTotal = items.Sum(i => i.Subtotal);
            return new BillSummary(subTotal, serviceChargeRate, vatRate);
        }
    }
}
