using FluentValidation;
using Application.Common.Responses;
using MediatR;

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
