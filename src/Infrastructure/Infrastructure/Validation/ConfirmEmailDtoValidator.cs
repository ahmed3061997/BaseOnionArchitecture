using Application.Models.Users;
using FluentValidation;

namespace Infrastructure.Validation
{
    public class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
