using Application.Interfaces.Emails;
using Application.Models.Common;
using Application.Models.Users;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Features.Emails
{
    public class ConfirmationEmailSender : IConfirmationEmailSender
    {
        private readonly IEmailService emailService;
        private readonly IHostEnvironment environment;

        public ConfirmationEmailSender(IEmailService emailService, IHostEnvironment environment)
        {
            this.emailService = emailService;
            this.environment = environment;
        }

        public async Task Send(IdentityTokenResult result, string confirmUrl)
        {
            await emailService.Send(CreateMessage(result, confirmUrl));
        }

        private EmailMessage CreateMessage(IdentityTokenResult result, string confirmUrl)
        {
            var url = confirmUrl
                .Replace("{Email}", result.Email)
                .Replace("{Token}", HttpUtility.UrlEncode(result.Token));

            var filePath = $"{environment.ContentRootPath}/Templates/Emails/EmailConfirmation.html";
            var content = File.ReadAllText(filePath)
                .Replace("{{Name}}", result.Name)
                .Replace("{{VerifyUrl}}", url);

            return new EmailMessage(
                new Dictionary<string, string>() { { result.Name, result.Email } },
                "Email Verfication",
                content);
        }
    }
}
