using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;
using System.Web;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class ResetPassowrdCommandHandler : IRequestHandler<ResetPassowrdCommand, IResultResponse<AuthResult>>
    {
        private readonly IAuthService authService;

        public ResetPassowrdCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<IResultResponse<AuthResult>> Handle(ResetPassowrdCommand request, CancellationToken cancellationToken)
        {
            return new ResultResponse<AuthResult>()
            {
                Result = true,
                Value = await authService.ResetPassword(
                    request.Username,
                    HttpUtility.HtmlDecode(request.Token),
                    request.NewPassword)
            };
        }
    }
}
