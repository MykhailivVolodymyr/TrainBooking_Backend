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
    public class CarriageRepository : ICarriageRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public CarriageRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Carriage>> GetByTrainIdAsync(int trainId)
        {
           return await dbContext.Carriages.Where(c => c.TrainId == trainId).ToListAsync();
        }
    }
}
