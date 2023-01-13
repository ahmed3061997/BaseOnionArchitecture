using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
