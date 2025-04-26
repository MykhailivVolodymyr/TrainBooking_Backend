using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Domain.Entities
{
    public class TicketEntity
    {
        public int TicketId { get; set; }
        public string FullName { get; set; }
        public string TrainNumber { get; set; }
        public int CarriageNumber { get; set; }
        public string CarriageType { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }

        public string DepartureStation { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalStation { get; set; }
        public string ArrivalCity { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public decimal TicketPrice { get; set; }
    }
}
