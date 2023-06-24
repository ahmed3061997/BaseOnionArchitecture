namespace Application.Models.Notifications.Users
{
    public class UserUpdatedNotification : BaseNotification
    {
        private const string SUBJECT = "USER_UPDATED";

        public UserUpdatedNotification(IEnumerable<string>? targetUsers)
            : base(targetUsers, SUBJECT, null)
        {
        }

        public UserUpdatedNotification(IEnumerable<string>? targetUsers, object? data)
            : base(targetUsers, SUBJECT, data)
        {
        }
    }
}
