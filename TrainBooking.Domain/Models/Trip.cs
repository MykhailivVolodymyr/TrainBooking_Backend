using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Trip
{
    public int TripId { get; set; }

    public int? TrainId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public int StartStationId { get; set; }

    public int EndStationId { get; set; }

    public virtual Station EndStation { get; set; } = null!;

    public virtual Station StartStation { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Train? Train { get; set; }
}
