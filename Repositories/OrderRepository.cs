using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Razorpay.Api;
using AutoMapper;

namespace Shoezy.Repositories
{
    public interface IOrderRepository {
        Task<Result<object>> RazorOrderCreate(int price);
        Task<Result<bool>> RazorPayment(PaymentDTO payment);
        Task<Result<object>> CreateOrder(int userId, CreateOrderDTO createOrderDTO);

        Task<Result<List<ViewUserOrderDetailDTO>>> GetOrderDetails(int userId);

        Task<Result<List<ViewUserOrderDetailDTO>>> GetAllOrders();

        Task<Result<object>> GetRevenue();

    }
    public class OrderRepository : IOrderRepository
    {
        private readonly ShoezyDbContext context;
        private readonly IMapper mapper;
        public OrderRepository(ShoezyDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }
        public async Task<Result<object>> RazorOrderCreate(int price)
        {
            try
            {
                if (price <= 0)

                {
                    return new Result<object> { StatusCode = 400, Message = "enter valid price" };
                }
                Dictionary<string, object> input = new Dictionary<string, object>
                {
                    { "amount", price * 100 }, // Razorpay expects price in paise (multiply by 100)
                    { "currency", "INR" }, // Currency is INR
                    { "receipt", Guid.NewGuid().ToString() } // Unique receipt ID for the order
                };

                // Hardcoded Razorpay credentials
                string key = "rzp_test_JChdnCdqvPMIoB"; // Replace with your Razorpay KeyId
                string secret = "cbz6sk8xnH9H14SpJ4Io8oke"; // Replace with your Razorpay KeySecret
                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                string orderId = order["id"].ToString();

                return new Result<object> { StatusCode = 200, Message = "OrderId Created Successfully", Data = orderId };
            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 200, Message = ex.Message };
            }
        }

        // Razorpay Payment Verification
        public async Task<Result<bool>> RazorPayment(PaymentDTO payment)
        {

            if (payment == null ||
        string.IsNullOrEmpty(payment.razorpay_payment_id) ||
        string.IsNullOrEmpty(payment.razorpay_order_id) ||
        string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return new Result<bool> { StatusCode = 400, Message = "Credentials not found" };
            }

            try
            {
                // Replace with your Razorpay credentials
                string key = "rzp_test_iA2stFg1qD86OQ";
                string secret = "B442j5qkUCP0WrsGGgHBG6F8";

                Dictionary<string, string> attributes = new Dictionary<string, string>
        {
            { "razorpay_payment_id", payment.razorpay_payment_id },
            { "razorpay_order_id", payment.razorpay_order_id },
            { "razorpay_signature", payment.razorpay_signature },
            { "secret", secret }  // Include the secret key inside the dictionary
        };

                // Use Razorpay's built-in signature verification (with one parameter)
                Utils.verifyPaymentSignature(attributes);

                return new Result<bool> { StatusCode = 200, Message = "Payment success", Data = true };
            }
            catch (Razorpay.Api.Errors.SignatureVerificationError ex)
            {
                return new Result<bool> { StatusCode = 400, Message = "Invalid signature: " + ex.Message };
            }
            catch (Exception ex)
            {
                return new Result<bool> { StatusCode = 400, Message = "Error while verifying Razorpay payment: " + ex.Message };
            }


        }

        private string GenerateSignature(string paymentId, string orderId, string secret)
        {
            string stringToSign = orderId + "|" + paymentId;
            Console.WriteLine("String to Sign: " + stringToSign);

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                string generatedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                Console.WriteLine("Generated Signature: " + generatedSignature);
                return generatedSignature;
            }
        }



        //private string GenerateSignature(string paymentId, string orderId, string secret)
        //{
        //    string stringToSign = orderId + "|" + paymentId;

        //    using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
        //    {
        //        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
        //        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //    }
        //}


        //private string GenerateSignature(string paymentId, string orderId, string secret)
        //{
        //    // Razorpay signature generation formula
        //    string stringToSign = orderId + "|" + paymentId;
        //    var hmac = new HMACSHA256();
        //    hmac.Key = Encoding.ASCII.GetBytes(secret);
        //    var hashBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(stringToSign));
        //    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //}

        public async Task<Result<object>> CreateOrder(int userId, CreateOrderDTO createOrderDTO)
        {
            try
            {
                var adressexist = await context.Addresses.FirstOrDefaultAsync(a => a.AddressId == createOrderDTO.AddressId && a.UserId == userId);
                if (adressexist == null) { return new Result<object> { StatusCode = 400, Message = "Address is wrong" }; }
                var cart = await context.cart.Include(c => c.cartItem).ThenInclude(c => c.product).FirstOrDefaultAsync(x => x.UserId == userId);
                if (cart == null)
                {
                    return new Result<object> { StatusCode = 400, Message = "Cart is empty." };
                }

                var order = new Shoezy.Models.Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    AddressId = createOrderDTO.AddressId,
                    TotalPrice = createOrderDTO.Totalamount,
                    TransactionId = createOrderDTO.TransactionId,
                    OrderItems = cart.cartItem.Select(c => new OrderItem
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        TotalPrice = c.Quantity * c.product.Price
                    }).ToList(),
                };


                foreach (var cartItem in cart.cartItem)
                {
                    var product = await context.Products.FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);
                    if (product != null && product.Quantity < cartItem.Quantity)
                    {
                        return new Result<object> { StatusCode = 404, Message = "No stock available" };
                    }
                    product.Quantity -= cartItem.Quantity;
                }

                await context.Orders.AddAsync(order);
                context.cart.Remove(cart);
                await context.SaveChangesAsync();

                return new Result<object> { StatusCode = 200, Message = "Order created successfully" };
            }
            catch (DbUpdateException ex)
            {
                return new Result<object> { StatusCode = 500, Message = "Database update failed: " + ex.InnerException?.Message };
            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = "Error creating order: " + ex.Message };
            }
        }



        public async Task<Result<List<ViewUserOrderDetailDTO>>> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await context.Orders.Include(o => o.Address).Include(i => i.OrderItems)
                                .ThenInclude(i => i.Product)
                                .Where(i => i.UserId == userId)
                                .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return new Result<List<ViewUserOrderDetailDTO>> { StatusCode = 200, Message = "No orders found" };
                }


                var orderdetails = orders.Select(i => new ViewUserOrderDetailDTO
                {
                    Id = i.UserId,
                    OrderId = i.OrderId,
                    TotalPrice = i.OrderItems.Sum(x => x.TotalPrice),
                    OrderDate = i.OrderDate,
                    TransactionId = i.TransactionId,
                    Address= mapper.Map<AddressViewDTO>(i.Address),
                    OrderProducts = mapper.Map<List<OrderViewDTO>>(i.OrderItems.ToList())

                }).ToList();

                return new Result<List<ViewUserOrderDetailDTO>>
                {
                    StatusCode = 200, Message = "Order retrieved successfully", Data = orderdetails
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ViewUserOrderDetailDTO>>
                {
                    StatusCode = 200,
                    Message = "Error retrieving order details: " + ex.Message
                };
            }
        }



        //admin

        public async Task<Result<List<ViewUserOrderDetailDTO>>> GetAllOrders() {
            var orders = await context.Orders.Include(o=>o.Address).Include(o => o.OrderItems).ThenInclude(o => o.Product).ToListAsync();
            if (orders == null || !orders.Any()) {
                return new Result<List<ViewUserOrderDetailDTO>> { StatusCode = 404, Message = "No orders found" };
            }
            var orderdetails = orders.Select(i => new ViewUserOrderDetailDTO
            {
                Id = i.UserId,
                OrderId = i.OrderId,
                TotalPrice = i.OrderItems.Sum(x => x.TotalPrice),
                OrderDate = i.OrderDate,
                Address = mapper.Map<AddressViewDTO>(i.Address),
                TransactionId = i.TransactionId,
                OrderProducts = mapper.Map<List<OrderViewDTO>>(i.OrderItems.ToList())

            }).ToList();
            return new Result<List<ViewUserOrderDetailDTO>> { StatusCode = 200, Message = "Orders retreived successfully", Data = orderdetails };
        }


        public async Task<Result<object>> GetRevenue(){
            var data= await context.OrderItems.Include(x=>x.Product).ToListAsync();
            var amount = data.Sum(x => x.TotalPrice);
            return new Result<object> { StatusCode = 200,Message="Revenue retrieved successfully", Data = amount };
        }
    }
}
