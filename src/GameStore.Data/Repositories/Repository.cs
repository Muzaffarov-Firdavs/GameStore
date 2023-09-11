using GameStore.Data.DbContexts;
using GameStore.Data.IRepositories;
using GameStore.Domain.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

#pragma warning disable

namespace GameStore.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async ValueTask DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async ValueTask<TEntity> InsertAsync(TEntity entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> expression = null)
        {
            if(expression != null)
                return _dbSet.Where(expression);

            return _dbSet;
        }

        public async ValueTask<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async ValueTask<TEntity> UpdateAsync(TEntity entity)
        {
            var result = _dbSet.Update(entity);
            return result.Entity;
        }
    }
}
