using GameStore.Data.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameStore.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _disposed;
        private IDbContextTransaction _objTran;
        public AppDbContext dbContext { get; }

        public UnitOfWork(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }

        public void Dispose()
        {
            DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask CreateTransactionAsync()
        {
            _objTran = await dbContext.Database.BeginTransactionAsync();
        }

        public async ValueTask CommitAsync()
        {
            await _objTran.CommitAsync();
        }

        public async ValueTask RollbackAsync()
        {
            await _objTran.RollbackAsync();
            await _objTran.DisposeAsync();
        }

        public async ValueTask SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        protected virtual void DisposeAsync(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    dbContext.Dispose();
            _disposed = true;
        }
    }
}
