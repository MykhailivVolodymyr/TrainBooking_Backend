using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Application.Servises.Auth;
using TrainBooking.Application.Servises.PDF;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises.Imp
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IStationService _stationService;
        private readonly ITicketRepository _ticketRepository;
        public TicketService(IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IStationService stationServise, ITicketRepository ticketRepository)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _stationService = stationServise;
            _ticketRepository = ticketRepository;
        }

        //To do додати оплату
        public async Task PurchaseTicketAsync(string token, TicketCreateDto ticketDto, TripDto tripDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _jwtProvider.GetUserIdFromToken(token);

                var trip = TripMapper.MapToTrip(tripDto);
                var startStationId = await _stationService.GetStationIdFromNameAsync(tripDto.StartStationName);
                var endStationId = await _stationService.GetStationIdFromNameAsync(tripDto.EndStationName);
                trip.StartStationId = startStationId;
                trip.EndStationId = endStationId;

                await _unitOfWork.TripRepository.AddTripAsync(trip);
                await _unitOfWork.SaveAsync();

                var ticket = new Ticket() {
                    UserId = userId,
                    TripId = trip.TripId,
                    SeatId = ticketDto.SeatId,
                    TicketPrice = ticketDto.Price
                };

                await _unitOfWork.TicketRepository.AddTicketAsync(ticket);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task ReturnTicketAsync(int ticketId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                Trip? trip = await _unitOfWork.TripRepository.GetTripByTickedId(ticketId);
                if (trip.DepartureTime.AddHours(-1) > DateTime.UtcNow)
                {

                    await _unitOfWork.TicketRepository.ReturnTicketAsync(ticketId);

                    // (опціонально) оновити статус оплати
                    await _unitOfWork.SaveAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    throw new InvalidOperationException("Ви не можете повернути квиток оськільки час відправлення вже настав");
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<IEnumerable<TicketEntity>> GetTicketsByUserIdAsync(string token)
        {
            var userId = _jwtProvider.GetUserIdFromToken(token);
            return await _ticketRepository.GetTicketsAsync(t => t.UserId == userId);
        }

        public async Task<TicketEntity?> GetTicketByTicketIdAsync(int ticketId)
        {
            var tickets = await _ticketRepository.GetTicketsAsync(t => t.TicketId == ticketId);
            return tickets.FirstOrDefault();
        }
    }
}
