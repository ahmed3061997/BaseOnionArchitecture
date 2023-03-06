using API.Common;
using Application.Common.Constants;
using Application.Interfaces.System;
using Application.Models.System;
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

        public SystemController(IClaimProvider claimProvider)
        {
            this.claimProvider = claimProvider;
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
    }
}
