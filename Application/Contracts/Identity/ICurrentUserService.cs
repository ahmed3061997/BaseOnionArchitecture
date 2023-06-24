namespace Application.Contracts.Identity
{
    public interface ICurrentUserService
    {
        string? GetCurrentUserId();
    }
}
