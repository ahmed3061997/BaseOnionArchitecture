namespace Application.Models.Users
{
    public class SendResetPasswordDto
    {
        public string ResetUrl { get; set; }
        public string Username { get; set; }
    }
}
