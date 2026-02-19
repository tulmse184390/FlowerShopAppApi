
namespace FlowerShopApp.Application.DTOs.Orders
{
    public class CheckoutRequestDto
    {
        public string ShippingAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = "VNPAY";
    }
}
