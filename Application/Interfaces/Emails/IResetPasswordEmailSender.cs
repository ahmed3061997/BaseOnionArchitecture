using Application.Models.Users;

namespace Application.Interfaces.Emails
{
    public interface IResetPasswordEmailSender
    {
        Task Send(IdentityTokenResult result, string resetUrl);
    }
}
