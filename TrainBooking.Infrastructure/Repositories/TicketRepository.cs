using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;
using TrainBooking.Domain.Models;
using TrainBooking.Infrastructure.Data;

namespace TrainBooking.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public TicketRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddTicketAsync(Ticket ticket)
        {
            await dbContext.Tickets.AddAsync(ticket);
        }

        public async Task ReturnTicketAsync(int ticketId)
        {
            var ticket = await dbContext.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                ticket.IsReturned = true;
            }
        }


        public async Task<IEnumerable<TicketEntity>> GetTicketsAsync(Expression<Func<Ticket, bool>> predicate)
        {
            var ticketDataQuery = dbContext.Tickets
                .Where(predicate)
                .Select(t => new
                {
                    t.TicketId,
                    FullName = t.User.FullName,
                    TrainId = t.Trip.TrainId,
                    TrainNumber = t.Trip.Train.Number,
                    CarriageId = t.Seat.CarriageId,
                    CarriageType = t.Seat.Carriage.CarriageType,
                    SeatNumber = t.Seat.SeatNumber,
                    SeatType = t.Seat.SeatType,
                    DepartureStation = t.Trip.StartStation.StationName,
                    DepartureCity = t.Trip.StartStation.City,
                    ArrivalStation = t.Trip.EndStation.StationName,
                    ArrivalCity = t.Trip.EndStation.City,
                    DepartureTime = t.Trip.DepartureTime,
                    ArrivalTime = t.Trip.ArrivalTime,
                    TicketPrice = t.TicketPrice
                });

            var ticketData = await ticketDataQuery.ToListAsync();

            if (!ticketData.Any())
                return Enumerable.Empty<TicketEntity>();

            var trainIds = ticketData.Select(td => td.TrainId).Distinct().ToList();

            var carriages = await dbContext.Carriages
                .Where(c => trainIds.Contains(c.TrainId))
                .OrderBy(c => c.CarriageId)
                .ToListAsync();

            var carriageNumbers = carriages
                .GroupBy(c => c.TrainId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select((c, index) => new { c.CarriageId, CarriageNumber = index + 1 })
                          .ToDictionary(x => x.CarriageId, x => x.CarriageNumber)
                );

            return ticketData.Select(t => new TicketEntity
            {
                TicketId = t.TicketId,
                FullName = t.FullName,
                TrainNumber = t.TrainNumber,
                CarriageNumber = carriageNumbers.TryGetValue(t.TrainId, out var carriagesInTrain) && carriagesInTrain.TryGetValue(t.CarriageId, out var carriageNumber)
                                  ? carriageNumber
                                  : 0,
                CarriageType = t.CarriageType,
                SeatNumber = t.SeatNumber,
                SeatType = t.SeatType,
                DepartureStation = t.DepartureStation,
                DepartureCity = t.DepartureCity,
                ArrivalStation = t.ArrivalStation,
                ArrivalCity = t.ArrivalCity,
                DepartureTime = t.DepartureTime,
                ArrivalTime = t.ArrivalTime,
                TicketPrice = t.TicketPrice
            });
        }

    }
}
