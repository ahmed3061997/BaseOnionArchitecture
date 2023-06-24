using Application.Interfaces.System;
using Application.Services.System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IClaimProvider, ClaimProvider>();

            return services;
        }
    }
}
