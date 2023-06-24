namespace Domain.Repository.Base
{
    public interface IDbTransaction
    {
        Task RollBackAsync();
        Task CommitAsync();
    }
}
