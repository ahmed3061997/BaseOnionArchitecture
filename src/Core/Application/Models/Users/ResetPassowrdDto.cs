namespace Application.Models.Users
{
    public class ResetPassowrdDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
