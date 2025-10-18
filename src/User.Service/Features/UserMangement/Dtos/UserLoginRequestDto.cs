using System.Text.Json.Serialization;

namespace User.Service.Features.UserMangement.Dtos
{
    public class UserLoginRequestDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
