namespace FlowerShopApp.Application.DTOs.Products
{
    public class ProductDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? BriefDescription { get; set; }
        public string? FullDescription { get; set; }
        public int StockQuantity { get; set; }
        public string? CategoryName { get; set; }


        public List<string> Images { get; set; } = new();
    }
}
