using GameStore.Data.DbContexts;

namespace GameStore.Data.UnitOfWorks
{
    public interface IUnitOfWork
    {
        AppDbContext dbContext { get; }
        ValueTask CreateTransactionAsync();
        ValueTask CommitAsync();
        ValueTask RollbackAsync();
        ValueTask SaveAsync();
    }
}
