using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Web;
using Application.Common.Responses;
using Application.Interfaces.Emails;
using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Users;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class SendResetPasswordCommandHandler : IRequestHandler<SendResetPasswordCommand, IResponse>
    {
        private readonly IAuthService authService;
        private readonly IEmailSender emailSender;
        private readonly IHostingEnvironment environment;

        public SendResetPasswordCommandHandler(
            IAuthService authService,
            IEmailSender emailSender,
            IHostingEnvironment environment)
        {
            this.authService = authService;
            this.emailSender = emailSender;
            this.environment = environment;
        }

        public async Task<IResponse> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await authService.GenerateResetPasswordToken(request.Username);
            emailSender.Send(CreateMessage(request.ResetUrl, result));
            return new Response() { Result = true };
        }

        private EmailMessage CreateMessage(string resetUrl, IdentityTokenResult result)
        {
            var url = resetUrl
                .Replace("{Email}", result.Email)
                .Replace("{Token}", HttpUtility.UrlEncode(result.Token));

            var filePath = $"{environment.WebRootPath}/Templates/Emails/ResetPassword.html";
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
