
namespace FlowerShopApp.Application.DTOs.Orders
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public decimal TotalAmount { get; set; }

        // Danh sách sản phẩm
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
