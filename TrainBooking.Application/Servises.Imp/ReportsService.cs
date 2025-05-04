using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises.Imp
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public async Task<IEnumerable<RevenueReportEntity>> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _reportsRepository.GetRevenueReportAsync(startDate, endDate);
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout: {ex.Message}");
                return Enumerable.Empty<RevenueReportEntity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return Enumerable.Empty<RevenueReportEntity>();
            }
        }

        public async Task<IEnumerable<RoutePopularityReportEntetity>> GetRoutePopularityReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _reportsRepository.GetRoutePopularityReportAsync(startDate, endDate);
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout: {ex.Message}");
                return Enumerable.Empty<RoutePopularityReportEntetity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return Enumerable.Empty<RoutePopularityReportEntetity>();
            }
        }


    }
}
