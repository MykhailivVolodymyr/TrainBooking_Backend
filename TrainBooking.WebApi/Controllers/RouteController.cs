using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.Servises;

namespace TrainBooking.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService trainService)
        {
            _routeService = trainService;
        }

        [HttpGet("{number}/stations")]
        public async Task<IActionResult> GetStations(string number)
        {
            try
            {
                var stations = await _routeService.GetRouteDetailsByTrainNumberAsync(number);

                if (stations == null || !stations.Any())
                {
                    return NotFound($"Поїзд з номером {number} не знайдено або не має маршрутів.");
                }

                return Ok(stations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Виникла внутрішня помилка сервера: {ex.Message}");
            }
        }

    }

}
