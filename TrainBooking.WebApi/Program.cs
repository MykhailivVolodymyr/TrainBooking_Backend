using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TrainBooking.Infrastructure.Data;
using TrainBooking.Infrastructure.Providers;
using TrainBooking.Application.Servises.Imp.Email;
using TrainBooking.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhot3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();  // Дозволити куки та заголовки автентифікації
    });
});




builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)));
// Add services to the container.
builder.Services.AddDbContext<TrainBookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();

var serviceProvider = builder.Services.BuildServiceProvider();
var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>();
// di з методів розширення
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();
builder.Services.AddApiAuthentication(jwtOptions);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowLocalhot3000");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
