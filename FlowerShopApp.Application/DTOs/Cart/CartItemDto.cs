namespace FlowerShopApp.Application.DTOs.Cart
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal SubTotal => Price * Quantity;
    }
}
