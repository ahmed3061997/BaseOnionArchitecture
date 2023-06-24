using API.Common;
using API.Middlewares;
using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            Configure(services, configuration);
            AddValidation(services);
            AddSignalR(services);
            AddLocalization(services);
            AddAuthorization(services);
            AddControllers(services);
            AddServices(services);
            AddCORS(services);
            return services;
        }

        #region Configuration

        private static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GetewayOptions>(configuration.GetSection("Gateway"));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<EmailOptions>(configuration.GetSection("EmailConfiguration"));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("en");
                options.AddSupportedUICultures(Extensions.GetStaticMembers<string>(typeof(Cultures)).ToArray());
                options.FallBackToParentUICultures = true;
            });
        }


        #endregion
        #region Validation

        private static void AddValidation(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }


        #endregion
        #region SignalR

        private static void AddSignalR(IServiceCollection services)
        {
            services.AddSignalR();
        }


        #endregion
        #region Localization

        private static void AddLocalization(IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Localization/Resources";
            });
        }


        #endregion
        #region Authorization


        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        }

        #endregion
        #region Controllers


        private static void AddControllers(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void AddCORS(IServiceCollection services)
        {
            services.AddCors(op =>
            {
                op.AddDefaultPolicy(p => p.AllowAnyOrigin()
                                          .AllowAnyHeader()
                                          .AllowAnyMethod()
                                          .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
            });
        }

        #endregion
        #region Services

        private static void AddServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ExceptionHandlingMiddleware>();
        }

        #endregion
    }
}
