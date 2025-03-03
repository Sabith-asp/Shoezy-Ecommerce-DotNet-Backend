using AutoMapper;
using Shoezy.DTOs;
using Shoezy.Models;

namespace Shoezy.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile() {
            CreateMap<RegisterDTO, User>().ForMember(dest=>dest.Id,opt=>opt.Ignore());
            CreateMap<ProductDTO, Product>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<WishlistAddRemoveDTO, Wishlist>().ForMember(dest => dest.WishListId, opt => opt.Ignore());
            CreateMap<Product, ProductGetDTO>().ForMember(dest=>dest.Category,opt=>opt.MapFrom(src=>src.Category.Name));
            CreateMap<Product, ProductGetAdminDTO>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Address, GetAddressDTO>();
            CreateMap<AddressCreateDTO, Address>();
            CreateMap<CartItem, CartViewDTO>().ForMember(dest=>dest.ProductName,opt=>opt.MapFrom(src=>src.product.Title)).ForMember(dest=>dest.Price,opt=>opt.MapFrom(src=>src.product.Price)).ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.product.Image)).ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Quantity*src.product.Price));
            CreateMap<OrderItem, OrderViewDTO>().ForMember(dest=>dest.TotalAmount,opt=>opt.MapFrom(src=>src.TotalPrice)).ForMember(dest=>dest.ProductName,opt=>opt.MapFrom(src=>src.Product.Title)).ForMember(dest=>dest.Image,opt=>opt.MapFrom(src=>src.Product.Image)).ForMember(dest=>dest.Price,opt=>opt.MapFrom(src=>src.Product.Price));
            CreateMap<AddProductDTO, ProductDTO>();
            CreateMap<AddProductDTO, Product>();
            CreateMap<User, GetUserDTO>();
            CreateMap<Address,AddressViewDTO>();
            CreateMap<AddCategoryDTO, Category>();
            CreateMap<Category, GetCategoryDTO>();
        }
    }
}
