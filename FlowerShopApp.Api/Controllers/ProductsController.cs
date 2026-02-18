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
            return Ok(products);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetProductDetail(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "The product does not exist!" });
            }

            return Ok(product);
        }
    }
}
