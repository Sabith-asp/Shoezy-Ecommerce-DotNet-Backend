using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;

namespace Shoezy.Repositories
{
    public interface ICartRepository
    {
        Task<Result<object>> AddToCart(int userId, Guid productId);
        Task<Result<CartResDTO>> GetAllInCart(int userId);

        Task<Result<object>> RemoveFromCart(int UserId,Guid cartitemId);
        Task<Result<object>> RemoveAllFromCart(int UserId);
        Task<Result<object>> IncreaseQty(int UserId,Guid cartitemId);
        Task<Result<object>> DecreaseQty(int UserId,Guid cartitemId);
    }
    public class CartRepository:ICartRepository
    {
        private readonly ShoezyDbContext context;
        private readonly IMapper mapper;
        public CartRepository(ShoezyDbContext _context,IMapper _mapper) {
            context = _context;
            mapper = _mapper;
        }
        public async Task<Result<object>> AddToCart(int userId, Guid productId) {
            try
            {
                var user = context.Users.Include(x => x.cart).ThenInclude(x => x.cartItem).ThenInclude(x => x.product).FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "User not found" };
                }
                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if (product == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "Product not found" };
                }
                if (product?.Quantity == 0) {
                    return new Result<object> { StatusCode = 200, Message = "Product out of stock" };
                }
                if (user != null && product != null) {
                    if (user.cart == null) {
                        user.cart = new Cart
                        {
                            UserId = userId,
                            cartItem = new List<CartItem>()
                        };
                        await context.cart.AddAsync(user.cart);
                        await context.SaveChangesAsync();
                    }
                    var exist = user.cart.cartItem.FirstOrDefault(i=>i.ProductId== productId);
                    if (exist != null) {
                        if (exist.Quantity > product.Quantity) {
                            return new Result<object> { StatusCode = 400, Message = "Out of stock" };
                        }
                        if(exist.Quantity < product.Quantity) {
                            exist.Quantity++;
                            await context.SaveChangesAsync();
                            return new Result<object> { StatusCode = 200, Message = "Quantity increased" };
                        };
                    }
                }
                var newitem = new CartItem
                {
                    ProductId = productId,
                    CartId = user.cart.CartId,
                    Quantity = 1
                };
                user.cart.cartItem.Add(newitem);
                context.SaveChanges();
                return new Result<object> { StatusCode = 200, Message = "Product added to cart" };
            } 
            catch (Exception ex) {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }
        
        public async Task<Result<CartResDTO>> GetAllInCart(int userId)
        {
            try {
                var data = await context.cart.Include(x => x.cartItem).ThenInclude(x => x.product).FirstOrDefaultAsync(c=>c.UserId== userId);
                if (data == null) {
                    return new Result<CartResDTO> { StatusCode = 200, Message = "Cart is empty" };
                }

                var cartItems = data.cartItem.ToList();
                var mappeddata = mapper.Map<List<CartViewDTO>>(cartItems);
                var totalItems = mappeddata.Count();
                var totalPrices = mappeddata.Sum(x=>x.TotalAmount);
                var finalcart = new CartResDTO {
                    totalItem = totalItems,
                    totalPrice = totalPrices,
                    cartitems = mappeddata,
                };
                return new Result<CartResDTO> { StatusCode = 200, Message = "getcartitem success", Data = finalcart };
            } catch (Exception ex) {
                return new Result<CartResDTO> { StatusCode = 500, Message = ex.Message };
            }
        }



        public async Task<Result<object>> RemoveFromCart(int userId, Guid cartItemId)
        {
            try
            {
                var userCart = await context.cart
                    .Include(c => c.cartItem)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (userCart == null || userCart.cartItem == null || !userCart.cartItem.Any())
                {
                    return new Result<object> { StatusCode = 404, Message = "Cart is empty" };
                }

                var itemToRemove = userCart.cartItem.FirstOrDefault(i => i.CartItemId == cartItemId);
                if (itemToRemove == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "Cart item not found" };
                }

                context.cartitem.Remove(itemToRemove);
                await context.SaveChangesAsync();

                return new Result<object> { StatusCode = 200, Message = "Item removed from cart" };
            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<object>> RemoveAllFromCart(int userId)
        {
            try
            {
                var userCart = await context.cart
                    .Include(c => c.cartItem)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (userCart == null || userCart.cartItem == null || !userCart.cartItem.Any())
                {
                    return new Result<object> { StatusCode = 404, Message = "Cart is already empty" };
                }

                context.cartitem.RemoveRange(userCart.cartItem);
                await context.SaveChangesAsync();

                return new Result<object> { StatusCode = 200, Message = "All items removed from cart" };
            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<object>> IncreaseQty(int userId, Guid cartItemId)
        {
            try
            {
                var userCart = await context.cart
                    .Include(c => c.cartItem)
                    .ThenInclude(ci => ci.product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (userCart == null || userCart.cartItem == null || !userCart.cartItem.Any())
                {
                    return new Result<object> { StatusCode = 404, Message = "Cart is empty" };
                }

                var cartItem = userCart.cartItem.FirstOrDefault(i => i.CartItemId == cartItemId);
                if (cartItem == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "Cart item not found" };
                }

                if (cartItem.Quantity >= cartItem.product.Quantity)
                {
                    return new Result<object> { StatusCode = 400, Message = "Cannot add more, out of stock" };
                }

                cartItem.Quantity++;
                await context.SaveChangesAsync();

                return new Result<object> { StatusCode = 200, Message = "Quantity increased" };
            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }


        

        public async Task<Result<object>> DecreaseQty(int userId, Guid cartItemId)
        {
            try
            {
                var userCart = await context.cart
                    .Include(c => c.cartItem)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (userCart == null || userCart.cartItem == null || !userCart.cartItem.Any())
                {
                    return new Result<object> { StatusCode = 404, Message = "Cart is empty" };
                }

                var cartItem = userCart.cartItem.FirstOrDefault(i => i.CartItemId == cartItemId);
                if (cartItem == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "Cart item not found" };
                }

                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    await context.SaveChangesAsync();
                    return new Result<object> { StatusCode = 200, Message = "Quantity decreased" };
                }

                context.cartitem.Remove(cartItem);
                await context.SaveChangesAsync();

                return new Result<object> { StatusCode = 200, Message = "Item removed from cart" };
            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }

    }
}
