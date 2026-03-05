using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Orders;
using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IVnPayService _vnPayService;

        public OrdersController(IOrderService orderService, IVnPayService vnPayService)
        {
            _orderService = orderService;
            _vnPayService = vnPayService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var orderId = await _orderService.CreateOrderAsync(userId, request);
                var order = await _orderService.GetOrderByIdAsync(orderId);

                var responseData = new CheckoutResponseDto { OrderId = orderId };

                if (request.PaymentMethod == "VNPAY")
                {
                    responseData.PaymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, order);
                    return Ok(new ApiResponse<CheckoutResponseDto>
                    {
                        Success = true,
                        Message = "Redirect to payment gateway",
                        Data = responseData
                    });
                }

                return Ok(new ApiResponse<CheckoutResponseDto>
                {
                    Success = true,
                    Message = "Order placed successfully",
                    Data = responseData
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("callback")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            await _orderService.ProcessPaymentReturnAsync(response);

            if (response.VnPayResponseCode == "00")
            {
                return Ok(new { message = "Payment successful!" });
            }
            else
            {
                return Ok(new { message = "Payment failed or cancelled!" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var orders = await _orderService.GetMyOrdersAsync(userId);
                return Ok(new ApiResponse<List<OrderHistoryDto>>
                {
                    Success = true,
                    Data = orders
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var order = await _orderService.GetOrderDetailsAsync(userId, id);
                return Ok(new ApiResponse<OrderDetailDto>
                {
                    Success = true,
                    Data = order
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
    }
}
