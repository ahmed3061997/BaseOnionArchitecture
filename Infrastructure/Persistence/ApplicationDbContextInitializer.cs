using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Entities.Users;
using Application.Common.Constants;
using Application.Common.Extensions;
using Infrastructure.Features.System;
using Application.Interfaces.System;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> logger;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IClaimProvider claimProvider;

        public ApplicationDbContextInitializer(
            ILogger<ApplicationDbContextInitializer> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IClaimProvider claimProvider)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.claimProvider = claimProvider;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (context.Database.IsSqlServer())
                    await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initialising the database.");
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
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            var claims = await claimProvider.GetClaims();

            // Default Roles & Users + Claims
            foreach (var role in Extensions.GetStaticMembers<string>(typeof(Roles)))
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new ApplicationRole(role));

                var user = await userManager.FindByNameAsync(role);
                if (user == null)
                {
                    user = new ApplicationUser(role)
                    {
                        FirstName = role,
                        LastName = "User",
                        Email = $"{role.ToLower()}@email.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(user, "User123@@@");
                }

                if (!await userManager.IsInRoleAsync(user, role))
                    await userManager.AddToRoleAsync(user, role);

                await context.Set<ApplicationUserClaim>().Where(x => x.UserId == user.Id).ExecuteDeleteAsync();
                await userManager.AddClaimsAsync(user, claims);
            }
        }
    }
}
