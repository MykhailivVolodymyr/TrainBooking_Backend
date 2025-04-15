using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDto>> GetTrainSchedule(string cityFrom, string cityTo, DateTime date);
    }
}
