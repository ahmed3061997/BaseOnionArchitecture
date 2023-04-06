using Application.Common.Constants;
using Application.Interfaces.Notifications;
using Application.Interfaces.Persistence;
using Application.Models.Notifications;
using Domain.Entities.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.FindFirstValue(ClaimTypes.Role) == Roles.Admin)
                await Groups.AddToGroupAsync(Context.ConnectionId, Roles.Admin);
        }

        public async Task Push(BaseNotification notification)
        {
            if (notification.TargetUsers != null)
            {
                await Clients.Group(Roles.Admin).SendAsync("Notify", notification);
                await Clients.Users(notification.TargetUsers.ToList()).SendAsync("Notify", notification);
            }
            else
            {
                await Clients.All.SendAsync("Notify", notification);
            }
        }
    }
}
