using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.Servises;
using TrainBooking.Domain.Models;
using TrainBooking.Application.DTOs;

namespace TrainBooking.Web.Controllers
{

    // Створюємо клас для об'єднання Ticket і Trip
    public class TicketPurchaseModel
    {
        public TicketDto Ticket { get; set; }
        public TripDto Trip { get; set; }
    }








    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }


        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseTicket([FromBody] TicketPurchaseModel model)
        {

            var token = Request.Cookies["Jwt-token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is required");
            }
            if (model == null || model.Ticket == null || model.Trip == null)
            {
                return BadRequest("Ticket and Trip details are required");
            }
            try
            {
                await _ticketService.PurchaseTicketAsync(token, model.Ticket, model.Trip);

                return Ok("Ticket purchased successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("returnTicket/{ticketId}")]
        public async Task<IActionResult> ReturnTicket(int ticketId)
        {
            try
            {
                await _ticketService.ReturnTicketAsync(ticketId);
                return Ok(new { message = "Квиток успішно повернуто." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Сталася помилка при поверненні квитка.", error = ex.Message });
            }
        }
    }
}
