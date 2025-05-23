﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Domain.Abstractions
{
    public interface IRouteRepository
    {
        Task<IEnumerable<RouteDetailsEntity>> GetRouteDetailsByTrainNumberAsync(string trainNumber);
    }
}
