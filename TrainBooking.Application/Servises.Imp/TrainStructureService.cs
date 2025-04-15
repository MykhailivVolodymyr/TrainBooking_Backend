using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Abstractions;

namespace TrainBooking.Application.Servises.Imp
{
    public class TrainStructureService : ITrainStructureService
    {
        private readonly ITrainRepository _trainRepository;
        private readonly ICarriageRepository _carriageRepository;
        private readonly ISeatRepository _seatRepository;

        public TrainStructureService(ITrainRepository trainRepository, ICarriageRepository carriageRepository, ISeatRepository seatRepository)
        {
            _trainRepository = trainRepository;
            _carriageRepository = carriageRepository;
            _seatRepository = seatRepository;
        }
        public async Task<TrainStructureDto> GetTrainStructureAsync(string trainNumber)
        {
            var train = await _trainRepository.GetByNumberAsync(trainNumber);
            if (train == null)
            {
                return null;
            }

            var carriages = await _carriageRepository.GetByTrainIdAsync(train.TrainId);

            var trainStructureDto = new TrainStructureDto
            {
                TrainNumber = train.Number,
                Carriages = new List<CarriageDto>()
            };

            foreach (var carriage in carriages)
            {
                // Отримуємо місця для кожного вагона
                var seats = await _seatRepository.GetByCarriageIdAsync(carriage.CarriageId);

                var carriageDto = new CarriageDto
                {
                    CarriageId = carriage.CarriageId,
                    CarriageType = carriage.CarriageType,
                    Capacity = carriage.Capacity,
                    Seats = seats.Select(s => new SeatDto
                    {
                        SeatId = s.SeatId,
                        SeatNumber = s.SeatNumber,
                        SeatType = s.SeatType
                    }).ToList()
                };

                trainStructureDto.Carriages.Add(carriageDto);
            }

            return trainStructureDto;
        }
    }
}
