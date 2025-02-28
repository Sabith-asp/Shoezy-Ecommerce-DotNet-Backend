using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Services;

namespace Shoezy.Controllers
{
    [Route("api/users/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authservice;
        //private readonly ShoezyDbContext context;
        public AuthController(IAuthService _authservice) { 
        authservice = _authservice;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO regdata) { 
           var response=await authservice.SignUp(regdata);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO logindata) { 
            var response=await authservice.Login(logindata);
            return StatusCode(response.StatusCode, response);

        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshDTO reftoken)
        {
            var response = await authservice.RefreshToken(reftoken);
            return StatusCode(response.StatusCode, response);
        }

    }
}
