using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Models.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Roles { get; set; }

        [JsonIgnore]
        public IFormFile? ProfileImageFile { get; set; }
    }
}
