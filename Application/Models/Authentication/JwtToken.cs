using System.Text.Json.Serialization;

namespace Application.Models.Authentication
{
    public class JwtToken
    {
        public string Token { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime RefreshTokenExpiresOn { get; set; }
    }
}
