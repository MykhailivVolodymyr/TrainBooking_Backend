using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.Servises;
using TrainBooking.Domain.Models;
using TrainBooking.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using TrainBooking.Application.Servises.PDF;
using TrainBooking.Application.Servises.Email;
using System.Text;

namespace TrainBooking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IEmailService _emailService;

        public TicketController(ITicketService ticketService, IPdfGeneratorService pdfGenerator, IEmailService emailService)
        {
            _ticketService = ticketService;
            _pdfGeneratorService = pdfGenerator;
            _emailService = emailService;
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
            if (model == null || model.Tickets == null || model.Trip == null)
            {
                return BadRequest("Ticket and Trip details are required");
            }
            try
            {
                var ticketIds = await _ticketService.PurchaseTicketAsync(token, model.Tickets, model.Trip);

                var bodyHeader = @"Шановний клієнте,

                        Дякуємо, що скористались нашими послугами! Ось ваші квитки на поїзд:";
                var pdfs = new List<(byte[] pdf, string fileName)>();
                var bodyBuilder = new StringBuilder(bodyHeader);

                foreach (var id in ticketIds)
                {
                    var ticket = await _ticketService.GetTicketByTicketIdAsync(id);
                    if (ticket == null) continue;

                    bodyBuilder.AppendLine($@"

                        Дата і час відправлення: {ticket.DepartureTime:yyyy-MM-dd HH:mm}, Номер поїзда: {ticket.TrainNumber}, Номер вагона: {ticket.CarriageNumber}, Місце: {ticket.SeatNumber}");

                    var pdf = await _pdfGeneratorService.GenerateTicketPdfAsync(ticket);
                    var fileName = $"{ticket.FullName.Replace(" ", "")}-{ticket.DepartureTime:yyyy-MM-dd}.pdf";
                    pdfs.Add((pdf, fileName));
                }

                bodyBuilder.AppendLine(@"

                    Документи з квитками додаються до листа.

                    З найкращими побажаннями,
                    Команда компанії TrainBooking
                    Телефон: 066666666
                    Email: supportEmail@gmail.com");

                await _emailService.SendTicketEmailAsync(token, pdfs, bodyBuilder.ToString());

                return Ok("Квитки успішно придбані");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("tickets/{ticketId}/return")]
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
