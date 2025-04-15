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
        Task<IEnumerable<ScheduleDto>> GetTrainScheduleAsync(string cityFrom, string cityTo, DateTime date);
        Task<IEnumerable<ScheduleTransitDto>> GetTrainScheduleByCityAndDateAsync(string city, DateTime date, bool isArrival);
    }
}
