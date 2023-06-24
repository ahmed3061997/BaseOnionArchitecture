using Application.Contracts.Notifications;

namespace Application.Models.Notifications
{
    public class BaseNotification : INotification
    {
        public BaseNotification(IEnumerable<string>? targetUsers, object subject, object? data)
        {
            TargetUsers = targetUsers;
            Subject = subject;
            Data = data;
        }

        public IEnumerable<string>? TargetUsers { get; set; }
        public object Subject { get; private set; }
        public object Data { get; set; }
    }
}
