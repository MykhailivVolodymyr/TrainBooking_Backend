using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;
using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Entities;
using TrainBooking.Domain.Abstractions;

namespace TrainBooking.Application.Servises
{
    public interface ITicketService
    {
        Task<IEnumerable<int>> PurchaseTicketAsync(string token, List<TicketCreateDto> tickets, TripDto trip);
        Task ReturnTicketAsync(int ticketId);
        Task<IEnumerable<TicketEntity>> GetTicketsByUserIdAsync(string token);
        Task<TicketEntity?> GetTicketByTicketIdAsync(int ticketId);
        Task<IEnumerable<TicketEntity>> GetTicketsByUserIdForAdminAsync(int userId);
        Task<IEnumerable<TicketEntity>> GetTicketsByPurcaseDateForAdminAsync(DateTime date);
        Task<IEnumerable<TicketEntity>> GetTicketsByTrainNumberForAdminAsync(string trainNumber);
    }
}
