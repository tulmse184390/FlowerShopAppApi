using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Cart;
using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);

                await _cartService.AddToCartAsync(userId, request);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Add to cart successfully!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized(new { message = "Invalid token." });

                int userId = int.Parse(userIdClaim.Value);

                var cart = await _cartService.GetCartByUserIdAsync(userId);
                return Ok(new ApiResponse<CartDto>
                {
                    Success = true,
                    Message = "Success",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                await _cartService.UpdateQuantityAsync(userId, request);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Cart updated successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                await _cartService.RemoveItemAsync(userId, productId);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Item removed from cart."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                await _cartService.ClearCartAsync(userId);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Cart cleared successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCartItemCount()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Ok(new { count = 0 });

                int userId = int.Parse(userIdClaim.Value);
                var count = await _cartService.GetCartItemCountAsync(userId);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = count
                });
            }
            catch (Exception)
            {
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = 0
                });
            }
        }
    }
}
