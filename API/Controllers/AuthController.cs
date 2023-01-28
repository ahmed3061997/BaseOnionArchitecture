using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Responses;
using Application.Common.Constants;
using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.RefreshToken;
using API.Common;
using Application.Features.Users.Commands.Logout;
using Application.Features.Users.Commands.ResetPassword;
using Application.Features.Users.Commands.EmailConfirmation;
using Application.Features.Users.Commands.Create;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(ApiRoutes.Authenticate), Authorize]
        public bool Authenticate() => true;

        [HttpPost(ApiRoutes.Login)]
        public async Task<IResponse> Login(LoginCommand request)
        {
            var res = await mediator.Send(request);
            SetRefreshTokenCookie(res.Value.Jwt.RefreshToken, res.Value.Jwt.RefreshTokenExpiresOn);
            return res;
        }

        [HttpPost(ApiRoutes.SendResetPassword)]
        public async Task<IResponse> SendResetPassword(SendResetPasswordCommand request)
        {
            return await mediator.Send(request);
        }

        [HttpPost(ApiRoutes.ResetPassword)]
        public async Task<IResponse> ResetPassword(ResetPassowrdCommand request)
        {
            return await mediator.Send(request);
        }

        [HttpPost(ApiRoutes.ConfirmEmail)]
        public async Task<IResponse> ConfirmEmail(ConfirmEmailCommand request)
        {
            return await mediator.Send(request);
        }

        [HttpPost(ApiRoutes.SendEmailConfirmation)]
        public async Task<IResponse> SendEmailConfirmation(SendEmailConfirmationCommand request)
        {
            return await mediator.Send(request);
        }

        [HttpGet(ApiRoutes.Logout)]
        public async Task<IResponse> Logout()
        {
            return await mediator.Send(new LogoutCommand());
        }

        [HttpPost(ApiRoutes.Register)]
        public async Task<IResponse> Register(CreateUserCommand request)
        {
            var res = await mediator.Send(request);
            SetRefreshTokenCookie(res.Value.Jwt.RefreshToken, res.Value.Jwt.RefreshTokenExpiresOn);
            return res;
        }

        [HttpPost(ApiRoutes.RefreshToken)]
        public async Task<IResponse> RefreshToken([FromForm] string? token)
        {
            var refreshToken = token ?? Request.Cookies[Cookies.RefreshToken];
            var res = await mediator.Send(new RefreshTokenCommand() { Token = refreshToken });
            SetRefreshTokenCookie(res.Value.RefreshToken, res.Value.RefreshTokenExpiresOn);
            return res;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private void SetRefreshTokenCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                Expires = expires,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append(Cookies.RefreshToken, refreshToken, cookieOptions);
        }
    }
}
