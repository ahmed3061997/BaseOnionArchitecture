using MediatR;
using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;

namespace Application.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, IResultResponse<JwtToken>>
    {
        private readonly ITokenService tokenService;

        public RefreshTokenCommandHandler(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<IResultResponse<JwtToken>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return new ResultResponse<JwtToken>()
            {
                Result = true,
                Value = await tokenService.RefreshToken(request.Token)
            };
        }
    }
}
