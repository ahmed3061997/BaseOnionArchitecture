using Application.Common.Constants;
using Application.Contracts.Notifications;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task Push(INotification notification)
        {
            if (notification.TargetUsers != null)
            {
                await hubContext.Clients.Group(Roles.Admin).SendAsync("Notify", notification);
                await hubContext.Clients.Users(notification.TargetUsers.ToList()).SendAsync("Notify", notification);
            }
            else
            {
                await hubContext.Clients.All.SendAsync("Notify", notification);
            }
        }
    }
}
