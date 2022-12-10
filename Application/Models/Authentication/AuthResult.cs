namespace Application.Models.Authentication
{
    public class AuthResult
    {
        public JwtToken Token { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
