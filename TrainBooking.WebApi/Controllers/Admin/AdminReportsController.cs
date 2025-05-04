using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.Servises;
using TrainBooking.Application.Servises.PDF;
using TrainBooking.Domain.Entities;

namespace TrainBooking.WebApi.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminReportsController: ControllerBase
    {
        private readonly ICsvExporter _csvExporter;
        private readonly IReportsService _reportsService;
        public AdminReportsController(ICsvExporter csvExporter, IReportsService reportsService)
        {
            _csvExporter = csvExporter;
            _reportsService = reportsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("export-csv")]
        public async Task<ActionResult> ExportTicketsToCsv(string reportType, DateTime startDate, DateTime endDate)
        {

            switch (reportType.ToLower())
            {
                case "routespopulariry":
                    var tickets = await _reportsService.GetRoutePopularityReportAsync(startDate, endDate);
                    var csvBytes = await _csvExporter.ExportToCsv<RoutePopularityReportEntetity>(tickets);
                    return File(csvBytes, "text/csv", $"{reportType}_report.csv");


                    break;

                case "revenuereport":
                    var revenues = await _reportsService.GetRevenueReportAsync(startDate, endDate);
                    var csvBytes2 = await _csvExporter.ExportToCsv<RevenueReportEntity>(revenues);
                    return File(csvBytes2, "text/csv", $"{reportType}_report.csv");
                    break;

                //case "trains":
                //    var trains = await _trainService.GetTrainsAsync(); // returns IEnumerable<TrainDto>
                //    data = trains.Cast<object>();
                //    break;

                default:
                    return BadRequest("Unknown report type.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("popularity-route-report")]
        public async Task<ActionResult<RoutePopularityReportEntetity>> RoutePopularity([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _reportsService.GetRoutePopularityReportAsync(startDate, endDate);

                if (result == null || !result.Any())
                {
                    return NotFound("No data found for the given date range.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("revenue-report")]
        public async Task<ActionResult<RevenueReportEntity>> RevenueReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await _reportsService.GetRevenueReportAsync(startDate, endDate);

                if (result == null || !result.Any())
                {
                    return NotFound("No data found for the given date range.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
