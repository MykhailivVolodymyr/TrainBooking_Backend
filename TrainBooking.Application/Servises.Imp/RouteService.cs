using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises.Imp
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }
        public async Task<IEnumerable<RouteDetailsEntity>> GetRouteDetailsByTrainNumberAsync(string trainNumber)
        {
            var stations = await _routeRepository.GetRouteDetailsByTrainNumberAsync(trainNumber);
            return stations;
        }
    }
}
