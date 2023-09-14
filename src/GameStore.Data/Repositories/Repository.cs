using GameStore.Data.DbContexts;
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
            entity.IsDeleted = true;
        }

        public async ValueTask<TEntity> InsertAsync(TEntity entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public IQueryable<TEntity> SelectAll(
            Expression<Func<TEntity, bool>> expression = null, string[] includes = null)
        {
            IQueryable<TEntity> query = expression is null ? _dbSet : _dbSet.Where(expression);

            if (includes is not null)
                foreach (string include in includes)
                    query = query.Include(include);

            return query;
        }

        public async ValueTask<TEntity> SelectAsync(
            Expression<Func<TEntity, bool>> expression, string[] includes = null)
        {
            var query = _dbSet.Where(expression);

            if (includes is not null)
                foreach (string include in includes)
                    query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }

        public async ValueTask<TEntity> UpdateAsync(TEntity entity)
        {
            var result = _dbSet.Update(entity);
            return result.Entity;
        }
    }
}
