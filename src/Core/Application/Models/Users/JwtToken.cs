using System.Text.Json.Serialization;

namespace Application.Models.Users
{
    public class JwtToken
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime RefreshTokenExpiresOn { get; set; }
    }
}
