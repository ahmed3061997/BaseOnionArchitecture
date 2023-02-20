using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Configurations.Options;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Options
            services.Configure<GetewayOptions>(configuration.GetSection("Gateway"));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<EmailOptions>(configuration.GetSection("EmailConfiguration"));

            // Automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Fluent Validation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
