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
    public class StationRepository : IStationRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public StationRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Station?> GetStationByName(string stationName)
        {
            return await dbContext.Stations.FirstOrDefaultAsync(st => st.StationName == stationName);
        }
    }
}
