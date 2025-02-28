using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;

namespace Shoezy.Repositories
{
    public interface IWishlistRepository {
        Task<Result<string>> AddOrRemoveToWishList(int userId, Guid productId);
        Task<Result<List<WishlistGetDTO>>> GetWishList(int userId);

    }
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ShoezyDbContext context;
        private readonly IMapper mapper;
        public WishlistRepository(ShoezyDbContext _context, IMapper _mapper) {
            context = _context;
            mapper = _mapper;
        }

        public async Task<Result<string>> AddOrRemoveToWishList(int userId, Guid productId)
        {
            try {
                var productExist = await context.Products.AnyAsync(x => x.Id == productId);
                if (!productExist) {
                    return new Result<string> { StatusCode = 200, Message = "Product not exist" };
                }

                var existInWishlist = await context.Wishlists
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);

                if (existInWishlist != null)
                {

                    context.Wishlists.Remove(existInWishlist);
                    await context.SaveChangesAsync();
                    return new Result<string> { StatusCode = 200, Message = "Product removed from wishlist" };
                }
                else
                {
                    var newItem = new Wishlist
                    {
                        ProductId = productId,
                        UserId = userId
                    };

                    await context.Wishlists.AddAsync(newItem);
                    await context.SaveChangesAsync();
                    return new Result<string> { StatusCode = 200, Message = "Product added to wishlist" };
                }

            } catch (Exception ex) {
                return new Result<string> { StatusCode = 500, Message = ex.Message };
            }
        }
    

    public async Task<Result<List<WishlistGetDTO>>> GetWishList(int userId) {
            try {
                if (userId == 0) {
                    return new Result<List<WishlistGetDTO>> { StatusCode = 401, Message = "user not authorized" };
                }
                var response = await context.Wishlists.Include(x => x.products).Where(x => x.UserId == userId).ToListAsync();
                if (response.Count > 0) {
                    var products = response.Select(x => new WishlistGetDTO {
                        WishListId = x.WishListId,
                        Id = x.ProductId,
                        Title = x.products.Title,
                        Image = x.products.Image,
                        Price = x.products.Price,
                        Brand = x.products.Brand,
                        Model = x.products.Model,
                        Color = x.products.Color,

                    }).ToList();
                    return new Result<List<WishlistGetDTO>> { StatusCode = 200, Message = "GetWishlist success", Data = products };
                }
                return new Result<List<WishlistGetDTO>> { StatusCode = 200, Message = "Wishlist is emplty" };

            } catch (Exception ex) {
                return new Result<List<WishlistGetDTO>> { StatusCode = 500, Message = ex.Message };
            }
        }
    } 
}
