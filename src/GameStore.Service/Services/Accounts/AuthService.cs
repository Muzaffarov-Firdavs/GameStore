﻿using GameStore.Domain.Entities.Users;
using GameStore.Domain.Enums;
using GameStore.Service.Interfaces.Accounts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameStore.Service.Services.Accounts
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration config)
        {
            this._configuration = config.GetSection("Jwt");
        }

        public string GenerateToken(User user, Role role)
        {
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Role", $"{role}")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new JwtSecurityToken(_configuration["Issuer"], _configuration["Audience"], claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Lifetime"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
