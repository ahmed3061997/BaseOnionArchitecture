using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Configurations.Options;
using Application.Features.Culture;
using Application.Features.Emails;
using Application.Features.FileManager;
using Application.Features.System;
using Application.Features.Users;
using Application.Features.Validation;
using Application.Interfaces.Culture;
using Application.Interfaces.Emails;
using Application.Interfaces.FileManager;
using Application.Interfaces.System;
using Application.Interfaces.Users;
using Application.Interfaces.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Interfaces.Notifications;
using Application.Features.Notifications;
using Microsoft.AspNetCore.SignalR;
using Application.Hubs;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Options
            services.Configure<GetewayOptions>(configuration.GetSection("Gateway"));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<EmailOptions>(configuration.GetSection("EmailConfiguration"));

            // Automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Fluent Validation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddSignalR();

            AddJwt(services, configuration);
            AddServices(services);

            return services;
        }

        private static void AddJwt(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(op =>
                {
                    op.RequireHttpsMetadata = false;
                    op.SaveToken = false;
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                    op.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/api/hubs")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            services.AddSingleton<IValidationService, ValidationService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICurrentCultureService, CurrentCultureService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IResetPasswordEmailSender, ResetPasswordEmailSender>();
            services.AddScoped<IConfirmationEmailSender, ConfirmationEmailSender>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IClaimProvider, ClaimProvider>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
