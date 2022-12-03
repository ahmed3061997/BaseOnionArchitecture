namespace Domain.Models
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<RefreshTokenDto> RefreshTokens { get; set; }
    }
}
