namespace FlowerShopApp.Application.DTOs.Products
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? BriefDescription { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; } 
    }
}