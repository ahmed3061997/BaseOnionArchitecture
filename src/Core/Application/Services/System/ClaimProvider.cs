using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Contracts.Culture;
using Application.Interfaces.System;
using Application.Models.System;
using Domain.Repository;
using System.Security.Claims;

namespace Application.Services.System
{
    public class ClaimProvider : IClaimProvider
    {
        private readonly IPageOperationRepository _pageOperationRepository;
        private readonly ICurrentCultureService _currentCultureService;

        public ClaimProvider(IPageOperationRepository pageOperationRepository, ICurrentCultureService currentCultureService)
        {
            _pageOperationRepository = pageOperationRepository;
            _currentCultureService = currentCultureService;
        }

        public async Task<IEnumerable<Claim>> GetClaims()
        {
            var query = await _pageOperationRepository
                .SelectAllAsync(x => new
                {
                    ModuleCode = x.Page.Module.Code,
                    PageCode = x.Page.Code,
                    OperationCode = x.Operation.Code,
                });
            return query.Select(x => new Claim(Claims.Permission, $"{x.ModuleCode.EnumToString()}.{x.PageCode.EnumToString()}.{x.OperationCode.EnumToString()}"));
        }

        public async Task<IEnumerable<PageClaimDto>> GetPageClaims()
        {
            var culture = _currentCultureService.GetCurrentUICulture();
            var query = await _pageOperationRepository
                .SelectAllAsync(x => new
                {
                    ModuleCode = x.Page.Module.Code,
                    ModuleName = x.Page.Module.Names.Where(n => n.Culture == culture).Select(n => n.Name).FirstOrDefault(),
                    PageCode = x.Page.Code,
                    PageName = x.Page.Names.Where(n => n.Culture == culture).Select(n => n.Name).FirstOrDefault(),
                    OperationCode = x.Operation.Code,
                    OperationName = x.Operation.Names.Where(n => n.Culture == culture).Select(n => n.Name).FirstOrDefault(),
                });

            return query.Select(x => new PageClaimDto()
            {
                Value = $"{x.ModuleCode.EnumToString()}.{x.PageCode.EnumToString()}.{x.OperationCode.EnumToString()}",
                ModuleName = x.ModuleName!,
                PageName = x.PageName!,
                OperationName = x.OperationName!,
            });
        }
    }
}
