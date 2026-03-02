using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Auth;
using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                return Ok(new ApiResponse<AuthResponseDto>
                {
                    Success = true,
                    Message = "Sign up successfully!",
                    Data = result
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null 
                });
            }
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(new ApiResponse<AuthResponseDto>
                {
                    Success = true,
                    Message = "Sign in successfully!",
                    Data = result
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
