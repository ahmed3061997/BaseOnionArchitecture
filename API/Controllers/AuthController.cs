using MediatR;
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
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost(ApiRoutes.Login)]
        public async Task<IResponse> Login(LoginCommand request)
        {
            return await mediator.Send(request);
        }

        [HttpPost(ApiRoutes.Register)]
        public async Task<IResponse> Register(CreateUserCommand request)
        {
            return await mediator.Send(request);
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

        [Authorize]
        [HttpGet(ApiRoutes.Logout)]
        public async Task<IResponse> Logout()
        {
            return await mediator.Send(new LogoutCommand());
        }

        [HttpPost(ApiRoutes.RefreshToken)]
        public async Task<IResponse> RefreshToken(string token)
        {
            return await mediator.Send(new RefreshTokenCommand() { Token = token });
        }
    }
}
