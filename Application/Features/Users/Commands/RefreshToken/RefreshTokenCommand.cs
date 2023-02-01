using Application.Common.Responses;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<IResultResponse<JwtToken>>
    {
        public string? Token { get; set; }
    }
}
