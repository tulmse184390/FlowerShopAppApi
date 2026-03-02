namespace FlowerShopApp.Application.DTOs
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }   
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }    
    }
}
