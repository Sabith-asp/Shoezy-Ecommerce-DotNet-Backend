using System.Collections.Generic;
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
    public class CartController : ControllerBase
    {
        private readonly ICartService cartservice;
        public CartController(ICartService _service) {
            cartservice= _service;
        }

        [HttpPost("add-to-cart")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddToCart(Guid productId) {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response=await cartservice.AddToCart(userId, productId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetAllInCart()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
                
            }
            var response = await cartservice.GetAllInCart(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("remove/{cartItemId}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> RemoveFromCart( Guid cartItemId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");

            }
            var result = await cartservice.RemoveFromCart(userId, cartItemId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("remove-all")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RemoveAllFromCart()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");

            }
            var result = await cartservice.RemoveAllFromCart(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("increase-qty/{cartItemId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> IncreaseQty( Guid cartItemId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");

            }
            var result = await cartservice.IncreaseQty(userId, cartItemId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("decrease-qty/{cartItemId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DecreaseQty( Guid cartItemId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");

            }
            var result = await cartservice.DecreaseQty(userId, cartItemId);
            return StatusCode(result.StatusCode, result);
        }



    }
}
