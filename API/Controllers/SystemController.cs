using API.Common;
using Application.Common.Constants;
using Application.Interfaces.System;
using Application.Models.System;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/system")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IClaimProvider claimProvider;
        private readonly ApplicationDbContextInitializer initializer;

        public SystemController(IClaimProvider claimProvider, ApplicationDbContextInitializer initializer)
        {
            this.claimProvider = claimProvider;
            this.initializer = initializer;
        }

        [HttpGet(ApiRoutes.GetCultures)]
        public IEnumerable<CultureDto> GetCultures()
        {
            return Cultures.All;
        }

        [HttpGet(ApiRoutes.GetClaims)]
        public async Task<IEnumerable<PageClaimDto>> GetClaims()
        {
            return await claimProvider.GetPageClaims();
        }

        [HttpPost(ApiRoutes.MigrateData)]
        public async Task MigrateData()
        {
            await initializer.SeedAsync();
        }
    }
}
