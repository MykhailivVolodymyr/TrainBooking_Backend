using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.Servises;
using TrainBooking.Domain.Models;
using TrainBooking.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using TrainBooking.Application.Servises.PDF;

namespace TrainBooking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IPdfGeneratorService _pdfGeneratorService;

        public TicketController(ITicketService ticketService, IPdfGeneratorService pdfGenerator)
        {
            _ticketService = ticketService;
            _pdfGeneratorService = pdfGenerator;
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpGet("user/tickets")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var token = Request.Cookies["Jwt-token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is required");
            }
            try
            {
               var tickets =  await _ticketService.GetTicketsByUserIdAsync(token);

                return Ok(tickets);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("user/ticket/{ticketId}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket(int ticketId)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByTicketIdAsync(ticketId);

                if (ticket == null)
                {
                    return NotFound("Ticket not found.");
                }
                var pdfBytes = await _pdfGeneratorService.GenerateTicketPdfAsync(ticket);

                var fileName = $"{ticket.FullName.Replace(" ", "")}-{ticket.DepartureTime:yyyy-MM-dd}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
