namespace Application.Models.Notifications.Roles
{
    public class RoleUpdatedNotification : BaseNotification
    {
        private const string SUBJECT = "ROLE_UPDATED";

        public RoleUpdatedNotification(IEnumerable<string>? targetUsers)
            : base(targetUsers, SUBJECT, null)
        {
        }

        public RoleUpdatedNotification(IEnumerable<string>? targetUsers, object? data)
            : base(targetUsers, SUBJECT, data)
        {
        }
    }
}
