using FlowerShopApp.Application.DTOs.Categories;

namespace FlowerShopApp.Application.IServices
{
    public interface ICategoryService
    {
            Task<List<CategoryDto>> GetAllCategoriesAsync();
    }
}
