using MediatR;

namespace Application.Common.EventHandlers
{
    public abstract class EventHandlerBase<T> : INotificationHandler<T> where T : INotification
    {
        public virtual Task Handle(T notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
