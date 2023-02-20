using FluentValidation;

namespace Application.Models.Users
{
    public class ConfirmEmailDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
