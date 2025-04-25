using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;
using TrainBooking.Application.DTOs;

namespace TrainBooking.Application.Servises
{
    public interface ITicketService
    {
        Task PurchaseTicketAsync(string token, TicketDto ticket, TripDto trip);
    }
}
