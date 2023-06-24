namespace Domain.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        public IModuleRepository Modules { get; }
        public IPageRepository Pages { get; }
        public IOperationRepository Operations { get; }
        public IPageOperationRepository PageOperations { get; }
        Task<IDbTransaction> BeginTransaction();
        Task CompleteAsync();
    }
}
