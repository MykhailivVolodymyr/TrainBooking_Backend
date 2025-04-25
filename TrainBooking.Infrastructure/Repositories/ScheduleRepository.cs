using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Entities;
using TrainBooking.Infrastructure.Data;

namespace TrainBooking.Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly TrainBookingDbContext dbContext;

        public ScheduleRepository(TrainBookingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<IEnumerable<ScheduleEntity>> GetTrainSchedule(string cityFrom, string cityTo, DateTime date)
        {
            var schedules = new List<ScheduleEntity>();

            var connection = dbContext.Database.GetDbConnection();
            await using (connection)
            {
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "dbo.GetTrainScheduleByCitiesAndDate";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Додаємо параметри
                var paramFrom = command.CreateParameter();
                paramFrom.ParameterName = "@CityFrom";
                paramFrom.Value = cityFrom;
                command.Parameters.Add(paramFrom);

                var paramTo = command.CreateParameter();
                paramTo.ParameterName = "@CityTo";
                paramTo.Value = cityTo;
                command.Parameters.Add(paramTo);

                var paramDate = command.CreateParameter();
                paramDate.ParameterName = "@Date";
                paramDate.Value = date;
                command.Parameters.Add(paramDate);

                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var schedule = new ScheduleEntity
                    {
                        ScheduleId = reader.GetInt32(reader.GetOrdinal("ScheduleId")),
                        TrainId = reader.GetInt32(reader.GetOrdinal("TrainId")),
                        TrainNumber = reader.GetString(reader.GetOrdinal("TrainNumber")),
                        RouteCities = reader.GetString(reader.GetOrdinal("RouteCities")),
                        RouteId = reader.GetInt32(reader.GetOrdinal("RouteId")),
                        RealDepartureDateFromCity = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("RealDepartureDateFromCity"))),
                        DepartureDateFromStart = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DepartureDateFromStart"))),
                        FromStationName = reader.GetString(reader.GetOrdinal("FromStationName")),
                        ArrivalTimeFromCity = TimeOnly.FromTimeSpan((TimeSpan)reader["ArrivalTimeFromCity"]),
                        ToStationName = reader.GetString(reader.GetOrdinal("ToStationName")),
                        ArrivalDateToEnd = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("ArrivalDateToEnd"))),
                        RealDepartureDateToCity = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("RealDepartureDateToCity"))),
                        ArrivalTimeToCity = TimeOnly.FromTimeSpan((TimeSpan)reader["ArrivalTimeToCity"])
                    };
                    schedules.Add(schedule);
                }
            }

            return schedules;
        }



        public async Task<IEnumerable<ScheduleTransitEntity>> GetTrainScheduleByCityAndDate(string city, DateTime date, bool isArrival)
        {
            var schedules = new List<ScheduleTransitEntity>();

            var connection = dbContext.Database.GetDbConnection();
            await using (connection)
            {
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "dbo.GetTrainScheduleByCityAndDate";
                command.CommandType = CommandType.StoredProcedure;

                // Параметр @City
                var paramCity = command.CreateParameter();
                paramCity.ParameterName = "@City";
                paramCity.Value = city;
                command.Parameters.Add(paramCity);

                // Параметр @Date
                var paramDate = command.CreateParameter();
                paramDate.ParameterName = "@Date";
                paramDate.Value = date.Date;
                command.Parameters.Add(paramDate);

                // Параметр @IsArrival
                var paramArrival = command.CreateParameter();
                paramArrival.ParameterName = "@IsArrival";
                paramArrival.Value = isArrival;
                command.Parameters.Add(paramArrival);

                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var schedule = new ScheduleTransitEntity
                    {
                        TrainNumber = reader.GetString(reader.GetOrdinal("TrainNumber")),
                        RouteCities = reader.GetString(reader.GetOrdinal("RouteCities")),
                        Time = TimeOnly.FromTimeSpan((TimeSpan)reader["Time"]),
                        StationName = reader.GetString(reader.GetOrdinal("StationName"))
                    };


                    schedules.Add(schedule);
                }
            }

            return schedules;
        }

    }
}
