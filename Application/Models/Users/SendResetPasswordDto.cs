using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Users
{
    public class SendResetPasswordDto
    {
        public string ResetUrl { get; set; }
        public string Username { get; set; }
    }

    public class SendResetPasswordDtoValidator : AbstractValidator<SendResetPasswordDto>
    {
        public SendResetPasswordDtoValidator()
        {
            RuleFor(x => x.ResetUrl).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
