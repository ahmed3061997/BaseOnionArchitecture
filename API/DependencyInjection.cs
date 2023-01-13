using API.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            Configure(services);
            AddServices(services);
            AddSwagger(services);
            AddCors(services);
            AddLocalization(services);
            return services;
        }

        private static void Configure(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ExceptionHandlingMiddleware>();
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void AddCors(IServiceCollection services)
        {
            services.AddCors(op =>
            {
                op.AddDefaultPolicy(p => p.AllowAnyOrigin()
                                          .AllowAnyHeader()
                                          .AllowAnyMethod()
                                          .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
            });
        }

        private static void AddLocalization(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("en-Us");
                options.AddSupportedUICultures("en-US", "ar-EG");
                options.FallBackToParentUICultures = true;
            });

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
        }
    }
}
