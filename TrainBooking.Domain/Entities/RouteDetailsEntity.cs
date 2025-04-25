using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Domain.Entities
{
    public class RouteDetailsEntity
    {
        public required string StationName { get; set; }
        public required string City { get; set; }
        public required int StationOrder { get; set; }
        public TimeOnly? ArrivalTime { get; set; }
        public TimeOnly? DepartureTime { get; set; }
    }
    
}
