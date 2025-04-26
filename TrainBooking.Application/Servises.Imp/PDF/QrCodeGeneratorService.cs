using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QRCoder;
using TrainBooking.Application.Servises.PDF;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises.Imp.PDF
{
    public class QrCodeGeneratorService : IQrCodeGeneratorService
    {
        public byte[] GenerateQrCode(TicketEntity ticket)
        {
            var ticketData = new
            {
                ticketId = ticket.TicketId,
                fullName = ticket.FullName,
                trainNumber = ticket.TrainNumber,
                departureStation = ticket.DepartureStation,
                arrivalStation = ticket.ArrivalStation,
                departureTime = ticket.DepartureTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                seat = new
                {
                    carriageNumber = ticket.CarriageNumber,
                    seatNumber = ticket.SeatNumber
                }
            };

            // Серіалізуємо об'єкт в JSON
            var json = JsonConvert.SerializeObject(ticketData);

            // Генерація QR-коду
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            var pngByteQRCode = new PngByteQRCode(qrCodeData);
            return pngByteQRCode.GetGraphic(20);
        }
    }
}