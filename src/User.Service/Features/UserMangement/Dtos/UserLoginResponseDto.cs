using System.Text.Json.Serialization;

namespace User.Service.Features.UserMangement.Dtos
{
    public class UserLoginResponseDto<T>
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; } = 200;
    }

}
