using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Carriage
{
    public int CarriageId { get; set; }

    public int? TrainId { get; set; }

    public string CarriageType { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual Train? Train { get; set; }
}
