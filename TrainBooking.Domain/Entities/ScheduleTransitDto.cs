using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Domain.Entities
{
    public class ScheduleTransitDto
    {
        public required string TrainNumber { get; set; }
        public required string RouteCities { get; set; }
        public required TimeOnly Time { get; set; }
        public required string StationName { get; set; }
    }

}
