using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.Servises.Email;
using TrainBooking.Application.Servises.Auth;


namespace TrainBooking.Application.Servises.Imp.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailSettings;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserService _userService;
        public EmailService(IOptions<EmailOptions> emailSettings, IJwtProvider jwtProvider, IUserService userService)
        {
            _emailSettings = emailSettings.Value;
            _jwtProvider = jwtProvider;
            _userService = userService;
        }
       
        public async Task SendTicketEmailAsync(string token, byte[] pdfBytes, string fileName, string body)
        {
            try
            {
                var userId = _jwtProvider.GetUserIdFromToken(token);
                var user = await _userService.GetUserByIdAsync(userId);
                
                var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SmtpUser, "TrainBooking"),
                    Subject = "Ваш залізничний квиток — підтвердження покупки",
                    Body = body,
                    IsBodyHtml = false
                };
                message.To.Add(user.Email);

                var attachment = new Attachment(new MemoryStream(pdfBytes), fileName, "application/pdf");
                message.Attachments.Add(attachment);

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
