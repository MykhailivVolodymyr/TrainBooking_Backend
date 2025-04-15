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
    public class TrainRepository : ITrainRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public TrainRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Train?> GetByNumberAsync(string trainNumber)
        {
           return await dbContext.Trains.FirstOrDefaultAsync(t => t.Number == trainNumber);
        }
    }
}
