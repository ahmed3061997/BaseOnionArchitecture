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
