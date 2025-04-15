using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int TrainId { get; set; }

    public DateOnly DepartureDate { get; set; }

    public TimeOnly DepartureTime { get; set; }

    public DateOnly ArrivedDate { get; set; }

    public TimeOnly ArrivedTime { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Train Train { get; set; } = null!;
}
