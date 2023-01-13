﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;
using Application.Models.Users;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<IResultResponse<AuthResult>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ConfirmEmailUrl { get; set; }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(120);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Role).NotEmpty();
            RuleFor(x => x.ConfirmEmailUrl).NotEmpty();
        }
    }
}
