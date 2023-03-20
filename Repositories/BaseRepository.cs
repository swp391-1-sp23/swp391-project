using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using SWP391.Project.DbContexts;
using SWP391.Project.Entities;

namespace SWP391.Project.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid entityId);
        Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool ignoreDeleted = false);
        Task<ICollection<TEntity>> GetCollectionAsync(Func<TEntity, bool>? predicate = null);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddCollectionAsync(ICollection<TEntity> collection);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> UpdateCollectionAsync(ICollection<TEntity> collection);
        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> RemoveCollectionAsync(ICollection<TEntity> collection);
        Task<bool> SaveChangesAsync();
    }

    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected ProjectDbContext DbContext { get; }
        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        public BaseRepository(ProjectDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            try
            {
                _ = await DbSet.AddAsync(entity);
                _ = await SaveChangesAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<TEntity?> GetByIdAsync(Guid entityId)
        {
            return await DbSet.FindAsync(keyValues: entityId);
        }

        public async Task<ICollection<TEntity>> GetCollectionAsync(Func<TEntity, bool>? predicate = null)
        {
            return predicate == null
                ? await DbSet.AsNoTracking()
                             .Where(predicate: item => item.IsDeleted == false)
                             .ToListAsync()
                : DbSet.AsNoTracking()
                       .Where(predicate: item => item.IsDeleted == false)
                       .Where(predicate)
                       .ToList();
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            try
            {
                entity.IsDeleted = true;

                // _ = DbSet.Remove(entity);
                _ = DbSet.Update(entity);
                _ = await SaveChangesAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                _ = DbSet.Update(entity);
                _ = await SaveChangesAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try { _ = await DbContext.SaveChangesAsync(); }
            catch { return false; }
            return true;
        }

        public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool ignoreDeleted = false)
        {
            return ignoreDeleted switch
            {
                true => await DbSet.SingleOrDefaultAsync(predicate),
                false => await DbSet.Where(predicate: item => item.IsDeleted == false)
                                    .SingleOrDefaultAsync(predicate),
            };
        }

        public async Task<bool> AddCollectionAsync(ICollection<TEntity> collection)
        {
            try
            {
                await DbSet.AddRangeAsync(entities: collection);
                _ = await SaveChangesAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> RemoveCollectionAsync(ICollection<TEntity> collection)
        {
            try
            {
                DbSet.RemoveRange(entities: collection);
                _ = await SaveChangesAsync();
            }
            catch { return false; }
            return true;
        }

        public async Task<bool> UpdateCollectionAsync(ICollection<TEntity> collection)
        {
            try
            {
                DbSet.UpdateRange(entities: collection);
                _ = await SaveChangesAsync();
            }
            catch { return false; }
            return true;
        }
    }
}