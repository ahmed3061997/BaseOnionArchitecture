using Application.Models.Users;

namespace Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<IdentityTokenResult> GenerateResetPasswordToken(string username);
        Task<AuthResult> ResetPassword(string username, string token, string newPassword);
        Task<IdentityTokenResult> GenerateEmailConfirmationToken(string username);
        Task<AuthResult> ConfirmEmail(string username, string token);
        Task<AuthResult> Login(string username, string password);
        Task Logout();
    }
}
