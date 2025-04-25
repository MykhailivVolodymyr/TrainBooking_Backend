using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises
{
    public interface IStationService
    {
        Task<Station?> GetStationByNameAsync(string StationName);
        Task<int> GetStationIdFromNameAsync(string stationName);
    }
}
