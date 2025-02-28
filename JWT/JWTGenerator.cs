﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shoezy.DTOs;
using Shoezy.Models;

namespace Shoezy.JWT
{
    public interface IJWTGenerator {
        string GetToken(User userdata);
    }
         
    public class JWTGenerator:IJWTGenerator
    {
        private readonly IConfiguration _config;
        public JWTGenerator(IConfiguration config) {
            _config = config;
        }

        public string GetToken(User userdata) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userdata.Id.ToString()), 
                new Claim(ClaimTypes.Name, userdata.Name),                  
                new Claim(ClaimTypes.Role, userdata.Role)                  
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
