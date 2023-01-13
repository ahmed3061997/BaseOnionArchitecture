using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;
using Application.Features.Users.Commands.CreateUser;
using Application.Interfaces.Users;
using Application.Models.Users;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, IResultResponse<AuthResult>>
    {
        private readonly IAuthService authService;

        public LoginCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<IResultResponse<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return new ResultResponse<AuthResult>()
            {
                Result = true,
                Value = await authService.Login(request.Username, request.Password)
            };
        }
    }
}
