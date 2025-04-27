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

        // Приватний метод, який будує структуру потяга
        private async Task<TrainStructureDto> BuildTrainStructureAsync(string trainNumber, int? scheduleId = null)
        {
            var train = scheduleId == null
                ? await _trainRepository.GetByNumberAsync(trainNumber)  // Якщо не передано scheduleId, шукаємо за номером потяга
                : await _trainRepository.GetByScheduleIdAsync(scheduleId.Value);  // Якщо є scheduleId, шукаємо за розкладом

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
                // Отримуємо місця для кожного вагона (в залежності від того, чи передано scheduleId)
                var seats = scheduleId == null
                    ? await _seatRepository.GetByCarriageIdAsync(carriage.CarriageId)  // Якщо scheduleId відсутній, отримуємо всі місця
                    : await _seatRepository.GetAvailableSeatsByCarriageAndScheduleAsync(carriage.CarriageId, scheduleId.Value);  // Якщо scheduleId є, отримуємо доступні місця

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

        public async Task<TrainStructureDto> GetTrainStructureAsync(string trainNumber)
        {
            return await BuildTrainStructureAsync(trainNumber);
        }

        public async Task<TrainStructureDto> GetTrainStructureWithAvalibleSeatsAsync(int scheduleId)
        {
            return await BuildTrainStructureAsync(null, scheduleId);
        }
    }
}
