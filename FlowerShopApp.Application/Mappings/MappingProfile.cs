using AutoMapper;
using FlowerShopApp.Application.DTOs.Auth;
using FlowerShopApp.Application.DTOs.Cart;
using FlowerShopApp.Application.DTOs.Categories;
using FlowerShopApp.Application.DTOs.Orders;
using FlowerShopApp.Application.DTOs.Products;
using FlowerShopApp.Application.DTOs.Store;
using FlowerShopApp.Domain.Entities;

namespace FlowerShopApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestDto, User>();
            CreateMap<User, AuthResponseDto>();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    src.ProductImages.FirstOrDefault(x => x.IsPrimary).ImageUrl
                    ?? src.ProductImages.FirstOrDefault().ImageUrl));

            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages.Select(x => x.ImageUrl).ToList()));

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.Product.ProductImages.FirstOrDefault(x => x.IsPrimary).ImageUrl
                    ?? src.Product.ProductImages.FirstOrDefault().ImageUrl));

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                    src.CartItems.Sum(x => x.Price * x.Quantity)));

            CreateMap<Order, OrderHistoryDto>()
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.OrderItems.Sum(x => x.Quantity)))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src =>
                    src.OrderItems.FirstOrDefault().Product.ProductImages.FirstOrDefault(x => x.IsPrimary).ImageUrl
                    ?? "https://via.placeholder.com/150"));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.Product.ProductImages.FirstOrDefault(x => x.IsPrimary).ImageUrl));

            CreateMap<Order, OrderDetailDto>();

            CreateMap<StoreLocation, StoreLocationDto>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => (double)src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => (double)src.Longitude));

            CreateMap<Category, CategoryDto>();    
        }
    }
}
