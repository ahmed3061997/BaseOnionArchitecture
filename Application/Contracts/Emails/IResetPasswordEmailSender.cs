using Application.Models.Users;

namespace Application.Contracts.Emails
{
    public interface IResetPasswordEmailSender
    {
        Task Send(IdentityTokenResult result, string resetUrl);
    }
}
