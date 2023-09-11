using GameStore.Domain.Commons;
using System.Linq.Expressions;

namespace GameStore.Data.IRepositories
{
    public interface IRepository<TEntity> where TEntity : Auditable
    {
        ValueTask<TEntity> InsertAsync(TEntity entity);
        ValueTask<TEntity> UpdateAsync(TEntity entity);
        ValueTask<bool> DeleteAsync(TEntity entity);
        ValueTask<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> expression);
    }
}
