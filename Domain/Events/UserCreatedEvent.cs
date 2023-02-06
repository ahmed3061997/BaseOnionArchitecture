using Domain.Common;

namespace Domain.Events
{
    public class UserCreatedEvent : BaseEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public string ConfirmEmailUrl { get; set; }

        public UserCreatedEvent(string userId, string email, IEnumerable<string>? roles, string confirmEmailUrl)
        {
            UserId = userId;
            Email = email;
            Roles = roles;
            ConfirmEmailUrl = confirmEmailUrl;
        }
    }
}
