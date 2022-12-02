namespace Domain.Models.Auth
{
    public class AuthResult
    {
        public JwtToken Token { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
