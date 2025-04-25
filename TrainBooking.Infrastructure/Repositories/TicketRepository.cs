using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Models;
using TrainBooking.Infrastructure.Data;

namespace TrainBooking.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public TicketRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddTicketAsync(Ticket ticket)
        {
            await dbContext.Tickets.AddAsync(ticket);
        }
    }
}
