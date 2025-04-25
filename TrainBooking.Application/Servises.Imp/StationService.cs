using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises.Imp
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        public StationService(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }
        public async Task<Station?> GetStationByNameAsync(string StationName)
        {
           return await _stationRepository.GetStationByName(StationName);
        }

        public async Task<int> GetStationIdFromNameAsync(string stationName)
        {
            var station = await GetStationByNameAsync(stationName);
            return station?.StationId ?? throw new ArgumentException($"Станцію з назвою '{nameof(stationName)}' не знайдено.");
        }

    }
}
