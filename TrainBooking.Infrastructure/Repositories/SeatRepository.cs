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

        public async Task<IEnumerable<Seat>> GetAvailableSeatsByCarriageAndScheduleAsync(int carriageId, int scheduleId)
        {
            // 1. Витягуємо всі зайняті SeatId для заданого ScheduleId
            var bookedSeatIds = await dbContext.Tickets
                .Where(t => t.Trip.ScheduleId == scheduleId)
                .Select(t => t.SeatId)
                .ToListAsync();

            // 2. Витягуємо всі місця у вказаному вагоні, які ще не зайняті
            var seats = await dbContext.Seats
                .Where(s => s.CarriageId == carriageId && !bookedSeatIds.Contains(s.SeatId))
                .ToListAsync();

            return seats;
        }

    }
}
