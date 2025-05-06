using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.DTOs;
using TrainBooking.Application.Servises;
using TrainBooking.Application.Servises.Imp;


namespace TrainBooking.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainController : ControllerBase
    {
        private readonly ITrainStructureService _trainStructureService;

        public TrainController(ITrainStructureService trainStructureService)
        {
            _trainStructureService = trainStructureService;
        }

        [HttpGet("{trainNumber}/structure")]
        public async Task<ActionResult<TrainStructureDto>> GetTrainStructure(string trainNumber)
        {
            if (string.IsNullOrWhiteSpace(trainNumber))
            {
                return BadRequest("Train number is required.");
            }
            try
            {
                var result = await _trainStructureService.GetTrainStructureAsync(trainNumber);

                if (result == null)
                {
                    return NotFound($"Train with number '{trainNumber}' not found.");
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An unexpected error occurred while processing the request." + e.Message);
            }
        }


        [HttpGet("{scheduleId}/AvalibleSeats")]
        public async Task<ActionResult<TrainStructureDto>> GetTrainAvalibleSeats(int scheduleId)
        {
            try
            {
                var result = await _trainStructureService.GetTrainStructureWithAvalibleSeatsAsync(scheduleId);

                if (result == null)
                {
                    return NotFound($"Schedule with id '{scheduleId}' not found.");
                }
                return Ok(result);
            }
            catch(Exception e)
            {
                return StatusCode(500, "An unexpected error occurred while processing the request." + e.Message);
            }
        }
    }
}
