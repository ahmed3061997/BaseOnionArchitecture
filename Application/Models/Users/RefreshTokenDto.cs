namespace Application.Models.Users
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
