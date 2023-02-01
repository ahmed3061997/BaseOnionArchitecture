using Application.Common.Responses;
using Application.Interfaces.Users;
using MediatR;

namespace Application.Features.Users.Commands.Logout
{
    public class LogoutCommand : IRequest<IResponse>
    {

    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, IResponse>
    {
        private readonly IAuthService authService;

        public LogoutCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<IResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await authService.Logout();
            return new Response() { Result = true };
        }
    }
}
