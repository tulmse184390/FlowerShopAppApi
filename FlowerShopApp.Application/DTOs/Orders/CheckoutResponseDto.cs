namespace FlowerShopApp.Application.DTOs.Orders
{
    public class CheckoutResponseDto
    {
        public int OrderId { get; set; }
        public string? PaymentUrl { get; set; }
    }
}
