using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Application.DTOs
{
    public class TripDto
    {
        public int TrainId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string StartStationName { get; set; }
        public string EndStationName { get; set; }
        public int SheduleId { get; set; }

    }
}
