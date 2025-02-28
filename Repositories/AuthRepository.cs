using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.JWT;
using Shoezy.Models;

namespace Shoezy.Repositories
{
    public interface IAuthRepository
    {
        Task<Result<object>> SignUp(RegisterDTO regdata);
        Task<Result<object>> Login(LoginDTO logindata);

        Task<Result<object>> RefreshToken(RefreshDTO reftoken);
    }
    public class AuthRepository : IAuthRepository
    {
        private readonly ShoezyDbContext context;
        private readonly IMapper mapper;
        private readonly IJWTGenerator jwttoken;
        public AuthRepository(ShoezyDbContext _context, IMapper _mapper,IJWTGenerator jwtgen)
        {
            context = _context;
            mapper = _mapper;
            jwttoken = jwtgen;
        }
        public async Task<Result<object>> SignUp(RegisterDTO regdata)
        {
            
           

            try
            {
                var existing = await context.Users.FirstOrDefaultAsync(x => x.UserName == regdata.Username && x.Email==regdata.Email);

                if (existing != null)
                {
                    return new Result<object> { StatusCode = 409, Message = "User Already exist" };
                }
                
                var data = mapper.Map<User>(regdata);
                data.PasswordHash = Hashpassword(regdata.Password);
                await context.Users.AddAsync(data);
                await context.SaveChangesAsync();
                return new Result<object> { StatusCode = 200, Message = "User Registered Succesfully" };

            }
            catch (Exception ex) { 
                return new Result<object>{ Error = ex.Message,StatusCode=500 };
            }
        }

        public async Task<Result<object>> Login(LoginDTO logindata) {
            try {
                var data = await context.Users.FirstOrDefaultAsync(user=>user.UserName==logindata.UserName);
               
                if (data == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "User doesnt exist" };
                }
                if (data.IsBlocked == true)
                {
                    return new Result<object> { StatusCode = 403, Message = "User is blocked by admin" };
                }
                var verifypass = Verifypassword(logindata.Password,data.PasswordHash);
                if (!verifypass) {
                    return new Result<object> { StatusCode=401,Message="Invalid Password"};
                }

                var Token = jwttoken.GetToken(data);

                var refreshToken = GenerateRefreshToken();
                data.RefreshToken = refreshToken;
                data.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); 
                await context.SaveChangesAsync();
                return new Result<object>
                {
                    StatusCode = 200,
                    Message = "Token Generated Successfully",
                    Data = new
                    {
                        AccessToken = Token,
                        RefreshToken = refreshToken
                    }
                };

            }
            catch (Exception ex) {
                return new Result<object> { StatusCode = 500,Message=ex.Message };
            }
        }


        public async Task<Result<object>> RefreshToken(RefreshDTO reftoken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == reftoken.refreshtoken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return new Result<object> { StatusCode = 401, Message = "Invalid or expired refresh token" };
            }

            var newAccessToken = jwttoken.GetToken(user);
            var newRefreshToken = GenerateRefreshToken();


            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();

            return new Result<object>
            {
                StatusCode = 200,
                Message = "Token Refreshed Successfully",
                Data = new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };
        }

        public string Hashpassword(string password)
        {
            string hasshed_password = BCrypt.Net.BCrypt.HashPassword(password);
            return hasshed_password;
        }

        public bool Verifypassword(string password, string hashedpassword)
        {
            bool verifiedpassword = BCrypt.Net.BCrypt.Verify(password, hashedpassword);
            return verifiedpassword;
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }




    }
}
