using Application.Models.Common;

namespace Application.Interfaces.Emails
{
    public interface IEmailSender
    {
        Task Send(EmailMessage message);
    }
}
