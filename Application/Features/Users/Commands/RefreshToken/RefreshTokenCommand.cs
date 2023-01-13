using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;
using Application.Models.Users;

namespace Application.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<IResultResponse<JwtToken>>
    {
        public string? Token { get; set; }
    }
}
