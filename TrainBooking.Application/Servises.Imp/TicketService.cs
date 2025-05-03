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
        public async Task<IEnumerable<int>> PurchaseTicketAsync(string token, List<TicketCreateDto> ticketDtos, TripDto tripDto)
        {
            if (ticketDtos.Count > 4)
                throw new InvalidOperationException("Неможливо купити більше 4 квитків за один раз.");

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

                var ticketIds = new List<int>();

                foreach (var ticketDto in ticketDtos)
                {
                    var ticket = new Ticket
                    {
                        UserId = userId,
                        TripId = trip.TripId,
                        SeatId = ticketDto.SeatId,
                        TicketPrice = ticketDto.Price
                    };

                    await _unitOfWork.TicketRepository.AddTicketAsync(ticket);
                    await _unitOfWork.SaveAsync();

                    ticketIds.Add(ticket.TicketId);
                }

                await _unitOfWork.CommitTransactionAsync();
                return ticketIds;
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

        public async Task<IEnumerable<TicketEntity>> GetTicketsByUserIdForAdminAsync(int userId)
        {
            return await _ticketRepository.GetTicketsAsync(t => t.UserId == userId);
        }

        public async Task<IEnumerable<TicketEntity>> GetTicketsByPurcaseDateForAdminAsync(DateTime date)
        {
            var startDate = date.Date;  
            var endDate = date.Date.AddDays(1).AddTicks(-1);  

            return await _ticketRepository.GetTicketsAsync(
                t => t.PurchaseDate >= startDate && t.PurchaseDate <= endDate);
        }


        public async Task<IEnumerable<TicketEntity>> GetTicketsByTrainNumberForAdminAsync(string trainNumber)
        {
            return await _ticketRepository.GetTicketsAsync(t => t.Seat.Carriage.Train.Number == trainNumber);
        }

    }
}

