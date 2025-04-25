using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises
{
    public interface IRouteService
    {
        Task<IEnumerable<RouteDetailsEntity>> GetRouteDetailsByTrainNumberAsync(string trainNumber);
    }
}
