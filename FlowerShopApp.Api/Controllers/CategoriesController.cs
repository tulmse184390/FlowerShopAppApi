using FlowerShopApp.Application.DTOs.Products;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Application.Services;
using Microsoft.AspNetCore.Http;
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
            var products = await _categoryService.GetAllCategoriesAsync();
            return Ok(products);
        }
    }
}
