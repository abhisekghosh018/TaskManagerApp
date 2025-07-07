using DevTaskTracker.Application.IUnitOfWork;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevTaskTracker.Infrastructure.UnitOfWork
{
    public class UnitWork : IUnitWork
    {

        private readonly AppDbContext _dbContext;
        private IDbContextTransaction _CurrentTransaction;

        public UnitWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransaction()
        {
            _CurrentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            if (_CurrentTransaction != null) {

               await _CurrentTransaction.CommitAsync();
               await _CurrentTransaction.DisposeAsync();
            }
        }

        public async Task RollbackTransaction()
        {
           await _CurrentTransaction.RollbackAsync();
           await _CurrentTransaction.DisposeAsync();
        }
    }
}
