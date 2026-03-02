using System.ComponentModel.DataAnnotations;

namespace FlowerShopApp.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username is required!")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "FullName is required!"), MaxLength(100)]
        public string FullName { get; set; } = null!;   
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone number is required!"), RegularExpression("^0[0-9]*$", ErrorMessage = ("Phone number should start with 0!")), StringLength(11, MinimumLength = 10, ErrorMessage = ("Phone number should be between 10 and 11!"))]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "Address is required!"), MaxLength(255)]
        public string Address { get; set; } = null!;    
    }
}
