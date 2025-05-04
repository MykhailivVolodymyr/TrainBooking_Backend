using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.Servises;
using TrainBooking.Application.Servises.Imp.PDF;
using TrainBooking.Application.Servises.PDF;
using TrainBooking.Domain.Entities;

namespace TrainBooking.WebApi.Controllers.Admin
{

    [ApiController]
    [Route("api/[controller]")]
    public class TicketManagementController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ICsvExporter _csvExporter;

        public TicketManagementController(ITicketService ticketService, ICsvExporter csvExporter)
        {
            _ticketService = ticketService;
            _csvExporter = csvExporter; 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<TicketEntity>>> GetTicketsByUserId(int userId)
        {
            var tickets = await _ticketService.GetTicketsByUserIdForAdminAsync(userId);

            if (!tickets.Any())
                return NotFound("Квитки для вказаного користувача не знайдено.");

            return Ok(tickets);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-purchase-date/{date}")]
        public async Task<ActionResult<IEnumerable<TicketEntity>>> GetTicketsByPurchaseDate(DateTime date)
        {
            var tickets = await _ticketService.GetTicketsByPurcaseDateForAdminAsync(date);

            if (!tickets.Any())
                return NotFound("Квитки на вказану дату не знайдено.");

            return Ok(tickets);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-train/{trainNumber}")]
        public async Task<ActionResult<IEnumerable<TicketEntity>>> GetTicketsByTrainNumber(string trainNumber)
        {
            var tickets = await _ticketService.GetTicketsByTrainNumberForAdminAsync(trainNumber);

            if (!tickets.Any())
                return NotFound("Квитки для вказаного поїзда не знайдено.");

            return Ok(tickets);
        }


      
    }
}
