using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;
using TrainBooking.Infrastructure.Data;

namespace TrainBooking.Infrastructure.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public RouteRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<RouteDetailsEntity>> GetRouteDetailsByTrainNumberAsync(string trainNumber)
        {
            return await dbContext.Trains
                .AsNoTracking()
                .Where(t => t.Number == trainNumber)
                .SelectMany(t => t.Route.RouteStations)
                .OrderBy(rs => rs.StationOrder)
                .Select(rs => new RouteDetailsEntity
                {
                    StationName = rs.Station.StationName,
                    City = rs.Station.City,
                    StationOrder = rs.StationOrder,
                    ArrivalTime = rs.ArrivesAt,
                    DepartureTime = rs.DepartsAt
                })
                .ToListAsync();
        }


    }
}
