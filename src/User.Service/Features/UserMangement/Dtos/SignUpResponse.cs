using System.Text.Json.Serialization;

namespace User.Service.Features.UserMangement.Dtos
{
    public class SignUpResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public SignupResponseData Data { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }

    public class SignupResponseData
    {
        [JsonPropertyName("user")]
        public UserResponseDto User { get; set; }

    }
}
