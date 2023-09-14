using GameStore.Domain.Commons;
using System.Linq.Expressions;

namespace GameStore.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : Auditable
    {
        ValueTask<TEntity> InsertAsync(TEntity entity);
        ValueTask<TEntity> UpdateAsync(TEntity entity);
        ValueTask DeleteAsync(TEntity entity);
        ValueTask<TEntity> SelectAsync(
            Expression<Func<TEntity, bool>> expression, string[] includes = null);
        IQueryable<TEntity> SelectAll(
            Expression<Func<TEntity, bool>> expression = null, string[] includes = null);
    }
}
