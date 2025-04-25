using TrainBooking.Application.Servises;
using TrainBooking.Application.Servises.Auth;
using TrainBooking.Application.Servises.Imp;
using TrainBooking.Application.Helpers;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Infrastructure.Repositories;
using TrainBooking.Infrastructure.Providers;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

namespace TrainBooking.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ITrainStructureService, TrainStructureService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IStationService, StationService>();


            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ITrainRepository, TrainRepository>();
            services.AddScoped<ICarriageRepository, CarriageRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStationRepository, StationRepository>();

            return services;
        }

        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IOptions<JwtOptions> JwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.SecretKey))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["Jwt-token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            // Додаємо політики для ролей
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Доступ лише для Admin
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));   // Доступ лише для User
            });

            services.AddAuthorization();
            return services;
        }
    }
}
