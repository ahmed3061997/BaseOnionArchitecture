using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;
using Application.Features.Users.Commands.EmailConfirmation;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class SendResetPasswordCommand : IRequest<IResponse>
    {
        public string ResetUrl { get; set; }
        public string Username { get; set; }
    }

    public class SendResetPasswordCommandValidator : AbstractValidator<SendResetPasswordCommand>
    {
        public SendResetPasswordCommandValidator()
        {
            RuleFor(x => x.ResetUrl).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
