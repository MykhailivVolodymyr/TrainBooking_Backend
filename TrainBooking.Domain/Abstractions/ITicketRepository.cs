using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;

namespace TrainBooking.Domain.Abstractions
{
    public interface ITicketRepository
    {
        Task AddTicketAsync(Ticket ticket);
        Task ReturnTicketAsync(int ticketId);
    }
}
