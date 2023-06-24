using Application.Models.Users;
using FluentValidation;

namespace Infrastructure.Validation
{
    public class ResetPassowrdDtoValidator : AbstractValidator<ResetPassowrdDto>
    {
        public ResetPassowrdDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
        }
    }
}
