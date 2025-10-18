using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace User.Service.Features.UserMangement.Dtos
{
    public class UserSignUpDto
    {
        [Required(ErrorMessage = "Email is required")]
        [JsonPropertyName("email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [JsonPropertyName("password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one letter and one number")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [JsonPropertyName("confirm_password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
