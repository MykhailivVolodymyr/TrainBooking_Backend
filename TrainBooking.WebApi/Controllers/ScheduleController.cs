using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainBooking.Domain.Entities;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Application.Servises;

namespace TrainBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // Метод для отримання розкладу потягів за містами та датою
        [HttpGet("GetSchedule")]
        public async Task<ActionResult<IEnumerable<ScheduleEntity>>> GetTrainSchedule(string cityFrom, string cityTo, DateTime date)
        {
            try
            {
                // Викликаємо метод репозиторію для отримання розкладу
                var schedules = await _scheduleService.GetTrainScheduleAsync(cityFrom, cityTo, date);
                if (schedules == null || !schedules.Any())
                {
                    return NotFound("Розклад не знайдено для вказаних міст і дати.");
                }
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                 return StatusCode(500, "Виникла помилка при отриманні розкладу: " + ex.Message);
            }
        }


        [HttpGet("GetScheduleTransit")]
        public async Task<ActionResult<IEnumerable<ScheduleTransitEntity>>> GetTrainScheduleByCityAndDate(string city, DateTime date, bool isArrival)
        {
            try
            {
                // Викликаємо метод сервісу для отримання розкладу
                var schedules = await _scheduleService.GetTrainScheduleByCityAndDateAsync(city, date, isArrival);
                if (schedules == null || !schedules.Any())
                {
                    return NotFound("Розклад не знайдено для вказаного міста і дати.");
                }
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Виникла помилка при отриманні розкладу: " + ex.Message);
            }
        }

    }
}
