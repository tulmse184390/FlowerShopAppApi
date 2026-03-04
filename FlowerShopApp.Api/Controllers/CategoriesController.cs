using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Categories;
using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var cates = await _categoryService.GetAllCategoriesAsync();
            return Ok(new ApiResponse<List<CategoryDto>>
            {
                Success = true,
                Message = "Get a list of categories successfully!",
                Data = cates
            });
        }
    }
}
