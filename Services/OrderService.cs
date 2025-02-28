using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Repositories;
using System.Threading.Tasks;

namespace Shoezy.Services
{
    public interface IOrderService
    {
        Task<Result<object>> RazorOrderCreate(int price);
        Task<Result<bool>> RazorPayment(PaymentDTO payment);
        Task<Result<object>> CreateOrder(int userId, CreateOrderDTO createOrderDTO);

        Task<Result<List<ViewUserOrderDetailDTO>>> GetOrderDetails(int userId);
        Task<Result<List<ViewUserOrderDetailDTO>>> GetAllOrders();
        Task<Result<object>> GetRevenue();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepo;

        public OrderService(IOrderRepository _orderRepo)
        {
            orderRepo = _orderRepo;
        }

        public async Task<Result<object>> RazorOrderCreate(int price)
        {
            return await orderRepo.RazorOrderCreate(price);
        }

        public async Task<Result<bool>> RazorPayment(PaymentDTO payment)
        {
            return await orderRepo.RazorPayment(payment);
        }

        public async Task<Result<object>> CreateOrder(int userId, CreateOrderDTO createOrderDTO)
        {
            return await orderRepo.CreateOrder(userId, createOrderDTO);
        }

        public async Task<Result<List<ViewUserOrderDetailDTO>>> GetOrderDetails(int userId) {
            return await orderRepo.GetOrderDetails(userId);
        }

        public async Task<Result<List<ViewUserOrderDetailDTO>>> GetAllOrders() { 
            return await orderRepo.GetAllOrders();
        }

        public async Task<Result<object>> GetRevenue() {
            return await orderRepo.GetRevenue();
        }
    }
}
