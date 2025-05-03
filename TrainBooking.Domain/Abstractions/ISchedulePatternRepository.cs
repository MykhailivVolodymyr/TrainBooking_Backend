using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;

namespace TrainBooking.Domain.Abstractions
{
    public interface ISchedulePatternRepository
    {
        Task<IEnumerable<SchedulePattern>> GetAll();
        Task UpdateScheduleAsync(int trainId, SchedulePattern schedule);
        Task<SchedulePattern> GetScheduleByTrainIdAsync(int trainId);
    }
}
