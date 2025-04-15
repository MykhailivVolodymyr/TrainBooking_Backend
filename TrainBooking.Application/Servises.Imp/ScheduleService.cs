﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises.Imp
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        public Task<IEnumerable<ScheduleDto>> GetTrainSchedule(string cityFrom, string cityTo, DateTime date)
        {
          return _scheduleRepository.GetTrainSchedule(cityFrom, cityTo, date);
        }
    }
}
