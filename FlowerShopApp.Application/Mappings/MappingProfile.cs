using AutoMapper;
using FlowerShopApp.Application.DTOs.Auth;
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
        }
    }
}
