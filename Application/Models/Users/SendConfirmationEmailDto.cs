using FluentValidation;

namespace Application.Models.Users
{
    public class SendConfirmationEmailDto
    {
        public string ConfirmUrl { get; set; }
        public string Username { get; set; }
    }

    public class SendConfirmationEmailDtoValidator : AbstractValidator<SendConfirmationEmailDto>
    {
        public SendConfirmationEmailDtoValidator()
        {
            RuleFor(x => x.ConfirmUrl).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
