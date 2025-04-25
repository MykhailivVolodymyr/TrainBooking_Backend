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
        Task<IEnumerable<ScheduleEntity>> GetTrainScheduleAsync(string cityFrom, string cityTo, DateTime date);
        Task<IEnumerable<ScheduleTransitEntity>> GetTrainScheduleByCityAndDateAsync(string city, DateTime date, bool isArrival);
    }
}
