﻿using System;
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
        Task<LoginResultDto> AddAsync(UserRegisterDto user);
        Task<LoginResultDto> Login(UserLoginDto user);
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByEmailAsync(string email);
    }
}
