using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Domain.Abstractions
{
    public interface IReportsRepository
    {
        Task<IEnumerable<RoutePopularityReportEntetity>> GetRoutePopularityReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<RevenueReportEntity>> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
    }
}
