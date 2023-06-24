using Application.Models.Users;
using FluentValidation;

namespace Infrastructure.Validation
{
    public class SendConfirmationEmailDtoValidator : AbstractValidator<SendConfirmationEmailDto>
    {
        public SendConfirmationEmailDtoValidator()
        {
            RuleFor(x => x.ConfirmUrl).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
