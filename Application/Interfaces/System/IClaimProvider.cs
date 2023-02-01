using Application.Models.System;

namespace Application.Interfaces.System
{
    public interface IClaimProvider
    {
        Task<IEnumerable<PageClaimDto>> GetClaims();
    }
}
