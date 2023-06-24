namespace Application.Contracts.Notifications
{
    public interface INotification
    {
        object Subject { get; }
        object? Data { get; set; }
        IEnumerable<string>? TargetUsers { get; set; }
    }
}
