﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Domain.Abstractions
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<ScheduleEntity>> GetTrainSchedule(string cityFrom, string cityTo, DateTime date);
        Task<IEnumerable<ScheduleTransitEntity>> GetTrainScheduleByCityAndDate(string city, DateTime date, bool isArrival);
    }
}
