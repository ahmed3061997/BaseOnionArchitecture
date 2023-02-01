using FluentValidation;
using Application.Common.Responses;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class ResetPassowrdCommand : IRequest<IResultResponse<AuthResult>>
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPassowrdCommandValidator : AbstractValidator<ResetPassowrdCommand>
    {
        public ResetPassowrdCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
        }
    }
}
