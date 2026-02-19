namespace FlowerShopApp.Application.DTOs.Store
{
    public class StoreLocationDto
    {
        public int LocationId { get; set; }
        public string StoreName { get; set; }
        public double Latitude { get; set; }  
        public double Longitude { get; set; } 
        public string Address { get; set; }
    }
}
