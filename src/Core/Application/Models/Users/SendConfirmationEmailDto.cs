namespace Application.Models.Users
{
    public class SendConfirmationEmailDto
    {
        public string ConfirmUrl { get; set; }
        public string Username { get; set; }
    }
}
