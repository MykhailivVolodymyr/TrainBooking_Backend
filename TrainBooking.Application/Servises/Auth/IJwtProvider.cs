using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        int GetUserIdFromToken(string token);
    }
}