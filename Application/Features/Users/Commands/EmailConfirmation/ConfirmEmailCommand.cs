using FluentValidation;
using Application.Common.Responses;
using Application.Models.Users;
using MediatR;

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
