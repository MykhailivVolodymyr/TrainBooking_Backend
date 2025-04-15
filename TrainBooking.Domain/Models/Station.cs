using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Station
{
    public int StationId { get; set; }

    public string StationName { get; set; } = null!;

    public string City { get; set; } = null!;

    public virtual ICollection<Route> RouteEndStations { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteStartStations { get; set; } = new List<Route>();

    public virtual ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();

    public virtual ICollection<Trip> TripEndStations { get; set; } = new List<Trip>();

    public virtual ICollection<Trip> TripStartStations { get; set; } = new List<Trip>();
}
