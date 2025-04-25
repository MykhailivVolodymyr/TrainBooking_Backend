using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Application.Servises.Auth;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises.Imp
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IStationService _stationService;
        public TicketService(IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IStationService stationServise)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _stationService = stationServise;
        }
        public async Task PurchaseTicketAsync(string token, TicketDto ticketDto, TripDto tripDto)
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
    }
}
