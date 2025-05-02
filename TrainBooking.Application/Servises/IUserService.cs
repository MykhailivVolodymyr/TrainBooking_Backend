using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises
{
    public interface IUserService
    {
        Task<string> AddAsync(UserRegisterDto user);
        Task<string> Login(UserLoginDto user);
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByLoginAsync(string login);
        Task<UserDto?> GetUserByEmailAsync(string email);
    }
}
