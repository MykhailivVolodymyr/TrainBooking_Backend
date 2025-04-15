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

        // Ін'єкція залежностей для репозиторія
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // Метод для отримання розкладу потягів за містами та датою
        [HttpGet("GetSchedule")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetTrainSchedule(string cityFrom, string cityTo, DateTime date)
        {
            try
            {
                // Викликаємо метод репозиторію для отримання розкладу
                var schedules = await _scheduleService.GetTrainSchedule(cityFrom, cityTo, date);

                // Якщо не знайдено жодного розкладу, повертаємо 404
                if (schedules == null || !schedules.Any())
                {
                    return NotFound("Розклад не знайдено для вказаних міст і дати.");
                }

                // Повертаємо результат з кодом 200 OK
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                // Логування помилки, якщо потрібно
                 return StatusCode(500, "Виникла помилка при отриманні розкладу: " + ex.Message);

                // Повертаємо загальну помилку сервера
               // return StatusCode(500, "Виникла помилка при отриманні розкладу.");
            }
        }
    }
}
