namespace FlowerShopApp.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
