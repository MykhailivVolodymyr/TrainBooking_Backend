using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Mapper
{
    public static class SchedulePatternMapper
    {
        public static SchedulePatternDto ToDto(SchedulePattern pattern)
        {
            return new SchedulePatternDto
            {
                TrainId = pattern.TrainId,
                TrainNumber = pattern.Train?.Number,
                FrequencyType = pattern.FrequencyType,
                DaysOfWeek = pattern.DaysOfWeek,
                DayParity = pattern.DayParity
            };
        }
        public static SchedulePattern ToModel(SchedulePatternDto dto)
        {
            return new SchedulePattern
            {
                TrainId = dto.TrainId,
                FrequencyType = dto.FrequencyType,
                DaysOfWeek = dto.DaysOfWeek,
                DayParity = dto.DayParity
            };
        }

    }
}
