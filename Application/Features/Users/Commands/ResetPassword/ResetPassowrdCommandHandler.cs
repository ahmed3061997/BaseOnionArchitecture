using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;

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
