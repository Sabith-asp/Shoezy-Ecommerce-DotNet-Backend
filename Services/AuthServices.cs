using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Repositories;

namespace Shoezy.Services
{
    public interface IAuthService
    {
        Task<Result<object>> SignUp(RegisterDTO regdata);
        Task<Result<object>> Login(LoginDTO logindata);

        Task<Result<object>> RefreshToken(RefreshDTO reftoken);
    }

    public class AuthServices : IAuthService
    {
        private readonly IAuthRepository authrepository;

        public AuthServices(IAuthRepository _authrepository, IMapper _mapper)
        {
            authrepository = _authrepository;

        }

        public async Task<Result<object>> SignUp(RegisterDTO regdata)
        {
            return await authrepository.SignUp(regdata);

        }
        public async Task<Result<object>> Login(LoginDTO logindata)
        {
            return await authrepository.Login(logindata);
        }

        public async Task<Result<object>> RefreshToken(RefreshDTO reftoken)
        {

            return await authrepository.RefreshToken(reftoken);
        }


    }
}
