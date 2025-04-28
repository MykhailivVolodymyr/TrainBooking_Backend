using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;
using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises
{
    public interface ITicketService
    {
        Task<int> PurchaseTicketAsync(string token, TicketCreateDto ticket, TripDto trip);
        Task ReturnTicketAsync(int ticketId);
        Task<IEnumerable<TicketEntity>> GetTicketsByUserIdAsync(string token);
        Task<TicketEntity?> GetTicketByTicketIdAsync(int ticketId);
    }
}
