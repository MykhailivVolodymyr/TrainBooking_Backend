using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = user.Role
            };
        }

        //public static User FromRegisterDto(UserRegisterDto dto)
        //{
        //    return new User
        //    {
        //        Login = dto.Login,
        //        FullName = dto.FullName,
        //        PasswordHash = dto.Password,
        //        Email = dto.Email,
        //        Role = dto.Role ?? "User"
        //    };
        //}

        public static User FromLoginDto(UserLoginDto dto)
        {
            return new User
            {
                Login = dto.LoginOrEmail, // логін або email — логіку обробки реалізуй окремо
                PasswordHash = dto.Password
            };
        }
    }
}
