using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;
using System.Web;

namespace Application.Features.Users.Commands.EmailConfirmation
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IResultResponse<AuthResult>>
    {
        private readonly IAuthService authService;

        public ConfirmEmailCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<IResultResponse<AuthResult>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return new ResultResponse<AuthResult>()
            {
                Result = true,
                Value = await authService.ConfirmEmail(request.Username, HttpUtility.HtmlDecode(request.Token))
            };
        }
    }
}
