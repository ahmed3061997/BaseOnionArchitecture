using Application.Models.Users;
using FluentValidation;

namespace Infrastructure.Validation
{
    public class SendResetPasswordDtoValidator : AbstractValidator<SendResetPasswordDto>
    {
        public SendResetPasswordDtoValidator()
        {
            RuleFor(x => x.ResetUrl).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
