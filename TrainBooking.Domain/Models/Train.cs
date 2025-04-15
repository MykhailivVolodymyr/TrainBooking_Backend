using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Train
{
    public int TrainId { get; set; }

    public string Number { get; set; } = null!;

    public string? Type { get; set; }

    public int? RouteId { get; set; }

    public virtual ICollection<Carriage> Carriages { get; set; } = new List<Carriage>();

    public virtual Route? Route { get; set; }

    public virtual ICollection<SchedulePattern> SchedulePatterns { get; set; } = new List<SchedulePattern>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
