namespace Domain.Entities;

public sealed class Table
{
    public Table(int number, int seats)
    {
        if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number));
        if (seats <= 0) throw new ArgumentOutOfRangeException(nameof(seats));

        Number = number;
        Seats = seats;
    }

    public int Number { get; }
    public int Seats { get; }
}
