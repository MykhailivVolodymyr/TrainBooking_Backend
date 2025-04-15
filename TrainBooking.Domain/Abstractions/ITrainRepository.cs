using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;

namespace TrainBooking.Domain.Abstractions
{
    public interface ITrainRepository
    {
        Task<Train?> GetByNumberAsync(string trainNumber);
    }
}
