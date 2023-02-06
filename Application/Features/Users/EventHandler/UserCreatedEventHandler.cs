using Application.Common.EventHandlers;
using Application.Features.Users.Commands.AssignToRole;
using Application.Features.Users.Commands.EmailConfirmation;
using Domain.Events;
using MediatR;

namespace Application.Features.Users.EventHandler
{
    public class UserCreatedEventHandler : EventHandlerBase<UserCreatedEvent>
    {
        private readonly IMediator mediator;

        public UserCreatedEventHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public override async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Roles != null)
                await AssignToRole(notification);
            await SendConfirmationEmail(notification);
        }

        private async Task AssignToRole(UserCreatedEvent notification)
        {
            await mediator.Send(new AssignToRoleCommand()
            {
                UserId = notification.UserId,
                Roles = notification.Roles,
            });
        }

        private async Task SendConfirmationEmail(UserCreatedEvent notification)
        {
            await mediator.Send(new SendEmailConfirmationCommand()
            {
                Username = notification.Email,
                ConfirmUrl = notification.ConfirmEmailUrl
            });
        }
    }
}
