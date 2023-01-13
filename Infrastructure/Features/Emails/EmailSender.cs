using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Application.Interfaces.Emails;
using Application.Models.Common;
using Application.Common.Configurations.Options;

namespace Infrastructure.Features.Emails
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailOptions> emailConfig;

        public EmailSender(IOptions<EmailOptions> emailConfig)
        {
            this.emailConfig = emailConfig;
        }

        public async Task Send(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", emailConfig.Value.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = message.IsHtml ? new BodyBuilder() { HtmlBody = message.Content }.ToMessageBody() : new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(emailConfig.Value.Server, emailConfig.Value.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(emailConfig.Value.Username, emailConfig.Value.Password);
                await client.SendAsync(mailMessage);
            }
        }
    }
}
