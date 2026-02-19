using AutoMapper;
using FlowerShopApp.Application.DTOs.Orders;
using FlowerShopApp.Application.DTOs.Payment;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Domain.Entities;
using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(int userId, CheckoutRequestDto request)
        {
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var cart = await _unitOfWork.Carts.Entities
                             .Include(c => c.CartItems)
                             .ThenInclude(ci => ci.Product)
                             .FirstOrDefaultAsync(c => c.UserId == userId && c.Status.ToLower() == "ACTIVE".ToLower());

                if (cart == null || !cart.CartItems.Any())
                    throw new Exception("Empty cart!");

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = request.ShippingAddress,
                    PaymentMethod = request.PaymentMethod,
                    OrderStatus = "PENDING",
                    TotalAmount = cart.CartItems.Sum(x => x.Quantity * x.Price)
                };

                foreach (var item in cart.CartItems)
                {
                    if (item.Product.StockQuantity < item.Quantity)
                    {
                        throw new Exception($"The product {item.Product.ProductName} is sold out!");
                    }

                    item.Product.StockQuantity -= item.Quantity;

                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    order.OrderItems.Add(orderItem);
                }

                _unitOfWork.Orders.Add(order);
                await _unitOfWork.CompleteAsync();

                cart.CartItems.Clear();

                await _unitOfWork.CompleteAsync();
                transaction.Commit();

                return order.OrderId;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new Exception("Order not found!");
            }

            return order;
        }

        public async Task ProcessPaymentReturnAsync(PaymentResponseDto response)
        {
            var orderId = int.Parse(response.OrderId);
            var order = await _unitOfWork.Orders.Entities
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (order == null) throw new Exception("Order not found");

            if (order.OrderStatus != "PENDING") return;

            if (response.VnPayResponseCode == "00")
            {
                order.OrderStatus = "PAID";
            }
            else
            {
                order.OrderStatus = "CANCELLED";
                foreach (var item in order.OrderItems)
                {
                    var product = item.Product;
                    product.StockQuantity += item.Quantity;
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<OrderHistoryDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.Entities
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductImages)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate) 
                .ToListAsync();

            return _mapper.Map<List<OrderHistoryDto>>(orders);
        }

        public async Task<OrderDetailDto> GetOrderDetailsAsync(int userId, int orderId)
        {
            var order = await _unitOfWork.Orders.Entities
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) throw new Exception("Order not found");

            // BẢO MẬT: Kiểm tra chính chủ
            if (order.UserId != userId)
                throw new Exception("Unauthorized access to this order.");

            return _mapper.Map<OrderDetailDto>(order);
        }
    }
}
