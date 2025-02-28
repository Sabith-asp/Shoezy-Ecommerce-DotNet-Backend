using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Repositories;

namespace Shoezy.Services
{
    public interface IWishlistService
    {
        Task<Result<string>> AddOrRemoveToWishList(int userId,Guid productId);
        Task<Result<List<WishlistGetDTO>>> GetWishList(int userId);

    }
    public class WishlistService: IWishlistService
    {
        private readonly IWishlistRepository repository;
        public WishlistService(IWishlistRepository _wishrepo) { 
            repository = _wishrepo;
        }
        public async Task<Result<string>> AddOrRemoveToWishList(int userId,Guid productId) {
            return await repository.AddOrRemoveToWishList(userId, productId);
        }


        public async Task<Result<List<WishlistGetDTO>>> GetWishList(int userId) {
            return await repository.GetWishList(userId);
        }
    }
}
