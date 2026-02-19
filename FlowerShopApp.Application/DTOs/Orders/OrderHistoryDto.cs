namespace FlowerShopApp.Application.DTOs.Orders
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = null!; 
        public string PaymentMethod { get; set; } = null!;
        public int ItemCount { get; set; } 
        public string? Thumbnail { get; set; } 
    }
}
