namespace FlowerShopApp.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;   
        public string? Email { get; set; }   
        public string? PhoneNumber { get; set; }    
        public string? Address { get; set; }    
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
