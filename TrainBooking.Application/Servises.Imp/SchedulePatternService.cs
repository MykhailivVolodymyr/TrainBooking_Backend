using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Application.Mapper;
using TrainBooking.Domain.Abstractions;

namespace TrainBooking.Application.Servises.Imp
{
    public class SchedulePatternService : ISchedulePatternService
    {
        private readonly ISchedulePatternRepository _schedulePatternRepository;

        public SchedulePatternService(ISchedulePatternRepository schedulePatternRepository)
        {
            _schedulePatternRepository = schedulePatternRepository;
        }

        public async Task<IEnumerable<SchedulePatternDto>> GetAll()
        {
            var patterns = await _schedulePatternRepository.GetAll();
            var dtos = patterns.Select(p => SchedulePatternMapper.ToDto(p)); 
            return dtos;
        }

        public async Task<SchedulePatternDto> GetScheduleByTrainIdAsync(int trainId)
        {
            var schedule = await _schedulePatternRepository.GetScheduleByTrainIdAsync(trainId);

            if (schedule == null)
                throw new InvalidOperationException($"Розклад для потяга з ID {trainId} не знайдено.");

            return SchedulePatternMapper.ToDto(schedule);
        }

        public async Task UpdateScheduleAsync(int trainId, SchedulePatternDto scheduleDto)
        {
            try
            {
                var schedule = SchedulePatternMapper.ToModel(scheduleDto);
                await _schedulePatternRepository.UpdateScheduleAsync(trainId, schedule);
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException($"Розклад для поїзда з ID {trainId} не знайдено.");
            }
            catch (Exception ex)
            {
                throw new Exception("Сталася непередбачена помилка під час оновлення розкладу.", ex);
            }
        }

    }
}
