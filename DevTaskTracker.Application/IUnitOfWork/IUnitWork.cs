namespace DevTaskTracker.Application.IUnitOfWork
{
    public interface IUnitWork
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();

    }
}
