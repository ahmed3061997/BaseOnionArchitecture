using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;
using Application.Features.Users.Commands.CreateUser;
using Application.Models.Users;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommand : IRequest<IResultResponse<AuthResult>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
