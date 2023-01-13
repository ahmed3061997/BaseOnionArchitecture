using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;
using Application.Features.Users.Commands.CreateUser;

namespace Application.Features.Users.Commands.EmailConfirmation
{
    public class SendEmailConfirmationCommand : IRequest<IResponse>
    {
        public string ConfirmUrl { get; set; }
        public string Username { get; set; }
    }

    public class SendEmailConfirmationCommandValidator : AbstractValidator<SendEmailConfirmationCommand>
    {
        public SendEmailConfirmationCommandValidator()
        {
            RuleFor(x => x.ConfirmUrl).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
