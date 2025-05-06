using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;
using TrainBooking.Application.Mappers;
using TrainBooking.Application.Servises.Auth;
using TrainBooking.Domain.Abstractions;
using TrainBooking.Domain.Models;

namespace TrainBooking.Application.Servises.Imp
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> AddAsync(UserRegisterDto userDto)
        {
            // Перевірка унікальності логіна або email
            var existingByLogin = await _userRepository.GetUserByLoginAsync(userDto.Login);
            var existingByEmail = await _userRepository.GetUserByEmailAsync(userDto.Email);

            if (existingByLogin != null)
                throw new ArgumentException("Користувач з таким логіном вже існує");

            if (existingByEmail != null)
                throw new ArgumentException("Користувач з таким email вже існує");

            string passwordHash = _passwordHasher.HashPassword(userDto.Password);
            var user = new User()
            {
                Login = userDto.Login,
                FullName = userDto.FullName,
                PasswordHash = passwordHash,
                Email = userDto.Email,
            };

            await _userRepository.AddAsync(user);

            var token = _jwtProvider.GenerateToken(user);
            return token;
        }


        public async Task<LoginResultDto> Login(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(userLoginDto.LoginOrEmail);

            if (user == null)
                user = await _userRepository.GetUserByLoginAsync(userLoginDto.LoginOrEmail);

            if (user == null)
                throw new UnauthorizedAccessException("Користувача не знайдено");

            var result = _passwordHasher.VerifyPassword(userLoginDto.Password, user.PasswordHash);

            if (!result)
                throw new UnauthorizedAccessException("Невірний пароль");

            var token = _jwtProvider.GenerateToken(user);

            return new LoginResultDto   
            {
                Token = token,
                Role = user.Role,         
                FullName = user.FullName  
            };
        }



        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user != null ? UserMapper.ToUserDto(user) : null;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user != null ? UserMapper.ToUserDto(user) : null;
        }

        public async Task<UserDto?> GetUserByLoginAsync(string login)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);
            return user != null ? UserMapper.ToUserDto(user) : null;
        }
    }
}
