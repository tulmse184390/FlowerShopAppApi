using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Products;
using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams paramsDto)
        {
            var products = await _productService.GetProductsAsync(paramsDto);
            return Ok(new ApiResponse<PagedResult<ProductDto>>
            {
                Success = true,
                Message = "Get a list of products successfullly!",
                Data = products
            });
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetProductDetail(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ApiResponse<ProductDetailDto>
                {
                    Success = false,
                    Message = "The product does not exist!",
                    Data = null
                });
            }

            return Ok(new ApiResponse<ProductDetailDto>
            {
                Success = true,
                Message = "Successfully retrieved product details!",
                Data = product
            });
        }
    }
}
