using API.Common;
using Application.Contracts.Emails;
using Application.Contracts.Identity;
using Application.Contracts.Validation;
using Application.Models.Users;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IValidationService _validationService;

        public AuthController(
            IAuthService authService,
            ITokenService tokenService,
            IValidationService validationService)
        {
            _authService = authService;
            _tokenService = tokenService;
            _validationService = validationService;
        }

        [HttpPost(ApiRoutes.Login)]
        public async Task<AuthResult> Login(LoginDto login)
        {
            await _validationService.ThrowIfInvalid(login);
            return await _authService.Login(login.Username, login.Password);
        }

        [HttpPost(ApiRoutes.SendResetPassword)]
        public async Task<bool> SendResetPassword(SendResetPasswordDto dto, [FromServices] IResetPasswordEmailSender emailSender)
        {
            await _validationService.ThrowIfInvalid(dto);
            var result = await _authService.GenerateResetPasswordToken(dto.Username);
            await emailSender.Send(result, dto.ResetUrl);
            return true;
        }

        [HttpPost(ApiRoutes.ResetPassword)]
        public async Task<AuthResult> ResetPassword(ResetPassowrdDto dto)
        {
            await _validationService.ThrowIfInvalid(dto);
            return await _authService.ResetPassword(
                      dto.Username,
                      HttpUtility.HtmlDecode(dto.Token),
                      dto.NewPassword);
        }

        [HttpPost(ApiRoutes.SendEmailConfirmation)]
        public async Task<bool> SendEmailConfirmation(SendConfirmationEmailDto dto, [FromServices] IConfirmationEmailSender emailSender)
        {
            await _validationService.ThrowIfInvalid(dto);
            var result = await _authService.GenerateResetPasswordToken(dto.Username);
            await emailSender.Send(result, dto.ConfirmUrl);
            return true;
        }

        [HttpPost(ApiRoutes.ConfirmEmail)]
        public async Task<AuthResult> ConfirmEmail(ConfirmEmailDto dto)
        {
            await _validationService.ThrowIfInvalid(dto);
            return await _authService.ConfirmEmail(dto.Username, HttpUtility.HtmlDecode(dto.Token));
        }

        [Authorize]
        [HttpGet(ApiRoutes.Logout)]
        public async Task<bool> Logout()
        {
            await _authService.Logout();
            return true;
        }

        [HttpPost(ApiRoutes.RefreshToken)]
        public async Task<JwtToken> RefreshToken(JwtToken token)
        {
            return await _tokenService.RefreshToken(token.RefreshToken);
        }
    }
}
