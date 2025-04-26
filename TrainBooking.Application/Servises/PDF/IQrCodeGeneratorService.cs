using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises.PDF
{
    public interface IQrCodeGeneratorService
    {
        byte[] GenerateQrCode(TicketEntity ticket);
    }
}
