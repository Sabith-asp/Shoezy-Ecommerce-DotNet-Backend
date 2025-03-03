using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Services;

namespace Shoezy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;
        public UserController(IUserService _service) { 
        service= _service;
        }
        [HttpGet("get-all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers() {
            var response=await service.GetUsers();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserById(int id) { 
            var response=await service.GetUserById(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("get-user")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetUser() {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response=await service.GetUser(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("block-or-unblock/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BlockOrUnblockUser(int id) {
            var response = await service.BlockOrUnblockUser(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
