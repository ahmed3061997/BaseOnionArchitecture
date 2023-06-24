namespace Application.Contracts.Notifications
{
    public interface INotificationService
    {
        Task Push(INotification notification);
    }
}
