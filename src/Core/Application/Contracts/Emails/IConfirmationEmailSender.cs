using Application.Models.Users;

namespace Application.Contracts.Emails
{
    public interface IConfirmationEmailSender
    {
        Task Send(IdentityTokenResult result, string resetUrl);
    }
}
