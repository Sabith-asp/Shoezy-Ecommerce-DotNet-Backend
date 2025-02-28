using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoezy.DTOs;
using Shoezy.Services;

namespace Shoezy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService service;
        public OrderController(IOrderService _service) {
            service= _service;
        }
        [Authorize]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(int price) {
                
                var response = await service.RazorOrderCreate(price);
                return StatusCode(response.StatusCode, response);

        }
        [Authorize]
        [HttpPost("payment")]
        public async Task<IActionResult> Payment(PaymentDTO razorpay)
        {
            try
            {
                if (razorpay == null)
                {
                    return BadRequest("razorpay details connot be null here");
                }
                var res =await service.RazorPayment(razorpay);
                return StatusCode(res.StatusCode, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [Authorize]
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(CreateOrderDTO createorderdto)
        {

                var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized("User not authorized");
                }

                var res = await service.CreateOrder(userId, createorderdto);
            return StatusCode(res.StatusCode, res);

        }
        [Authorize]
        [HttpGet("get-orders")]

        public async Task<IActionResult> GetOrders() {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response= await service.GetOrderDetails(userId);
            return StatusCode(response.StatusCode,response);
        }

        [HttpGet("get-all-orders")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllOrders() {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User not authorized");
            }
            var response = await service.GetAllOrders();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-revenue")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetRevenue() { 
            var response=await service.GetRevenue();
            return StatusCode(response.StatusCode, response);
        }
    }
}
