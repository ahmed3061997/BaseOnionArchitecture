using FluentValidation;

namespace Application.Models.Users
{
    public class ResetPassowrdDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

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
