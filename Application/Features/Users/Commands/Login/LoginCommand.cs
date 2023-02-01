using FluentValidation;
using Application.Common.Responses;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommand : IRequest<IResultResponse<AuthResult>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
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
