using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Products;

namespace FlowerShopApp.Application.IServices
{
    public interface IProductService
    {
        Task<PagedResult<ProductDto>> GetProductsAsync(ProductParams productParams);
        Task<ProductDetailDto?> GetProductByIdAsync(int id);
    }
}
