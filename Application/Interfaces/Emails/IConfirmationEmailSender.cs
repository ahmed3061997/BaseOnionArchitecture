using Application.Models.Users;

namespace Application.Interfaces.Emails
{
    public interface IConfirmationEmailSender
    {
        Task Send(IdentityTokenResult result, string resetUrl);
    }
}
