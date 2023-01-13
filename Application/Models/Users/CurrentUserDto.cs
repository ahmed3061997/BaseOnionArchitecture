namespace Application.Models.Users
{
    public class CurrentUserDto : UserDto
    {
        public IEnumerable<string> Roles { get; set; }
    }
}
