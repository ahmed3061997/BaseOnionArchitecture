using Domain.Repository;
using Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repository;
using Persistence.Repository.Base;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        #region DbContext

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(
                                configuration.GetConnectionString("DefaultConnection"),
                                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        #endregion

        #region Repositories

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();
            services.AddScoped<IPageOperationRepository, PageOperationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        #endregion
    }
}
