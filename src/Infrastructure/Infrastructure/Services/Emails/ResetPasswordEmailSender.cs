using Application.Contracts.Emails;
using Application.Models.Common;
using Application.Models.Users;
using Microsoft.Extensions.Hosting;
using System.Web;

namespace Infrastructure.Services.Emails
{
    public class ResetPasswordEmailSender : IResetPasswordEmailSender
    {
        private readonly IEmailService emailService;
        private readonly IHostEnvironment environment;

        public ResetPasswordEmailSender(IEmailService emailService, IHostEnvironment environment)
        {
            this.emailService = emailService;
            this.environment = environment;
        }

        public async Task Send(IdentityTokenResult result, string resetUrl)
        {
            await emailService.Send(CreateMessage(result, resetUrl));
        }

        private EmailMessage CreateMessage(IdentityTokenResult result, string resetUrl)
        {
            var url = resetUrl
            .Replace("{Email}", result.Email)
                .Replace("{Token}", HttpUtility.UrlEncode(result.Token));

            var filePath = $"{environment.ContentRootPath}/Templates/Emails/ResetPassword.html";
            var content = File.ReadAllText(filePath)
                .Replace("{{Name}}", result.Name)
                .Replace("{{ResetUrl}}", url);

            return new EmailMessage(
                new Dictionary<string, string>() { { result.Name, result.Email } },
                "Reset Password",
                content);
        }
    }
}
