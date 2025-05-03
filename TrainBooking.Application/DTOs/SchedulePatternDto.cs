using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Application.DTOs
{
    public class SchedulePatternDto
    {
        public int TrainId { get; set; }
        public string TrainNumber { get; set; }
        public string FrequencyType { get; set; } = null!;

        public string? DaysOfWeek { get; set; }

        public string? DayParity { get; set; }
    }
}
