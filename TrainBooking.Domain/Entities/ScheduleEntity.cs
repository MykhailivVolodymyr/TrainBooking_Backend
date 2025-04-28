using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Domain.Entities
{
    public class ScheduleEntity
    {
        public required int ScheduleId { get; set; }
        public required int TrainId { get; set; }
        public required string TrainNumber { get; set; }
        public string RouteCities { get; set; }
        public int RouteId { get; set; }
        public int StationCount { get; set; }
        public DateOnly RealDepartureDateFromCity { get; set; }
        public DateOnly DepartureDateFromStart { get; set; }
        public string FromStationName { get; set; }
        public required TimeOnly ArrivalTimeFromCity { get; set; }
        public string ToStationName { get; set; }
        public DateOnly ArrivalDateToEnd { get; set; }
        public required DateOnly RealDepartureDateToCity { get; set; }
        public required TimeOnly ArrivalTimeToCity { get; set; }
    }

}
