using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.DTOs;
using TrainBooking.Application.Servises;
using TrainBooking.Application.Servises.Imp;
using TrainBooking.Domain.Abstractions;

namespace TrainBooking.WebApi.Controllers.Admin
{

    [ApiController]
    [Route("api/[controller]")]
    public class SchedulePatternController : ControllerBase
    {
        private readonly ITrainRepository _trainRepository;
        private readonly ISchedulePatternService _schedulePatternService;

        public SchedulePatternController(ITrainRepository trainRepository, ISchedulePatternService schedulePatternService)
        {
            _trainRepository = trainRepository;
            _schedulePatternService = schedulePatternService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("schedule-patterns/all")]

        public async Task<ActionResult<IEnumerable<SchedulePatternDto>>> GetAllPatterns()
        {
            try
            {
                var patterns = await _schedulePatternService.GetAll();
                if (patterns == null || !patterns.Any())
                {
                    return NotFound("Патерни розкладів не знайдені.");
                }

                return Ok(patterns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Виникла помилка: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("train/{trainNumber}")]
        public async Task<ActionResult<SchedulePatternDto>> GetScheduleByTrainId(string trainNumber)
        {
            try
            {
                var train = await _trainRepository.GetByNumberAsync(trainNumber);
                var scheduleDto = await _schedulePatternService.GetScheduleByTrainIdAsync(train.TrainId);
                return Ok(scheduleDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Сталася помилка на сервері.", details = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("train/{trainNumber}/update")]
        public async Task<IActionResult> UpdateSchedule(string trainNumber, [FromBody] SchedulePatternDto scheduleDto)
        {
            if (scheduleDto == null || trainNumber != scheduleDto.TrainNumber)
            {
                return BadRequest("Некоректні дані: ID поїзда не збігається або DTO є null.");
            }

            var train = await _trainRepository.GetByNumberAsync(trainNumber);
            try
            {
                await _schedulePatternService.UpdateScheduleAsync(train.TrainId, scheduleDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
