using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;
using Application.Models.Users;

namespace Application.Features.Users.Commands.EmailConfirmation
{
    public class ConfirmEmailCommand : IRequest<IResultResponse<AuthResult>>
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
