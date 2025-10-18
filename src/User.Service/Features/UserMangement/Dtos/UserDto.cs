using System.Text.Json.Serialization;

namespace User.Service.Features.UserMangement.Dtos
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        public decimal Gil { get; set; }

        //[JsonIgnore]
        //[JsonPropertyName("products")]
        //public IEnumerable<ProductDto> Products { get; set; }
    }
}
