using MediatR;
using Microsoft.AspNetCore.Hosting;
using System.Web;
using Application.Common.Responses;
using Application.Interfaces.Emails;
using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Users;

namespace Application.Features.Users.Commands.EmailConfirmation
{
    public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand, IResponse>
    {
        private readonly IAuthService authService;
        private readonly IEmailSender emailSender;
        private readonly IHostingEnvironment environment;

        public SendEmailConfirmationCommandHandler(
            IAuthService authService,
            IEmailSender emailSender,
            IHostingEnvironment environment)
        {
            this.authService = authService;
            this.emailSender = emailSender;
            this.environment = environment;
        }

        public async Task<IResponse> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            var result = await authService.GenerateEmailConfirmationToken(request.Username);
            emailSender.Send(CreateMessage(request.ConfirmUrl, result));
            return new Response() { Result = true };
        }

        private EmailMessage CreateMessage(string resetUrl, IdentityTokenResult result)
        {
            var url = resetUrl
                .Replace("{Email}", result.Email)
                .Replace("{Token}", HttpUtility.UrlEncode(result.Token));

            var filePath = $"{environment.WebRootPath}/Templates/Emails/EmailConfirmation.html";
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
