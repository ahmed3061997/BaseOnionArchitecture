using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Entities.Users;
using Application.Common.Constants;
using Application.Common.Extensions;
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
            foreach (var roleName in Extensions.GetStaticMembers<string>(typeof(Roles)))
            {
                bool createRole = false;
                var role = await roleManager.Roles
                    .Include(x => x.Names)
                    .Include(x => x.Claims)
                    .FirstOrDefaultAsync(x => x.Name == roleName);
                if (role == null)
                {
                    createRole = true;
                    role = new ApplicationRole(roleName);
                }
                role.IsActive = true;
                role.Names = new List<ApplicationRoleName>() {
                    new ApplicationRoleName() { Culture = "en", Name = roleName },
                    new ApplicationRoleName() { Culture = "ar", Name = Roles.ArDic[roleName] },
                };
                role.Claims = claims.Select(x => new ApplicationRoleClaim() { ClaimType = x.Type, ClaimValue = x.Value }).ToList();

                if (createRole)
                    await roleManager.CreateAsync(role);
                else
                    await roleManager.UpdateAsync(role);

                var user = await userManager.FindByNameAsync(roleName);
                if (user == null)
                {
                    user = new ApplicationUser(roleName)
                    {
                        FirstName = roleName,
                        LastName = "User",
                        Email = $"{roleName.ToLower()}@email.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(user, "User123@@@");
                }

                if (!await userManager.IsInRoleAsync(user, roleName))
                    await userManager.AddToRoleAsync(user, roleName);

                await context.Set<ApplicationUserClaim>().Where(x => x.UserId == user.Id).ExecuteDeleteAsync();
                //await userManager.AddClaimsAsync(user, claims);
            }
        }
    }
}
