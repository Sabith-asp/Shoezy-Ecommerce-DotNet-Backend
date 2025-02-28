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

    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressservice;
        public AddressController(IAddressService _service) {
            addressservice = _service;
                }
        
        [HttpPost("add-address")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> AddAddress(AddressCreateDTO newaddress) {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response = await addressservice.AddAddress(userId, newaddress);
            return StatusCode(response.StatusCode,response);
        }
        
        [HttpGet("get-all")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> GetAddress() {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response = await addressservice.GetAddress(userId);
            return StatusCode(response.StatusCode,response);
        }

        [HttpDelete("remove-address/{addressid}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> RemoveAddress(Guid addressid) {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response = await addressservice.RemoveAddress(userId, addressid);
            return StatusCode(response.StatusCode, response);
        }
    }
}
