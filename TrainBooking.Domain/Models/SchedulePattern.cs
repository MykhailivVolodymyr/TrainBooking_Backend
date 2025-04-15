using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class SchedulePattern
{
    public int PatternId { get; set; }

    public int TrainId { get; set; }

    public string FrequencyType { get; set; } = null!;

    public string? DaysOfWeek { get; set; }

    public string? DayParity { get; set; }

    public virtual Train Train { get; set; } = null!;
}
