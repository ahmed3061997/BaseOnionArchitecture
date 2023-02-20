using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Users
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public string ConfirmEmailUrl { get; set; }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(120);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmEmailUrl).NotEmpty();
        }
    }
}
