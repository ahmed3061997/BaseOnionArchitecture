using Microsoft.AspNetCore.Mvc;
using API.Common;
using Microsoft.AspNetCore.Authorization;
using Application.Models.Users;
using Application.Interfaces.Users;
using Application.Interfaces.Validation;
using Application.Interfaces.Emails;
using System.Web;
using AutoMapper;
using Infrastructure.Features.Users;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IValidationService validationService;

        public AuthController(
            IAuthService authService,
            IUserService userService,
            ITokenService tokenService,
            IMapper mapper,
            IValidationService validationService)
        {
            this.authService = authService;
            this.userService = userService;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.validationService = validationService;
        }

        [HttpPost(ApiRoutes.Register)]
        public async Task<AuthResult> Register(CreateUserDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            return await userService.Create(mapper.Map<UserDto>(dto), dto.Password);
        }

        [HttpPost(ApiRoutes.Login)]
        public async Task<AuthResult> Login(LoginDto login)
        {
            await validationService.ThrowIfInvalid(login);
            return await authService.Login(login.Username, login.Password);
        }

        [HttpPost(ApiRoutes.SendResetPassword)]
        public async Task<bool> SendResetPassword(SendResetPasswordDto dto, [FromServices] IResetPasswordEmailSender emailSender)
        {
            await validationService.ThrowIfInvalid(dto);
            var result = await authService.GenerateResetPasswordToken(dto.Username);
            emailSender.Send(result, dto.ResetUrl);
            return true;
        }

        [HttpPost(ApiRoutes.ResetPassword)]
        public async Task<AuthResult> ResetPassword(ResetPassowrdDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            return await authService.ResetPassword(
                      dto.Username,
                      HttpUtility.HtmlDecode(dto.Token),
                      dto.NewPassword);
        }

        [HttpPost(ApiRoutes.SendEmailConfirmation)]
        public async Task<bool> SendEmailConfirmation(SendConfirmationEmailDto dto, [FromServices] IConfirmationEmailSender emailSender)
        {
            await validationService.ThrowIfInvalid(dto);
            var result = await authService.GenerateResetPasswordToken(dto.Username);
            emailSender.Send(result, dto.ConfirmUrl);
            return true;
        }

        [HttpPost(ApiRoutes.ConfirmEmail)]
        public async Task<AuthResult> ConfirmEmail(ConfirmEmailDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            return await authService.ConfirmEmail(dto.Username, HttpUtility.HtmlDecode(dto.Token));
        }

        [Authorize]
        [HttpGet(ApiRoutes.Logout)]
        public async Task<bool> Logout()
        {
            await authService.Logout();
            return true;
        }

        [HttpPost(ApiRoutes.RefreshToken)]
        public async Task<JwtToken> RefreshToken(JwtToken token)
        {
            return await tokenService.RefreshToken(token.RefreshToken);
        }
    }
}
