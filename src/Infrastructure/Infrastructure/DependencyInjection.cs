using Application.Contracts.Culture;
using Application.Contracts.Emails;
using Application.Contracts.Identity;
using Application.Contracts.Notifications;
using Application.Contracts.Validation;
using Infrastructure.Hubs;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Services.Culture;
using Infrastructure.Services.Emails;
using Infrastructure.Services.FileManager;
using Infrastructure.Services.Notifications;
using Infrastructure.Services.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Persistence.Identity;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            AddIdentity(services);
            AddJwt(services, configuration);
            AddServices(services);

            return services;
        }

        #region Identity

        private static void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        #endregion
        #region Jwt

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

        #endregion
        #region Services

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
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<INotificationService, NotificationService>();
        }


        #endregion
    }
}
