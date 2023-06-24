using Application.Models.Common;

namespace Application.Contracts.Emails
{
    public interface IEmailService
    {
        Task Send(EmailMessage message);
    }
}
