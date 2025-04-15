using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Route
{
    public int RouteId { get; set; }

    public int StartStationId { get; set; }

    public int EndStationId { get; set; }

    public string? VariantName { get; set; }

    public virtual Station EndStation { get; set; } = null!;

    public virtual ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();

    public virtual Station StartStation { get; set; } = null!;

    public virtual ICollection<Train> Trains { get; set; } = new List<Train>();
}
