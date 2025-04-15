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
    public class SeatRepository : ISeatRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public SeatRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Seat>> GetByCarriageIdAsync(int carriageId)
        {
            return await dbContext.Seats.Where(s => s.CarriageId == carriageId).ToListAsync();
        }
    }
}
