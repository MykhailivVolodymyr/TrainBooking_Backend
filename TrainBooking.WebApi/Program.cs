using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TrainBooking.Infrastructure.Data;
using TrainBooking.Infrastructure.Providers;
using TrainBooking.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
