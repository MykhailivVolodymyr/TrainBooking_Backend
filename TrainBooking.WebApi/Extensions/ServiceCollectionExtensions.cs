using TrainBooking.Application.Servises;
using TrainBooking.Application.Servises.Imp;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Infrastructure.Repositories;

namespace TrainBooking.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ITrainStructureService, TrainStructureService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ITrainRepository, TrainRepository>();
            services.AddScoped<ICarriageRepository, CarriageRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();

            return services;
        }
    }
}
