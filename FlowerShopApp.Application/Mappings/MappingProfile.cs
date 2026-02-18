using AutoMapper;
using FlowerShopApp.Application.DTOs.Auth;
using FlowerShopApp.Application.DTOs.Cart;
using FlowerShopApp.Application.DTOs.Products;
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
        }
    }
}
