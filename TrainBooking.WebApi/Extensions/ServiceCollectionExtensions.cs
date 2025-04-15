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
           

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
           

            return services;
        }
    }
}
