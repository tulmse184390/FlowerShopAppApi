namespace FlowerShopApp.Application.DTOs.Cart
{
    public class CartDto
    {
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; } 
        public List<CartItemDto> Items { get; set; } = new();
    }
}
