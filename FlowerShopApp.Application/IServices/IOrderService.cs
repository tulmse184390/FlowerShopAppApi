using FlowerShopApp.Application.DTOs.Orders;
using FlowerShopApp.Application.DTOs.Payment;
using FlowerShopApp.Domain.Entities;

namespace FlowerShopApp.Application.IServices
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(int userId, CheckoutRequestDto request);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task ProcessPaymentReturnAsync(PaymentResponseDto response);
        Task<List<OrderHistoryDto>> GetMyOrdersAsync(int userId);
        Task<OrderDetailDto> GetOrderDetailsAsync(int userId, int orderId);
    }
}
