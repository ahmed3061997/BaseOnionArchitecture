using Application.Models.Common;

namespace Application.Interfaces.Emails
{
    public interface IEmailService
    {
        Task Send(EmailMessage message);
    }
}
