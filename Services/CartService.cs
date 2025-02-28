using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Repositories;

namespace Shoezy.Services
{
    public interface ICartService
    {
        Task<Result<object>> AddToCart(int userId,Guid productId);
        Task<Result<CartResDTO>> GetAllInCart(int userId);
        Task<Result<object>> RemoveFromCart(int UserId, Guid cartitemId);
        Task<Result<object>> RemoveAllFromCart(int UserId);
        Task<Result<object>> IncreaseQty(int UserId, Guid cartitemId);
        Task<Result<object>> DecreaseQty(int UserId, Guid cartitemId);
    }
    public class CartService : ICartService
    {
        private readonly ICartRepository cartrepo;
        public CartService(ICartRepository _cartrepo) {
            cartrepo = _cartrepo;
        }
        public async Task<Result<object>> AddToCart(int userId,Guid productId) {
            return await cartrepo.AddToCart(userId, productId);
        }
        public async Task<Result<CartResDTO>> GetAllInCart(int userId)
        {
            return await cartrepo.GetAllInCart(userId);
        }

        public async Task<Result<object>> RemoveFromCart(int UserId, Guid cartitemId) {
            return await cartrepo.RemoveFromCart(UserId, cartitemId);
        }
        public async Task<Result<object>> RemoveAllFromCart(int UserId) {
            return await cartrepo.RemoveAllFromCart(UserId);
        }
        public async Task<Result<object>> IncreaseQty(int UserId, Guid cartitemId) {
            return await cartrepo.IncreaseQty(UserId, cartitemId);
        }
        public async Task<Result<object>> DecreaseQty(int UserId, Guid cartitemId) {
            return await cartrepo.DecreaseQty(UserId, cartitemId);
        }
    }

}
