using Domain.Repository.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence.Repository.Base
{
    internal sealed class DbTransaction : IDbTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public DbTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
