using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Application.DTOs
{
    public class TicketCreateDto
    {
        public int SeatId { get; set; }
        public decimal Price { get; set; }
    }

}
