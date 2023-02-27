using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Interfaces.Culture;
using Application.Interfaces.Persistence;
using Application.Interfaces.System;
using Application.Models.System;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Features.System
{
    public class ClaimProvider : IClaimProvider
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentCultureService currentCultureService;

        public ClaimProvider(IApplicationDbContext context, ICurrentCultureService currentCultureService)
        {
            this.context = context;
            this.currentCultureService = currentCultureService;
        }

        public async Task<IEnumerable<Claim>> GetClaims()
        {
            var query = await context.PageOperations
                .Select(x => new
                {
                    ModuleCode = x.Page.Module.Code,
                    PageCode = x.Page.Code,
                    OperationCode = x.Operation.Code,
                })
                .ToListAsync();
            return query.Select(x => new Claim(Claims.Permission, $"{x.ModuleCode.EnumToString()}.{x.PageCode.EnumToString()}.{x.OperationCode.EnumToString()}"));
        }

        public async Task<IEnumerable<PageClaimDto>> GetPageClaims()
        {
            var culture = currentCultureService.GetCurrentUICulture();
            var query = await context.PageOperations
                .Select(x => new
                {
                    ModuleCode = x.Page.Module.Code,
                    ModuleName = x.Page.Module.Names.Where(n => n.Culture == culture).Select(n => n.Name).FirstOrDefault(),
                    PageCode = x.Page.Code,
                    PageName = x.Page.Names.Where(n => n.Culture == culture).Select(n => n.Name).FirstOrDefault(),
                    OperationCode = x.Operation.Code,
                    OperationName = x.Operation.Names.Where(n => n.Culture == culture).Select(n => n.Name).FirstOrDefault(),
                })
                .ToListAsync();

            return query.Select(x => new PageClaimDto()
            {
                Value = $"{x.ModuleCode.EnumToString()}.{x.PageCode.EnumToString()}.{x.OperationCode.EnumToString()}",
                ModuleName = x.ModuleName,
                PageName = x.PageName,
                OperationName = x.OperationName,
            });
        }
    }
}
