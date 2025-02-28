using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Services;

namespace Shoezy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(IWishlistService _service)
        {
            wishlistService = _service;
        }

        [HttpPost("add-or-remove")]
        [Authorize(Roles = "user")]


        public async Task<IActionResult> AddOrRemoveWishList(Guid productId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }

            var response=await wishlistService.AddOrRemoveToWishList(userId, productId);
            return StatusCode(response.StatusCode, response);


        }

        [HttpGet("get-all")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetWishList() {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response = await wishlistService.GetWishList(userId);
            return StatusCode(response.StatusCode, response);
        }

    }
}
