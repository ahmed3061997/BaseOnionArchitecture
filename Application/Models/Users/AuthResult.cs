namespace Application.Models.Users
{
    public class AuthResult
    {
        public JwtToken Jwt { get; set; }
        public UserDto User { get; set; }
    }
}
