using Microsoft.EntityFrameworkCore;
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
    public class TripRepository : ITripRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public TripRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddTripAsync(Trip trip)
        {
            await dbContext.Trips.AddAsync(trip);
        }

        public async Task<Trip?> GetTripByTickedId(int ticketId)
        {
            var tripId = await dbContext.Tickets
                 .Where(t => t.TicketId == ticketId)
                 .Select(t => t.TripId)
                 .FirstOrDefaultAsync();

            if (tripId == 0)
                return null;

            return await dbContext.Trips.FindAsync(tripId);
        }
    }

}
