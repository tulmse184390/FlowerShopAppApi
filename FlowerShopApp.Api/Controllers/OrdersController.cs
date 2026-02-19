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

                if (request.PaymentMethod == "VNPAY")
                {
                    var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, order);

                    return Ok(new
                    {
                        status = "success",
                        orderId = orderId,
                        paymentUrl = paymentUrl,
                        message = "Redirect to payment gateway"
                    });
                }

                return Ok(new
                {
                    status = "success",
                    orderId = orderId,
                    message = "Order placed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var order = await _orderService.GetOrderDetailsAsync(userId, id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
