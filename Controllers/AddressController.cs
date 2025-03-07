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

        private int? GetUserId() {
            return HttpContext.Items["UserId"] as int?;
        }

        [HttpPost("add-address")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> AddAddress(AddressCreateDTO newaddress) {
            var userId = GetUserId();
            Console.WriteLine(userId);
            if (userId == null)
            {
                return Unauthorized("User not authorized");
            }
            var response = await addressservice.AddAddress(userId.Value, newaddress);
            return StatusCode(response.StatusCode,response);
        }
        
        [HttpGet("get-all")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> GetAddress() {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User not authorized");
            }
            var response = await addressservice.GetAddress(userId.Value);
            return StatusCode(response.StatusCode,response);
        }

        [HttpDelete("remove-address/{addressid}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> RemoveAddress(Guid addressid) {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User not authorized");
            }
            var response = await addressservice.RemoveAddress(userId.Value, addressid);
            return StatusCode(response.StatusCode, response);
        }
    }
}
