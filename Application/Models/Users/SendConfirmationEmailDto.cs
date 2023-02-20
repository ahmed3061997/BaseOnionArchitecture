using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
