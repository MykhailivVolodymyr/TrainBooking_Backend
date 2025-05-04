using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;
using TrainBooking.Domain.Models;
using TrainBooking.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrainBooking.Infrastructure.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public ReportsRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<RevenueReportEntity>> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            var sales = new List<RevenueReportEntity>();

            var connection = dbContext.Database.GetDbConnection();
            await using (connection)
            {
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "dbo.GetTicketSalesReport";
                command.CommandType = CommandType.StoredProcedure;

                var paramStartDate = command.CreateParameter();
                paramStartDate.ParameterName = "@StartDate";
                paramStartDate.Value = startDate.Date;
                command.Parameters.Add(paramStartDate);

                var paramEndDate = command.CreateParameter();
                paramEndDate.ParameterName = "@EndDate";
                paramEndDate.Value = endDate.Date;
                command.Parameters.Add(paramEndDate);


                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var sale = new RevenueReportEntity
                    {
                        Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                        TicketsSold = reader.GetInt32(reader.GetOrdinal("Tickets Sold")),
                        Revenue = reader.GetDecimal(reader.GetOrdinal("Revenue")),
                        MostPopularTrain = reader.GetString(reader.GetOrdinal("Most Popular Train")),
                    };

                    sales.Add(sale);
                }
            }

            return sales;
        }

        public async Task<IEnumerable<RoutePopularityReportEntetity>> GetRoutePopularityReportAsync(DateTime startDate, DateTime endDate)
        {
            var routes = new List<RoutePopularityReportEntetity>();

            var connection = dbContext.Database.GetDbConnection();
            await using (connection)
            {
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "dbo.GetRouteOccupancyReport";
                command.CommandType = CommandType.StoredProcedure;

                var paramStartDate = command.CreateParameter();
                paramStartDate.ParameterName = "@StartDate";
                paramStartDate.Value = startDate.Date;
                command.Parameters.Add(paramStartDate);

                var paramEndDate = command.CreateParameter();
                paramEndDate.ParameterName = "@EndDate";
                paramEndDate.Value = endDate.Date;
                command.Parameters.Add(paramEndDate);


                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var route = new RoutePopularityReportEntetity
                    {
                        Number = reader.GetString(reader.GetOrdinal("TrainNumber")),
                        Direction = reader.GetString(reader.GetOrdinal("Route")),
                        NumberOfTrips = reader.GetInt32(reader.GetOrdinal("NumberOfTrips")), 
                        NumberOfTicketsSold = reader.GetInt32(reader.GetOrdinal("NumberOfTicketsSold")),
                        AverageOccupancyPercentage = reader.GetDecimal(reader.GetOrdinal("AverageOccupancyPercentage"))
                    };
                  
                    routes.Add(route);
                }
            }

            return routes;
        }
    }
    
}
