using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;

namespace TrainBooking.Domain.Abstractions
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetByCarriageIdAsync(int carriageId);
        Task<IEnumerable<Seat>> GetAvailableSeatsByCarriageAndScheduleAsync(int carriageId, int scheduleId);
    }
}
