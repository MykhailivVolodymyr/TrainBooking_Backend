using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Domain.Abstractions
{
    public interface IUnitOfWork: IDisposable
    {
        ITripRepository TripRepository { get; }
        ITicketRepository TicketRepository { get; }

        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
