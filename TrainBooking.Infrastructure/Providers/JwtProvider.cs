using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Domain.Models;
using TrainBooking.Application.Servises.Auth;


namespace TrainBooking.Infrastructure.Providers
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }
        public string GenerateToken(User user)
        {
            var claims = new[]
                {
                    new Claim("userId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        // Отримання userId з токена
        public int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

            try
            {
                // Верифікація та декодування токена
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out var validatedToken);

                var userIdClaim = claimsPrincipal.FindFirst("userId");
                if (userIdClaim == null)
                {
                    throw new UnauthorizedAccessException("UserId claim is missing");
                }

                return int.Parse(userIdClaim.Value);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid or expired token");
            }
        }
    }
}
