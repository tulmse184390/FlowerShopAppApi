namespace FlowerShopApp.Application.DTOs.Products
{
    public class ProductParams
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public int PageIndex { get; set; } = 1; 
        public int PageSize { get; set; } = 6; 
    }
}