using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int CarriageId { get; set; }

    public int SeatNumber { get; set; }

    public string SeatType { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public virtual Carriage Carriage { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
