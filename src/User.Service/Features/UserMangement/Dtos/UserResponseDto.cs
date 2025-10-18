using System.Text.Json.Serialization;

namespace User.Service.Features.UserMangement.Dtos
{
    public class UserResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("is_superadmin")]
        public bool IsSuperAdmin { get; set; }
    }
}
