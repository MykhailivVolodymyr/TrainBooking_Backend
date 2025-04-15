using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class RouteStation
{
    public int RouteId { get; set; }

    public int StationId { get; set; }

    public int StationOrder { get; set; }

    public TimeOnly ArrivesAt { get; set; }

    public TimeOnly DepartsAt { get; set; }

    public virtual Route Route { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
