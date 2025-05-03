using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Models;
using TrainBooking.Infrastructure.Data;

namespace TrainBooking.Infrastructure.Repositories
{
    public class SchedulePatternRepository : ISchedulePatternRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public SchedulePatternRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SchedulePattern>> GetAll()
        {
            return await dbContext.SchedulePatterns.Include(sp => sp.Train).ToListAsync();
        }

        public async Task<SchedulePattern> GetScheduleByTrainIdAsync(int trainId)
        {
            return await dbContext.SchedulePatterns
                .Where(sp => sp.TrainId == trainId)
                .FirstAsync();
        }

        public async Task UpdateScheduleAsync(int trainId, SchedulePattern schedule)
        {
           var SchPattern = await GetScheduleByTrainIdAsync(trainId);


           SchPattern.FrequencyType = schedule.FrequencyType ?? SchPattern.FrequencyType;
           SchPattern.DayParity = schedule.DayParity;
           SchPattern.DaysOfWeek = schedule.DaysOfWeek;

          await dbContext.SaveChangesAsync();
        }
    }
}
