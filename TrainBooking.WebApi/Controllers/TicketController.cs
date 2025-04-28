using Microsoft.AspNetCore.Mvc;
using TrainBooking.Application.Servises;
using TrainBooking.Domain.Models;
using TrainBooking.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using TrainBooking.Application.Servises.PDF;
using TrainBooking.Application.Servises.Email;

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
            if (model == null || model.Ticket == null || model.Trip == null)
            {
                return BadRequest("Ticket and Trip details are required");
            }
            try
            {
               var ticketId = await _ticketService.PurchaseTicketAsync(token, model.Ticket, model.Trip);
                var ticket = await _ticketService.GetTicketByTicketIdAsync(ticketId);

                if (ticket == null)
                {
                    return NotFound("Ticket not found.");
                }

                var body = $@"Шановний клієнте,

                        Дякуємо, що скористались нашими послугами! Ми раді повідомити, що ваш залізничний квиток був успішно придбаний. Ось ваш квиток на поїзд:

                        Дата і час відправлення: {ticket.DepartureTime:yyyy-MM-dd HH:mm}
                        Номер поїзда: {ticket.TrainNumber}
                        Номер вагона: {ticket.CarriageNumber}
                        Місце: {ticket.SeatNumber}

                        Документ з квитком у вигляді PDF додається до цього листа для вашої зручності.

                        Якщо у вас виникли питання або необхідність змінити квиток, будь ласка, звертайтесь до нашої служби підтримки.

                        З найкращими побажаннями,
                        Команда компанії TrainBooking, що продає залізничні квитки
                        Телефон служби підтримки: {"066666666"}
                        Email служби підтримки: {"supportEmail@gmail.com"}";



                var pdfBytes = await _pdfGeneratorService.GenerateTicketPdfAsync(ticket);

                var fileName = $"{ticket.FullName.Replace(" ", "")}-{ticket.DepartureTime:yyyy-MM-dd}.pdf";

                await _emailService.SendTicketEmailAsync(token, pdfBytes, fileName, body);


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
