using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, IResultResponse<AuthResult>>
    {
        private readonly IAuthService authService;

        public LoginCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<IResultResponse<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return new ResultResponse<AuthResult>()
            {
                Result = true,
                Value = await authService.Login(request.Username, request.Password)
            };
        }
    }
}
