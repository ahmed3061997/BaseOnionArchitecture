using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Entities.Users;
using Application.Common.Constants;
using Application.Common.Extensions;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                    await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            // Default roles
            //foreach (var role in Extensions.GetStaticMembers<string>(typeof(Roles)))
            //    if (!_roleManager.Roles.Any(r => r.Name == role))
            //        await _roleManager.CreateAsync(new ApplicationUserRole(role));
        }
    }
}
