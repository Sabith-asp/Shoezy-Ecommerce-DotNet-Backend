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


        private int? GetUserId()
        {
            return HttpContext.Items["UserId"] as int?;
        }


        [HttpPost("add-or-remove")]
        [Authorize(Roles = "user")]


        public async Task<IActionResult> AddOrRemoveWishList(Guid productId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User not authorized");
            }

            var response=await wishlistService.AddOrRemoveToWishList(userId.Value, productId);
            return StatusCode(response.StatusCode, response);


        }

        [HttpGet("get-all")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetWishList() {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User not authorized");
            }
            var response = await wishlistService.GetWishList(userId.Value);
            return StatusCode(response.StatusCode, response);
        }

    }
}
