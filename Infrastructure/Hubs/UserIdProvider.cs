using Application.Common.Constants;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    internal class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst(Claims.UserId)!.Value;
        }
    }
}
