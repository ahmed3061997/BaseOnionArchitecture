using Application.Models.System;
using System.Security.Claims;

namespace Application.Interfaces.System
{
    public interface IClaimProvider
    {
        Task<IEnumerable<Claim>> GetClaims();
        Task<IEnumerable<PageClaimDto>> GetPageClaims();
    }
}
