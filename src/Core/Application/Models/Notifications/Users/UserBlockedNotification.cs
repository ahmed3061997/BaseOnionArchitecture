namespace Application.Models.Notifications.Users
{
    public class UserBlockedNotification : BaseNotification
    {
        private const string SUBJECT = "USER_BLOCKED";

        public UserBlockedNotification(IEnumerable<string>? targetUsers)
            : base(targetUsers, SUBJECT, null)
        {
        }

        public UserBlockedNotification(IEnumerable<string>? targetUsers, object? data)
            : base(targetUsers, SUBJECT, data)
        {
        }
    }
}
