using FluentValidation;
using Application.Common.Responses;
using MediatR;

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
