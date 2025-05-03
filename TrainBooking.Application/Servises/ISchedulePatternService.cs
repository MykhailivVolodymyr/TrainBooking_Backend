using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises
{
    public interface ISchedulePatternService
    {
        Task<IEnumerable<SchedulePatternDto>> GetAll();
        Task UpdateScheduleAsync(int trainId, SchedulePatternDto scheduleDto);
        Task<SchedulePatternDto> GetScheduleByTrainIdAsync(int trainId);
    }
}
